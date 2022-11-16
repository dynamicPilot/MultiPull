using Mirror;
using MP.Game.Players;
using MP.Room.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MP.Manager
{
    public class NetworkRoomManagerExtended : NetworkRoomManager
    {
        public GameObject ReturnToCanvas;
        public List<PlayerStats> Players = new List<PlayerStats>();

        public event Action OnConnectionError;

        bool _canStart;
        bool _isRestarting;

        public override void OnClientDisconnect()
        {
            OnConnectionError?.Invoke();
            base.OnClientDisconnect();
        }

        public override void OnRoomServerPlayersReady()
        {
            UpdateFirstPlayerWhenAllIsReady();
            _canStart = true;
        }

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            if ((sceneName == GameplayScene || sceneName == RoomScene) && NetworkServer.active)
            {
                NetworkServer.Spawn(UnityEngine.Object.Instantiate(ReturnToCanvas));
            }                       
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            var playerStats = gamePlayer.GetComponent<PlayerStats>();
            var roomPlayerStats = roomPlayer.GetComponent<NetworkRoomPlayerExtended>();

            playerStats.Index = roomPlayerStats.index;
            playerStats.PlayerName = roomPlayerStats.PlayerName;

            Players.Add(gamePlayer.GetComponent<PlayerStats>());

            return true;
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();

            if (IsSceneActive(GameplayScene))
            {
                ResetPlayers();
            }
            
        }

        private void ResetPlayers()
        {
            //Debug.Log("Try Reset Players for "+ Players.Count);
            for (int i = 0; i < Players.Count; i++)
            {
                // reset player
                Players[i].InRestart = !Players[i].InRestart;
                Players[i].Score = 0;
                Players[i].IsWinner = false;
                Players[i].GetComponent<PlayerTrigger>().IsRestarting = false;

                // reset position
                Transform startPos = GetStartPosition();
                Players[i].GetComponent<Transform>().position = startPos.position;
            }
        }

        private void BlockPlayers()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].GetComponent<PlayerTrigger>().IsRestarting = false;
            }
        }
        

        public void StartGame()
        {
            if (allPlayersReady && _canStart)
            {
                _canStart = false;
                ServerChangeScene(GameplayScene);
            }
        }

        private void UpdateFirstPlayerWhenAllIsReady()
        {
            var extendedPlayer = roomSlots[0] as NetworkRoomPlayerExtended;
            extendedPlayer.ActivateStartButton();
        }

        public void HostStopServer()
        {
            if (_isRestarting) StopAllCoroutines();
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                StopHost();
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                StopClient();
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                StopServer();
            }

        }

        public void RestartGame(float timer)
        {
            if (_isRestarting) return;

            _isRestarting = true;
            BlockPlayers();
            StartCoroutine(CountTimeToRestart(timer));
        }

        private IEnumerator CountTimeToRestart(float timer)
        {           
            yield return new WaitForSeconds(timer);            
            OnRestartGame();
        }

        private void OnRestartGame()
        {
            _isRestarting = false;
            ServerChangeScene(GameplayScene);
        }        
    }
}

