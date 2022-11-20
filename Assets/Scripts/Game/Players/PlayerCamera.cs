using Cinemachine;
using Mirror;
using MP.Common;
using UnityEngine;


namespace MP.Game.Players
{
    public class PlayerCamera : NetworkBehaviour
    {
        [SerializeField] private GameObject _cameraObject;

        public override void OnStartAuthority()
        {
            _cameraObject.SetActive(true);
        }

    }
}

