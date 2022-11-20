using Mirror;
using MP.Manager;
using UnityEngine;
using PlayerMovement = MP.Game.Movements.PlayerMovement;
using Debug = UnityEngine.Debug;

namespace MP.Game.Players 
{
    public class GamePlayerObject : NetworkBehaviour
    {
        [SerializeField] private PlayerStats _stats;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerTrigger _trigger;


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

        private void OnDisable()
        {
            NetworkServer.Destroy(gameObject);
        }

    }
}


