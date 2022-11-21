using Mirror;
using MP.Common.Players;
using MP.Game.Players;
using MP.Game.SpawnAndControlSystems;
using MP.Room.Players;
using System;
using System.Collections;
using UnityEngine;


namespace MP.Manager
{
    public class NetworkRoomManagerExtended : NetworkRoomManager
    {
        [SerializeField] private GameObject _returnToCanvasPrefab;
        [SerializeField] private GameObject _spawnSystemPrefab;

        public event Action OnConnectionError;
        public event Action<NetworkConnection> OnGameServerReady;
        public event Action OnRestartingStart;
        public event Action OnRestartingEnd;

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

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);

            if (IsSceneActive(GameplayScene) && conn != null && conn.identity != null)
            {
                OnGameServerReady?.Invoke(conn);
            }
        }

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            // spawn UI canvas for server only
            if ((sceneName == GameplayScene || sceneName == RoomScene) && NetworkServer.active)
                NetworkServer.Spawn(Instantiate(_returnToCanvasPrefab));

            // spawn players object spawner
            if (sceneName == GameplayScene && NetworkServer.active)
            {
                var spawnSystemObject = Instantiate(_spawnSystemPrefab);
                var spawnSystem = spawnSystemObject.GetComponent<SpawnSystem>();
                spawnSystem.AddSpawnPoints(startPositions.ToArray());

                NetworkServer.Spawn(spawnSystemObject);
            }
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            // transfer data from a room player to a game player
            var netGamePlayer = gamePlayer.GetComponent<NetworkGamePlayer>();
            var netRoomPlayer = roomPlayer.GetComponent<NetworkRoomPlayerExtended>();
            netGamePlayer.PlayerName = netRoomPlayer.PlayerName;
            return true;
        }

        #endregion

        public void OnGamePlayerObjectLoadedForPlayer(GameObject gamePlayer, GameObject playerObject)
        {
            // transfer data from a game player to the current player's object
            var netGamePlayer = gamePlayer.GetComponent<NetworkGamePlayer>();
            var playerStats = playerObject.GetComponent<PlayerStats>();
            playerStats.PlayerName = netGamePlayer.PlayerName;
        }

        private void UpdateFirstPlayerWhenAllIsReady()
        {
            var extendedPlayer = roomSlots[0] as NetworkRoomPlayerExtended;
            extendedPlayer.ActivateStartButton();
        }

        public void StartGame()
        {
            if (allPlayersReady && _canStart)
            {
                _canStart = false;
                ServerChangeScene(GameplayScene);
            }
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
            if (IsRestarting) return;

            IsRestarting = true;
            OnRestartingStart?.Invoke();

            StartCoroutine(CountTimeToRestart(timer));
        }

        private IEnumerator CountTimeToRestart(float timer)
        {
            yield return new WaitForSeconds(timer);            
            OnRestartGame();
        }

        private void OnRestartGame()
        {
            OnRestartingEnd?.Invoke();
            IsRestarting = false;

            ServerChangeScene(GameplayScene);
        }
    }
}

