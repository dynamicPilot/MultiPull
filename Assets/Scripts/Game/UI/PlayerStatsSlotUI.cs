using MP.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MP.Game.UI
{
    public class PlayerStatsSlotUI : MonoBehaviour
    {
        [SerializeField] private StaticSceneData _sceneData;

        [Header("UI Elements")]
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _sliderFill;

        private bool _isLocalPlayer = false;

        private void Awake()
        {
            _slider.maxValue = _sceneData.ScoreToWin;
            _slider.minValue = 0;
            _slider.wholeNumbers = true;
        }

        public void SetSlot(string name, bool isLocalPlayer)
        {
            Debug.Log("Set slot to " + name);
            _name.text = name;
            _isLocalPlayer = isLocalPlayer;

            _slider.value = _slider.minValue;

            _sliderFill.color = (_isLocalPlayer) ? _sceneData.SliderFillColorForActivePlayer
                : _sceneData.SliderFillColor;
            //gameObject.SetActive(true);
        }

        public void UpdateValue(int newValue)
        {
            Debug.Log("Set slot new value " + newValue);
            if (newValue > _slider.maxValue) newValue = (int) _slider.maxValue;
            _slider.value = newValue;
        }

        public void UpdateName(string newValue)
        {
            _name.text = newValue;
        }

    }

}
