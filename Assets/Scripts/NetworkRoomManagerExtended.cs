using Mirror;
using MP.Common.Players;
using MP.Game.Movements;
using MP.Game.Players;
using MP.Game.SpawnSystems;
using MP.Room.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MP.Manager
{
    public class NetworkRoomManagerExtended : NetworkRoomManager
    {
        [SerializeField] private GameObject _returnToCanvasPrefab;
        [SerializeField] private GameObject _spawnSystemPrefab;

        //public List<NetworkGamePlayer> GameSlots = new List<NetworkGamePlayer>();

        public event Action OnConnectionError;
        public event Action<NetworkConnection> OnGameServerReady;

        SpawnSystem _spawnSystem;

        bool _canStart;
        bool IsRestarting;
        public override void OnClientDisconnect()
        {
            OnConnectionError?.Invoke();
            base.OnClientDisconnect();
        }

        #region Server
        public override void OnRoomServerPlayersReady()
        {
            UpdateFirstPlayerWhenAllIsReady();
            _canStart = true;
        }

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            // spawn UI canvas for server only
            if ((sceneName == GameplayScene || sceneName == RoomScene) && NetworkServer.active)
            {
                NetworkServer.Spawn(Instantiate(_returnToCanvasPrefab));
            }  
            
            // spawn players object spawner
            if (sceneName == GameplayScene && NetworkServer.active)
            {
                var spawnSystemObject = Instantiate(_spawnSystemPrefab);
                _spawnSystem = spawnSystemObject.GetComponent<SpawnSystem>();
                _spawnSystem.AddSpawnPoints(startPositions.ToArray());
                NetworkServer.Spawn(spawnSystemObject);
            }
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);

            if (IsSceneActive(GameplayScene) && conn != null && conn.identity != null)
            {
                OnGameServerReady?.Invoke(conn);
            }
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            var netGamePlayer = gamePlayer.GetComponent<NetworkGamePlayer>();
            var netRoomPlayer = roomPlayer.GetComponent<NetworkRoomPlayerExtended>();
            netGamePlayer.PlayerName = netRoomPlayer.PlayerName;
            return true;
        }

        #endregion

        public void OnGamePlayerObjectLoadedForPlayer(GameObject gamePlayer, GameObject playerObject)
        {
            var netGamePlayer = gamePlayer.GetComponent<NetworkGamePlayer>();
            var playerStats = playerObject.GetComponent<PlayerStats>();
            playerStats.PlayerName = netGamePlayer.PlayerName;
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
                return;
            }

            IsRestarting = true;
            StartCoroutine(CountTimeToRestart(timer));
        }

        private IEnumerator CountTimeToRestart(float timer)
        {
            yield return new WaitForSeconds(timer);            
            OnRestartGame();
        }

        private void OnRestartGame()
        {
            DisabledPlayersObjects();
            IsRestarting = false;
            ServerChangeScene(GameplayScene);
        }
        
        public void DisabledPlayersObjects()
        {
            if (_spawnSystem != null) _spawnSystem.DisablePlayers();
        }
    }
}

