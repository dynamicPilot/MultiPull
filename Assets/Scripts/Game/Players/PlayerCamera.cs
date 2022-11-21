using Mirror;
using UnityEngine;


namespace MP.Game.Players
{
    /// <summary>
    /// This is a script to activate Camera in OnStartAuthority.
    /// </summary>
    public class PlayerCamera : NetworkBehaviour
    {
        [SerializeField] private GameObject _cameraObject;

        public override void OnStartAuthority()
        {
            _cameraObject.SetActive(true);
        }

    }
}

