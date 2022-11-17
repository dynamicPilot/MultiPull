using Mirror;
using MP.Common;
using MP.Game.Players;
using MP.Game.Input;
using System;
using UnityEngine;
using MP.Manager;
using UnityEngine.UIElements;

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

        [SyncVar(hook = nameof(SyncStartPosition))]
        [SerializeField] public Vector3 StartPosition;

        CharacterController _characterController = null;
        bool _nextPull = false;
        bool _previousPull = false;
        bool _forceMove = false;

        Vector3 _forceMovePosition;
        float _angle;
        int _pullDelayCount = 0;

        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                Debug.Log("Start Local player");
                _inputControls.OnPullPerformed += NextMoveIsPull;
                _characterController = gameObject.AddComponent<CharacterController>();
                //_characterController.enabled = true;
            }
        }

        public override void OnStopLocalPlayer()
        {
            _inputControls.OnPullPerformed -= NextMoveIsPull;
            _characterController.enabled = false;
            
        }
        private void SyncStartPosition(Vector3 oldValue, Vector3 newValue)
        {
            Debug.Log("SyncStartPosition");
            ForceMoveToStartPosition();
        }

        public void ForceMoveToStartPosition()
        {
            Debug.Log("ForceMoveToPosition");
            _forceMove = true;
            if (_characterController != null) _characterController.enabled = false;
        }

        private void FixedUpdate()
        {
            if (_forceMove)
            {
                ForceMove();
                return;
            }

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

        private void ForceMove()
        {
            Debug.Log("ForceMove finished");
            transform.position = StartPosition;

            _forceMove = false;
            if (_characterController != null) _characterController.enabled = true;
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
            Debug.Log("Start Pull");
        }

        private void PrevMoveIsPull()
        {
            if (_pullDelayCount-- > 0) return;
            _previousPull = false;
            _playerPullState.CmdInAPullValue(false);
            Debug.Log("Stop Pull");
        }

    }
}



