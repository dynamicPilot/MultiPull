using MP.Common;
using MP.Lobby.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MP.Room.UI
{
    public class RoomPlayerUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _playerPanel;
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private GameObject _startButton;

        [Header("Parameters")]
        [SerializeField] private StaticRoomPlayerData _playerData;

        private bool _ready = false;
        [SerializeField] private string _playerName = "player";

        public void SetViewActiveState(bool newState)
        {
            _playerPanel.SetActive(newState);
        }

        public void SetStartButtonState(bool newState)
        {
            _startButton.SetActive(newState);
        }

        public void SetPlayerName(string playerName)
        {
            if (!string.IsNullOrEmpty(playerName))
                _playerName = playerName;

            UpdateView();
        }

        public void ChangeState(bool isReady)
        {
            _ready = isReady;
            UpdateView();
        }

        private void UpdateView()
        {
            //_buttonText.text = (_ready) ? "Stop" : "Ready";
            //_buttonImage.color = (_ready) ? _playerData.ReadyColor : _playerData.NotReadyColor;
            _headerText.text = (_ready) ? _playerData.ReadyText : string.Format(_playerData.NotReadyText, _playerName) ;
        }

    }

}
