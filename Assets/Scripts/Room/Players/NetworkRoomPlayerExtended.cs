using Mirror;
using MP.Common;
using MP.Manager;
using MP.Room.UI;
using UnityEngine;


namespace MP.Room.Players
{
    /// <summary>
    /// This is a main component for Player in a Room.
    /// <para>SyncVar: PlayerName.</para>
    /// <para>Extends NetworkRoomPlayer.</para>
    /// </summary>
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

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            if (!newReadyState) _playerUI.SetStartButtonState(false);
        }


        private void SyncPlayerName(string oldValue, string newValue)
        {
            PlayerName = newValue;
            _playerUI.SetPlayerName(newValue);
        }

        [Command]
        public void CmdPlayerNameValue(string newPlayerName)
        {
            PlayerName = newPlayerName;
        }

        private string GetPlayerNameFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(_sceneData.NameKey)) return PlayerPrefs.GetString(_sceneData.NameKey);
            return _sceneData.DefaultPlayerName;
        }

        public void ActivateStartButton()
        {
            _playerUI.SetStartButtonState(true);            
        }

        // used by Button Conponent
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

