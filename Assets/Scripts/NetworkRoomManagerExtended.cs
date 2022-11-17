using Mirror;
using MP.Game.Movements;
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
        public List<Vector3> _usedPositions = new List<Vector3>();

        public PlayerStats hostPlayer;
        public event Action OnConnectionError;

        bool _canStart;
        public bool IsRestarting;
        bool _firstGameSceneLoad = false;
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
            _usedPositions.Add(gamePlayer.transform.position);

            playerStats.PlayerName = roomPlayerStats.PlayerName;
            Players.Add(gamePlayer.GetComponent<PlayerStats>());

            return true;
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);

            if (IsSceneActive(GameplayScene))
            {
                //if (hostPlayer == null) hostPlayer = NetworkServer.localConnection.identity.gameObject.GetComponent<PlayerStats>();
                //var movement = hostPlayer.gameObject.GetComponent<PlayerMovement>();

                //hostPlayer.InRestart = !hostPlayer.InRestart;
                //hostPlayer.Score = 0;
                //hostPlayer.IsWinner = false;
                //hostPlayer.GetComponent<PlayerTrigger>().IsRestarting = false;
                //Vector3 startPos = startPositions[UnityEngine.Random.Range(0, startPositions.Count)].position;

                //while (_usedPositions.Contains(startPos))
                //    startPos = startPositions[UnityEngine.Random.Range(0, startPositions.Count)].position;

                //_usedPositions.Add(startPos);
                //movement.StartPosition = startPos;
                //movement.ForceMoveToStartPosition();
            }
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
            if (_firstGameSceneLoad)
            {
                _firstGameSceneLoad = false;
                return;
            }

            Debug.Log("reset players for " + Players.Count);
            List<int> playersToRemove = new List<int>();
            List<Vector3> levelStartPosition = new List<Vector3>();

            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i] == null)
                {
                    playersToRemove.Add(i);
                    continue;
                }

                // reset player
                Players[i].InRestart = !Players[i].InRestart;
                Players[i].Score = 0;
                Players[i].IsWinner = false;
                Players[i].GetComponent<PlayerTrigger>().IsRestarting = false;

                // reset position
                var movement = Players[i].GetComponent<PlayerMovement>();
                Vector3 startPos = startPositions[UnityEngine.Random.Range(0, startPositions.Count)].position;

                while (_usedPositions.Contains(startPos) || levelStartPosition.Contains(startPos))
                    startPos = startPositions[UnityEngine.Random.Range(0, startPositions.Count)].position;

                movement.StartPosition = startPos;
                levelStartPosition.Add(startPos);
            }

            if (_firstGameSceneLoad) _firstGameSceneLoad = false;
            for (int i = 0; i < playersToRemove.Count; i++) Players.RemoveAt(i);
            _usedPositions.Clear();
            _usedPositions.AddRange(levelStartPosition);
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
                _firstGameSceneLoad = true;
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
            if (IsRestarting) StopAllCoroutines();

            OnRestartGame();
            return;
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
            if (IsRestarting) return;

            IsRestarting = true;
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
            IsRestarting = false;
            ServerChangeScene(GameplayScene);
        }        
    }
}

