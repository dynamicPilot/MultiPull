using Mirror;
using MP.Common;
using MP.Common.UI;
using MP.Game.Players.UI;
using MP.Game.UI;
using UnityEngine;


namespace MP.Game.Players
{
    public class PlayerStats : NetworkBehaviour
    {
        [SerializeField] private StaticSceneData _sceneData;
        [SerializeField] private PlayerNameUI _playerNameUI;
        [SerializeField] private PlayersStatsUI _playerStatsUI;

        [SyncVar(hook = nameof(SyncScore))]
        public int Score;

        [SyncVar(hook = nameof(SyncName))]
        public string PlayerName;

        [SyncVar(hook = nameof(SyncIsWinner))]
        public bool IsWinner;

        [SyncVar(hook = nameof(SyncInRestart))]
        public bool InRestart;

        [SyncVar]
        public int Index;

        private int _slotIndex;
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        private void SyncScore(int oldValue, int newValue)
        {
            if (_playerStatsUI == null) FindGameUI();
            _playerStatsUI.UpdateSlotValue(_slotIndex, newValue);
        }

        private void SyncName(string oldValue, string newValue)
        {
            _playerNameUI.SetPlayerName(newValue);

            if (_playerStatsUI == null) FindGameUI();
            _playerStatsUI.UpdatePlayerName(_slotIndex, newValue);
        }

        private void SyncIsWinner(bool oldValue, bool newValue)
        {
            if (newValue) _playerStatsUI.GetComponent<EndGameUI>().EndGameStart(PlayerName);
        }

        private void SyncInRestart(bool oldValue, bool newValue)
        {
            FindGameUI();
        }


        private void FindGameUI()
        {            
            if (_playerStatsUI != null) return;
            _playerStatsUI = GameObject.FindGameObjectWithTag("GameUI").GetComponent<PlayersStatsUI>();

            _slotIndex = _playerStatsUI.AddSlotForPlayer(PlayerName, false);
        }
    }

}
