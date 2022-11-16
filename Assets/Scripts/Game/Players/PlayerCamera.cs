using Mirror;
using UnityEngine;


namespace MP.Game.Players
{
    public class PlayerCamera : NetworkBehaviour
    {
        [SerializeField] private GameObject _cameraObject;

        public override void OnStartLocalPlayer()
        {
            _cameraObject.SetActive(true);
        }

        public override void OnStopLocalPlayer()
        {
            _cameraObject.SetActive(false);
        }
    }
}

