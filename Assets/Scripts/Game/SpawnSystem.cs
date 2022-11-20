using Mirror;
using MP.Common;
using MP.Manager;
using System.Collections.Generic;
using UnityEngine;


namespace MP.Game.SpawnSystems
{
    public class SpawnSystem : NetworkBehaviour
    {
        [SerializeField] private StaticGamePlayerData _playerData;
        private List<Transform> _spawnPoints = new List<Transform>();
        private List<GameObject> _spawnObjects = new List<GameObject>();

        [Server]
        public void AddSpawnPoints(Transform[] points)
        {
            _spawnPoints.Clear();
            _spawnPoints.AddRange(points);
        }

        public override void OnStartServer()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended manager)
                manager.OnGameServerReady += SpawnPlayer;
        }

        private void OnDestroy()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended manager)
                manager.OnGameServerReady -= SpawnPlayer;
        }



        [Server]
        private void SpawnPlayer(NetworkConnection conn)
        {
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

            GameObject playerInstance = Instantiate(_playerData.PlayerPrefab,
                spawnPoint.position, spawnPoint.rotation);
            NetworkServer.Spawn(playerInstance, conn);

            if (NetworkManager.singleton is NetworkRoomManagerExtended manager)
                manager.OnGamePlayerObjectLoadedForPlayer(conn.identity.gameObject, playerInstance);

            _spawnObjects.Add(playerInstance);
            _spawnPoints.Remove(spawnPoint);

        }

        [Server]
        public void DisablePlayers()
        {
            for (int i = 0; i < _spawnObjects.Count; i++)
            {
                _spawnObjects[i].SetActive(false);
            }
        }
    }
}

