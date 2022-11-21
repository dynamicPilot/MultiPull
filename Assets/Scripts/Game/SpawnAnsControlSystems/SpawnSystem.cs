using Mirror;
using MP.Common;
using MP.Game.Players;
using MP.Manager;
using System.Collections.Generic;
using UnityEngine;


namespace MP.Game.SpawnAndControlSystems
{
    /// <summary>
    /// Controls players objects spawn in a game scene.
    /// <para>SpawnPlayer on NetworkRoomManagerExtended event OnGameServerReady.</para>
    /// </summary>
    [RequireComponent(typeof(PlayersObjectsControlSystem))]
    public class SpawnSystem : NetworkBehaviour
    {
        [SerializeField] private PlayersObjectsControlSystem _controlSystem;
        [SerializeField] private StaticGamePlayerData _playerData;

        private List<Transform> _spawnPoints = new List<Transform>();

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
        public void AddSpawnPoints(Transform[] points)
        {
            _spawnPoints.Clear();
            _spawnPoints.AddRange(points);
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

            _controlSystem.AddPlayerObject(playerInstance.GetComponent<GamePlayerObject>());

            _spawnPoints.Remove(spawnPoint);

        }
    }
}

