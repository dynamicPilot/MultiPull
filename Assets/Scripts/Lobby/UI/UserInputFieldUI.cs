using TMPro;
using UnityEngine;

namespace MP.Room.UI
{
    public class UserInputFieldUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        public void SetInputField(string text)
        {
            _inputField.text = text;
        }

        public string GetInputValue()
        {
            return _inputField.text;
        }

    }
}

