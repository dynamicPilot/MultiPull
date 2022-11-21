using TMPro;
using UnityEngine;

namespace MP.Game.Players.UI
{
    /// <summary>
    /// UI component to display Player name.
    /// </summary>
    public class PlayerNameUI : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private TMP_Text _text;

        private void OnValidate()
        {
            _canvas.SetActive(false);
        }

        public void SetViewActiveState(bool state)
        {
            _canvas.SetActive(state);
        }

        public void SetPlayerName(string playerName)
        {
            _text.text = playerName;
            
            SetViewActiveState(true);
        }

        public void CorrectViewPosition(Transform lookAtTarget)
        {
            _canvas.transform.LookAt(lookAtTarget);
        }
    }
}


