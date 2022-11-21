using Mirror;
using MP.Common;
using MP.Game.Players.UI;
using MP.Game.UI;
using UnityEngine;


namespace MP.Game.Players
{
    /// <summary>
    /// This is a script to store current player's score and name.
    /// <para>SyncVar: Score, PlayerName.</para>
    /// <para>Looking for PlayersStatsUI, EndGameUI with FindGameObjectWithTag("GameUI").</para>
    /// </summary>
    public class PlayerStats : NetworkBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private StaticSceneData _sceneData;

        [Header("UI Elements")]
        [SerializeField] private PlayerNameUI _playerNameUI;
        [SerializeField] private PlayersStatsUI _playerStatsUI;

        [SyncVar(hook = nameof(SyncScore))]
        public int Score;

        [SyncVar(hook = nameof(SyncName))]
        public string PlayerName;

        int _slotIndex;

        private void SyncScore(int oldValue, int newValue)
        {
            if (_playerStatsUI == null) FindGameUI();
            _playerStatsUI.UpdateSlotValue(_slotIndex, newValue);

            if (Score == _sceneData.ScoreToWin) Winner();
        }

        private void SyncName(string oldValue, string newValue)
        {
            _playerNameUI.SetPlayerName(newValue);

            if (_playerStatsUI == null) FindGameUI();
            _playerStatsUI.UpdatePlayerName(_slotIndex, newValue);
        }

        private void Winner()
        {
            _playerStatsUI.GetComponent<EndGameUI>().EndGameStart(PlayerName);
        }

        private void FindGameUI()
        {            
            if (_playerStatsUI != null) return;
            _playerStatsUI = GameObject.FindGameObjectWithTag("GameUI").GetComponent<PlayersStatsUI>();

            _slotIndex = _playerStatsUI.AddSlotForPlayer(PlayerName);
        }
    }

}
