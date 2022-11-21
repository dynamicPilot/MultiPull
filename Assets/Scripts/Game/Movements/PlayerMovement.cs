using Mirror;
using MP.Common;
using MP.Game.Players;
using MP.Game.Input;
using System;
using UnityEngine;

namespace MP.Game.Movements
{
    /// <summary>
    /// Component for calculation movements with InputControl, PlayerRotation and CharacterController.
    /// <para>Calls PlayerPullState with CmdInAPullValue to change pull state.</para>
    /// <para>Creates CharacterController on OnStartAuthority.</para>
    /// </summary>
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private PlayerRotation _playerRotation;
        [SerializeField] private PlayerPullState _playerPullState;
        [SerializeField] private InputControls _inputControls;

        [Header("Movement Parameters")]
        [SerializeField] private StaticGamePlayerData _playerData;
        [SerializeField] private float _minMoveDirectionValue = 0.1f;
        [SerializeField] private int _pullWillContinueFor = 50;
        
        CharacterController _characterController = null;        

        bool _nextPull = false;
        bool _previousPull = false;

        float _angle;
        int _pullDelayCount = 0;

        public override void OnStartAuthority()
        {
            _inputControls.enabled = true;
            _characterController = gameObject.AddComponent<CharacterController>();

            if (_inputControls != null) _inputControls.OnPullPerformed += NextMoveIsPull;
            _characterController.enabled = true;
        }

        private void OnDisable()
        {
            if (_inputControls != null) _inputControls.OnPullPerformed -= NextMoveIsPull;
            _inputControls.enabled = false;
        }

        private void FixedUpdate()
        {
            if (_characterController == null || !_characterController.enabled || _inputControls == null ||
                _playerData == null || _playerRotation == null)
            {
                return;
            }

            if (isClient) MakeMove();
        }

        [Client]
        private void MakeMove()
        {
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

        [Client]
        private Tuple<bool,Vector3> CheckDirection(Vector2 moveVector)
        {
            var direction = new Vector3(moveVector.x, 0f, moveVector.y).normalized;

            return (direction.magnitude <= _minMoveDirectionValue) 
                ? new Tuple<bool, Vector3> (false, Vector3.zero) 
                : new Tuple<bool, Vector3> (true, direction);
        }

        [Client]
        private Vector3 GetDirection(float angle)
        {           
            var correctedDirection = (Quaternion.Euler(0f, angle, 0f) * Vector3.forward).normalized;

            return (_nextPull) ? correctedDirection * _playerData.PullDistance :
                // if grouded
                (_characterController.isGrounded) ? correctedDirection * _playerData.Speed * Time.fixedDeltaTime
                // else -> move to the ground
                : new Vector3(correctedDirection.x * _playerData.Speed * Time.fixedDeltaTime,
                -transform.position.y * 100f, correctedDirection.z * _playerData.Speed * Time.fixedDeltaTime);
        }

        [Client]
        private void NextMoveIsPull()
        {
            _nextPull = true;
        }

        [Client]
        private void MoveIsPull()
        {
            _nextPull = false;
            _previousPull = true;
            _pullDelayCount = _pullWillContinueFor;
            if (_playerPullState!= null) _playerPullState.CmdInAPullValue(true);
        }
        [Client]
        private void PrevMoveIsPull()
        {
            if (_pullDelayCount-- > 0) return;
            _previousPull = false;
            if (_playerPullState != null) _playerPullState.CmdInAPullValue(false);
        }

    }
}



