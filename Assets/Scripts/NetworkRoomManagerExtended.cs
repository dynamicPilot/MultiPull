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

        public List<NetworkGamePlayer> GameSlots = new List<NetworkGamePlayer>();

        public event Action OnConnectionError;

        bool _canStart;
        public bool IsRestarting;
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
            playerStats.PlayerName = roomPlayerStats.PlayerName;
            return true;
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (IsSceneActive(GameplayScene))
            {
                OnGameSceneChanged();
            }

            base.OnServerSceneChanged(sceneName);
        }

        private void OnGameSceneChanged()
        {
            List<Vector3> usedPositions = new List<Vector3>();
            List<Vector3> levelStartPosition = new List<Vector3>();

            for (int i = 0; i < GameSlots.Count; i++)
            {
                usedPositions.Add(GameSlots[i].GetStartPosition());
            }

            for (int i = 0; i < GameSlots.Count; i++)
            {
                if (GameSlots[i] == null) continue;
                GameSlots[i].OnGameSceneChanged();
                
            }
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();

            if (IsSceneActive(GameplayScene))
            {
                if (NetworkClient.isConnected)
                    CallOnClientEnterGame();
            }
        }

        internal void CallOnClientEnterGame()
        {
            foreach (NetworkGamePlayer player in GameSlots)
                if (player != null)
                {
                    player.OnClientEnterGame();
                }
        }


        private void OnGameRestarting()
        {
            List<Vector3> usedPositions = new List<Vector3>();
            List<Vector3> levelStartPosition = new List<Vector3>();

            for (int i = 0; i < GameSlots.Count; i++)
            {
                usedPositions.Add(GameSlots[i].GetStartPosition());
            }

            for (int i = 0; i < GameSlots.Count; i++)
            {
                Vector3 startPos = startPositions[UnityEngine.Random.Range(0, startPositions.Count)].position;

                while (usedPositions.Contains(startPos) || levelStartPosition.Contains(startPos))
                    startPos = startPositions[UnityEngine.Random.Range(0, startPositions.Count)].position;

                levelStartPosition.Add(startPos);

                GameSlots[i].OnGameRestartingChange(startPos);
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
            if (IsRestarting) StopAllCoroutines();

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
            if (IsRestarting)
            {
                Debug.Log("Restarting");
                return;
            }
            IsRestarting = true;
            StartCoroutine(CountTimeToRestart(timer));
        }

        private IEnumerator CountTimeToRestart(float timer)
        {            
            OnGameRestarting();

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

