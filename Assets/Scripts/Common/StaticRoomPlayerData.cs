using UnityEngine;

namespace MP.Common
{
    [CreateAssetMenu(menuName = "StaticData/StaticRoomPlayerData", fileName = "StaticRoomPlayerData", order = 1)]
    public class StaticRoomPlayerData : ScriptableObject
    {
        [Header("Color")]
        public Color ReadyColor;
        public Color NotReadyColor;

        [Header("Text")]
        public string NotReadyText = "Are you ready, {0}?";
        public string ReadyText = "Okay! Just wait a minute ...";
    }
}
