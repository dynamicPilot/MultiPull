using Mirror;
using MP.Common;
using MP.Manager;
using UnityEngine;


namespace MP.Game.Players
{
    /// <summary>
    /// This is a script to detect Player's Collision with other Players, calls on server.
    /// <para>Detects collision with OnTriggerEnter. 
    /// Increments PlayerStats Score variable and calls RestartGame via NetworkRoomManagerExtended.</para>
    /// </summary>
    public class PlayerTrigger : NetworkBehaviour
    {
        [SerializeField] private StaticSceneData _sceneData;

        [ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerBody")) &&
                other.gameObject != gameObject)
            {
                var selfPullState = gameObject.GetComponent<PlayerPullState>();
                var playerCanBePulledState = other.gameObject.GetComponent<PlayerCanBePulledState>();

                if (playerCanBePulledState == null || selfPullState == null) return;


                if (selfPullState.InAPull && playerCanBePulledState.CanBePulled)
                    CanNotBePulledStateActivator(playerCanBePulledState);
            }
        }

        private void CanNotBePulledStateActivator(PlayerCanBePulledState player)
        {
            player.StartCanNotBePulledState();

            var selfStats = gameObject.GetComponent<PlayerStats>();

            if (selfStats != null)
            {
                selfStats.Score++;
                if (selfStats.Score == _sceneData.ScoreToWin) RestartGame();
            }
        }

        [Server]
        private void RestartGame()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended room)
                room.RestartGame(_sceneData.RestartTimer);
        }
    }
}
