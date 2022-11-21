using Mirror;
using UnityEngine;
using PlayerMovement = MP.Game.Movements.PlayerMovement;

namespace MP.Game.Players 
{
    /// <summary>
    /// This is a script to control Player's game object. Object will be destroyed on OnDisable.
    /// </summary>
    public class GamePlayerObject : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        private void OnDisable()
        {
            NetworkServer.Destroy(gameObject);
        }

        [Server]
        public void DisablePlayerObject()
        {
            gameObject.SetActive(false);
            RpcMakeObjectNotActive();
        }

        [ClientRpc]
        private void RpcMakeObjectNotActive()
        {
            gameObject.SetActive(false);
        }

        [Server]
        public void BlockPlayerMovement()
        {
            _movement.enabled = false;
            RpcBlockPlayerMovement();
        }

        [ClientRpc]
        private void RpcBlockPlayerMovement()
        {
            _movement.enabled = false;
        }

    }
}


