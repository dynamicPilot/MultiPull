using MP.Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MP.Game.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private PlayersStatsUI _playersStatsUI;
        [SerializeField] private EndGameUI _endGameUI;

        public void AddPlayerToStats(string playerName, bool isLocalPlayer, ref int index)
        {
            //_playersStatsUI.AddPlayerToStats(playerName, isLocalPlayer, ref index);
        }

        public void UpdateSlotValue(int index, int newValue)
        {
            //_playersStatsUI.UpdateSlotValue(index, newValue);
        }
    }
}
