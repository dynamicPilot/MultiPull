using Mirror;
using MP.Common;
using MP.Manager;
using MP.Room.UI;
using UnityEngine;


namespace MP.Room.Players
{
    public class NetworkRoomPlayerExtended : NetworkRoomPlayer
    {
        [SerializeField] private RoomPlayerUI _playerUI;
        [SerializeField] private StaticSceneData _sceneData;

        [SyncVar(hook = nameof(SyncPlayerName))] 
        public string PlayerName;

        public override void OnStartLocalPlayer()
        {
            CmdPlayerNameValue(GetPlayerNameFromPlayerPrefs());
            _playerUI.SetViewActiveState(true);
        }

        public override void OnClientExitRoom()
        {
            _playerUI.SetViewActiveState(false);           
        }


        private void SyncPlayerName(string oldValue, string newValue)
        {
            PlayerName = newValue;
            _playerUI.SetPlayerName(newValue);
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            if (!newReadyState) _playerUI.SetStartButtonState(false);
        }

        [Command]
        public void CmdPlayerNameValue(string newPlayerName)
        {
            PlayerName = newPlayerName;
        }


        public string GetPlayerNameFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(_sceneData.NameKey)) return PlayerPrefs.GetString(_sceneData.NameKey);
            return _sceneData.DefaultPlayerName;
        }

        public void ActivateStartButton()
        {
            _playerUI.SetStartButtonState(true);            
        }


        public void StartGame()
        {
            NetworkRoomManagerExtended room = NetworkManager.singleton as NetworkRoomManagerExtended;
            if (room != null)
            {
                room.StartGame();
                _playerUI.SetStartButtonState(false);
            }
        }
    }
}

