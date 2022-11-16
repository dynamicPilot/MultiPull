using MP.Common;
using MP.Room.Players;
using MP.Room.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MP.Lobby.UI
{
    public class ReadyButtonUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private Image _buttonImage;

        [Header("Parameters")]
        [SerializeField] private StaticRoomPlayerData _playerData;
        [SerializeField] private NetworkRoomPlayerExtended _roomPlayer;
        [SerializeField] private RoomPlayerUI _roomPlayerUI;

        //public event Action<bool> OnButtonStayChanged;

        bool _isReady = false;

        public void ResetState()
        {
            _isReady = false;
            UpdateView();
        }

        public void ChangeState()
        {
            _isReady = !_isReady;
            UpdateView();
            _roomPlayer.CmdChangeReadyState(_isReady);
            _roomPlayerUI.ChangeState(_isReady);
        }

        private void UpdateView()
        {
            _buttonText.text = (_isReady) ? "Stop" : "Ready";
            _buttonImage.color = (_isReady) ? _playerData.ReadyColor : _playerData.NotReadyColor;
            //_headerText.text = (_isReady) ? _playerData.ReadyText : string.Format(_playerData.NotReadyText, _playerName);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ChangeState();
        }
    }
}
