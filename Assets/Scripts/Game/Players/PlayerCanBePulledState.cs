using Mirror;
using MP.Common;
using System.Collections;
using UnityEngine;

namespace MP.Game.Players
{
    /// <summary>
    /// Component to store ans sync CanBePulled and performed logic for player's pulled state.
    /// <para>SyncVar:CanBePulled.</para>
    /// </summary>
    public class PlayerCanBePulledState : NetworkBehaviour
    {
        [SerializeField] private StaticGamePlayerData _playerData;
        [SerializeField] private PlayerColor _playerColor;

        [SyncVar]
        public bool CanBePulled = true;

        private void OnValidate()
        {
            CanBePulled = true;
        }

        public override void OnStopLocalPlayer()
        {
            StopAllCoroutines();
        }

        [ServerCallback]
        public void StartCanNotBePulledState()
        {
            if (!CanBePulled) return;
            StartCoroutine(CanNotBePulledState());
        }

        [ServerCallback]
        public IEnumerator CanNotBePulledState()
        {
            CanBePulled = false;
            _playerColor.Color = _playerData.HitColor;
            yield return new WaitForSeconds(_playerData.HitTime);

            _playerColor.Color = _playerData.InitialColor;
            CanBePulled = true;

        }

    }

}
