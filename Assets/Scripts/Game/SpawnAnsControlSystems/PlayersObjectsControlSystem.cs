using Mirror;
using MP.Game.Players;
using MP.Manager;
using System.Collections.Generic;

namespace MP.Game.SpawnAndControlSystems
{
    /// <summary>
    /// Controls players objects behavior in a game scene.
    /// <para>BlockPlayersMovement on NetworkRoomManagerExtended event OnRestartingStart.</para>
    /// <para>DisablePlayers on NetworkRoomManagerExtended event OnRestartingEnd.</para>
    /// </summary>
    public class PlayersObjectsControlSystem : NetworkBehaviour
    {
        private List<GamePlayerObject> _playersObjects = new List<GamePlayerObject>();

        public override void OnStartServer()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended manager)
            {
                manager.OnRestartingStart += BlockPlayersMovement;
                manager.OnRestartingEnd += DisablePlayers;
            }                
        }

        private void OnDestroy()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended manager)
            {
                manager.OnRestartingStart -= BlockPlayersMovement;
                manager.OnRestartingEnd -= DisablePlayers;
            }
        }

        [Server]
        public void AddPlayerObject(GamePlayerObject playerObject)
        {
            if (playerObject == null) return;
            _playersObjects.Add(playerObject);
        }

        [Server]
        public void BlockPlayersMovement()
        {
            for (int i = 0; i < _playersObjects.Count; i++)
                _playersObjects[i].BlockPlayerMovement();
        }

        [Server]
        public void DisablePlayers()
        {
            for (int i = 0; i < _playersObjects.Count; i++)
                _playersObjects[i].DisablePlayerObject();
        }
    }
}
