using UnityEngine;


namespace MP.Game.Movements
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private Transform _playerUI;
        [SerializeField] private float _smoothTime = 0.1f;
        private float _smoothVelocity;
        private Transform _cameraTransform;

        private void Start()
        {
            _playerUI.LookAt(_cameraTransform);
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
            _playerUI.LookAt(_cameraTransform);

            return angle;
        }
    }
}

