using MP.Common;
using MP.Room.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MP.Room.UI
{
    public class NameLoadAndSaveUI : MonoBehaviour
    {
        [SerializeField] private JoinUI _joinUI;
        [SerializeField] private UserInputFieldUI _inputFieldUI;
        [SerializeField] private Button _continueButton;
        [SerializeField] private GameObject _playerNamePanel;
        [SerializeField] private StaticSceneData _sceneData;
        


        public void SaveNameToPrefs()
        {
            string newName = _inputFieldUI.GetInputValue();

            if (string.IsNullOrEmpty(newName)) return;

            PlayerPrefs.SetString(_sceneData.NameKey, newName);
            _playerNamePanel.SetActive(false);
            _joinUI.ActivateUI();
        }
    }
}

