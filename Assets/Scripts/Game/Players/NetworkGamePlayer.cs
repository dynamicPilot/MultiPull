using Mirror;
using MP.Manager;
using UnityEngine;
using PlayerMovement = MP.Game.Movements.PlayerMovement;
using Debug = UnityEngine.Debug;

namespace MP.Game.Players 
{
    public class NetworkGamePlayer : NetworkBehaviour
    {
        [SerializeField] private PlayerStats _stats;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerTrigger _trigger;

        public void Start()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended manager)
            {
                if (manager.dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);

                manager.GameSlots.Add(this);
            }
           
        }

        public virtual void OnDisable()
        {
            if (NetworkClient.active && NetworkManager.singleton is NetworkRoomManagerExtended room)
            {
                room.GameSlots.Remove(this);
            }
        }

        [Server]
        public void OnGameSceneChanged()
        {
            _stats.InRestart = !_stats.InRestart;
            _stats.Score = 0;
            _trigger.IsRestarting = false;
        }

        [Server]
        public void OnGameRestartingChange(Vector3 startPoint)
        {
            _trigger.IsRestarting = true;
            _movement.StartPosition = startPoint;
        }

        public Vector3 GetStartPosition()
        {
            return _movement.StartPosition;
        }

        public void OnClientEnterGame()
        {
            //update owner only
            if (isOwned)
            {
                transform.position = _movement.StartPosition;
                //Debug.Log($"OWNER : Change position for {_stats.PlayerName}: " +
                //                $" new start position {_movement.StartPosition} and actual position is {transform.position}.");
            }
        }
    }
}


