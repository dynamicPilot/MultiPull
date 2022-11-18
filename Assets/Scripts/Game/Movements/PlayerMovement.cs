using Mirror;
using MP.Common;
using MP.Game.Players;
using MP.Game.Input;
using System;
using UnityEngine;

namespace MP.Game.Movements
{
    [RequireComponent(typeof(NetworkTransform))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private InputControls _inputControls;
        
        [SerializeField] private PlayerRotation _playerRotation;
        [SerializeField] private PlayerPullState _playerPullState;

        [Header("Movement Parameters")]
        [SerializeField] private StaticGamePlayerData _playerData;
        [SerializeField] private float _minMoveDirectionValue = 0.1f;
        [SerializeField] private int _pullWillContinueFor = 50;

        [SyncVar]
        [SerializeField] public Vector3 StartPosition;

        CharacterController _characterController = null;
        bool _nextPull = false;
        bool _previousPull = false;

        float _angle;
        int _pullDelayCount = 0;

        public override void OnStartLocalPlayer()
        {
            _inputControls.OnPullPerformed += NextMoveIsPull;
            _characterController = gameObject.AddComponent<CharacterController>();
            _characterController.enabled = true;
        }

        public override void OnStopLocalPlayer()
        {
            _inputControls.OnPullPerformed -= NextMoveIsPull;
            _characterController.enabled = false;
            
        }

        private void FixedUpdate()
        {

            if (!isLocalPlayer || _characterController == null || !_characterController.enabled) return;

            // check direction
            var checkResult = CheckDirection(_inputControls.GetMoveValue());

            if (!checkResult.Item1 && !_nextPull && !_previousPull) return;

            // rotate
            _angle = _playerRotation.GetRotationAngle(checkResult.Item2, !_nextPull);

            // move
            _characterController.Move(GetDirection(_angle));

            if (_previousPull && !_nextPull) PrevMoveIsPull();

            if (_nextPull) MoveIsPull();

        }

        private Tuple<bool,Vector3> CheckDirection(Vector2 moveVector)
        {
            var direction = new Vector3(moveVector.x, 0f, moveVector.y).normalized;

            return (direction.magnitude <= _minMoveDirectionValue) 
                ? new Tuple<bool, Vector3> (false, Vector3.zero) 
                : new Tuple<bool, Vector3> (true, direction);
        }

        private Vector3 GetDirection(float angle)
        {           
            var correctedDirection = (Quaternion.Euler(0f, angle, 0f) * Vector3.forward).normalized;

            return (_nextPull) ? correctedDirection * _playerData.PullDistance :
                (_characterController.isGrounded) ? correctedDirection * _playerData.Speed * Time.fixedDeltaTime
                : new Vector3(correctedDirection.x * _playerData.Speed * Time.fixedDeltaTime,
                -transform.position.y, correctedDirection.z * _playerData.Speed * Time.fixedDeltaTime);
        }
        private void NextMoveIsPull()
        {
            _nextPull = true;
        }

        private void MoveIsPull()
        {
            _nextPull = false;
            _previousPull = true;
            _pullDelayCount = _pullWillContinueFor;
            _playerPullState.CmdInAPullValue(true);
        }

        private void PrevMoveIsPull()
        {
            if (_pullDelayCount-- > 0) return;
            _previousPull = false;
            _playerPullState.CmdInAPullValue(false);
        }

    }
}



