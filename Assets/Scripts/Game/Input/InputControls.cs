using MP.InputSystems;
using MP.Game.Input.Interfaces;
using UnityEngine;

namespace MP.Game.Input
{ 
    public class InputControls : MonoBehaviour, IGetMoveValue, IPullPerformed
    {
        private Controls _controls;

        public event IPullPerformed.PullPerformed OnPullPerformed;

        private void Awake()
        {
            _controls = new Controls();
        }

        private void Start()
        {
            _controls.PlayerMap.PullAction.performed += ctx => OnPullPerformed?.Invoke();
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        public Vector2 GetMoveValue()
        {
            return _controls.PlayerMap.MoveAction.ReadValue<Vector2>();
        }

    }

}
