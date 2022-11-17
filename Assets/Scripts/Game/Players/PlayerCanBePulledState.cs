using Mirror;
using MP.Common;
using System.Collections;
using UnityEngine;

namespace MP.Game.Players
{
    public class PlayerCanBePulledState : NetworkBehaviour
    {
        [SerializeField] private StaticGamePlayerData _playerData;
        [SerializeField] private PlayerColor _playerColor;

        [SyncVar(hook = nameof(SyncCanBePulled))]
        public bool CanBePulled = true;

        private void OnValidate()
        {
            CanBePulled = true;
        }

        public override void OnStopLocalPlayer()
        {
            StopAllCoroutines();
        }

        public void SyncCanBePulled(bool oldValue, bool newValue)
        {
            Debug.Log("Can be pulled changed from " + oldValue +" to " + newValue);
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
