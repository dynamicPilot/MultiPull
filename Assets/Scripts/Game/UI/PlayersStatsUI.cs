using Mirror;
using MP.Common;
using MP.Common.UI;
using MP.Game.Players;
using MP.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Game.UI
{
    public class PlayersStatsUI : MonoBehaviour
    {
        [SerializeField] private StaticSceneData _sceneData;
        [SerializeField] private Transform _slotsParent;
        [SerializeField] private EndGameUI _endGameUI;

        private IDictionary<int, PlayerStatsSlotUI> _slots = new Dictionary<int, PlayerStatsSlotUI>();

        private void OnValidate()
        {
            _slotsParent.gameObject.SetActive(true);
        }

        //private void Start()
        //{
        //    if (NetworkManager.singleton is NetworkRoomManagerExtended room)
        //        CreateSlotsForPlayers(room.Players.ToArray());
        //}

        //public void CreateSlotsForPlayers(PlayerStats[] playersStats)
        //{
        //    Debug.Log("Create slots for players " + playersStats.Length);
        //    for (int i = 0; i < playersStats.Length; i++)
        //    {
        //        AddSlotForPlayer(playersStats[i].Index, playersStats[i].PlayerName);
        //    }
        //}

        //public void AddSlotForPlayer(int index, string playerName)
        //{
        //    PlayerStatsSlotUI slot = CreateSlot();
        //    slot.SetSlot(playerName, false);
        //    _slots.Add(index, slot);
        //}

        //private void Start()
        //{
        //    _endGameUI.OnEndGameStart += OnEndGameStart;
        //}

        //private void OnDestroy()
        //{
        //    _endGameUI.OnEndGameStart -= OnEndGameStart;
        //}

        public int AddSlotForPlayer(string playerName, bool isLocalPlayer)
        {

            PlayerStatsSlotUI slot = CreateSlot();

            int index = _slots.Count;
            slot.SetSlot(playerName, isLocalPlayer);
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

