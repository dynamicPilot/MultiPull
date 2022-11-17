using MP.Game.UI;
using UnityEngine;

namespace MP.Common
{
    [CreateAssetMenu(menuName = "StaticData/StaticSceneData", fileName = "StaticSceneData", order = 0)]
    public class StaticSceneData : ScriptableObject
    {
        [Header("End Game Parameters")]
        public int ScoreToWin = 3;
        public int RestartTimer = 5;
        public string EndGameWinnerMessage = "{0} is a Winner!";
        public string RestartCounterText = "Restart after: {0}";

        [Header("Player's Name Parameters")]
        public string NameKey = "PlayerName";
        public string DefaultPlayerName = "Nemo";

        [Header("GameScene UI Parameters")]
        public GameObject StatsSlotPrefab;
        public Color SliderFillColor;

        private void OnValidate()
        {
            if (StatsSlotPrefab == null) 
                Debug.LogError("StaticSceneData: no StatsSlotPrefab");
            else if (StatsSlotPrefab.GetComponent<PlayerStatsSlotUI>() == null)
                Debug.LogError("StaticSceneData: no PlayerStatsSlotUI in StatsSlotPrefab");
        }
    }
}

