using MP.Common;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Game.UI
{
    public class PlayersStatsUI : MonoBehaviour
    {
        [SerializeField] private StaticSceneData _sceneData;
        [SerializeField] private Transform _slotsParent;

        private IDictionary<int, PlayerStatsSlotUI> _slots = new Dictionary<int, PlayerStatsSlotUI>();

        private void OnValidate()
        {
            _slotsParent.gameObject.SetActive(true);
        }

        public int AddSlotForPlayer(string playerName)
        {

            PlayerStatsSlotUI slot = CreateSlot();

            int index = _slots.Count;
            slot.SetSlot(playerName);
            _slots.Add(index, slot);

            if (!_slotsParent.gameObject.activeSelf) _slotsParent.gameObject.SetActive(true);
            return index;
        }

        public bool UpdatePlayerName(int index, string newValue)
        {
            if (!_slots.ContainsKey(index)) return false;
            _slots[index].UpdateName(newValue);
            return true;
        }

        public bool UpdateSlotValue(int index, int newValue)
        {
            if (!_slots.ContainsKey(index)) return false;
            _slots[index].UpdateValue(newValue);
            return true;
        }

        public void OnEndGameStart()
        {
            _slotsParent.gameObject.SetActive(false);
        }
        
        private PlayerStatsSlotUI CreateSlot()
        {
            return Instantiate(_sceneData.StatsSlotPrefab, _slotsParent).GetComponent<PlayerStatsSlotUI>();            
        }
    }
}

