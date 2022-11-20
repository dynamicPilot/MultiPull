using MP.Game.Players.UI;
using UnityEngine;


namespace MP.Game.Movements
{
    [RequireComponent(typeof(PlayerNameUI))]
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] public PlayerNameUI _playerUI;
        [SerializeField] private float _smoothTime = 0.1f;

        Transform _cameraTransform;

        float _smoothVelocity;
        private void Start()
        {
            if (_cameraTransform == null) GetCameraTransform();
            _playerUI.CorrectViewPosition(_cameraTransform);
        }

        private void GetCameraTransform() => _cameraTransform = Camera.main.transform;

        public float GetRotationAngle(Vector3 direction, bool needSmooth)
        {
            if (_cameraTransform == null) GetCameraTransform();

            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            var smoothAngle = (needSmooth) ? Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _smoothVelocity, _smoothTime)
                : angle;

            // rotate
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            _playerUI.CorrectViewPosition(_cameraTransform);

            return angle;
        }
    }
}

