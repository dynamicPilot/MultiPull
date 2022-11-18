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

        private void Awake()
        {
            _slider.maxValue = _sceneData.ScoreToWin;
            _slider.minValue = 0;
            _slider.wholeNumbers = true;
        }

        public void SetSlot(string name)
        {
            _name.text = name;

            _slider.value = _slider.minValue;

            _sliderFill.color = _sceneData.SliderFillColor;
        }

        public void UpdateValue(int newValue)
        {
            if (newValue > _slider.maxValue) newValue = (int) _slider.maxValue;
            _slider.value = newValue;
        }

        public void UpdateName(string newValue)
        {
            _name.text = newValue;
        }

    }

}
