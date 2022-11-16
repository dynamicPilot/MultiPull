using TMPro;
using UnityEngine;


namespace MP.Common.UI
{
    public class EndGameUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject _endGamePanel;
        [SerializeField] private TMP_Text _winnerText;

        [Header("Parameters")]
        [SerializeField] private StaticSceneData _sceneData;


        private void OnValidate()
        {
            _endGamePanel.SetActive(false);
        }

        public void EndGameStart(string playerName)
        {
            _winnerText.text = string.Format(_sceneData.EndGameWinnerMessage, playerName);
            _endGamePanel.SetActive(true);
        }

    }
}

