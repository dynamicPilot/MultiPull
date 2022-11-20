using UnityEngine;

namespace MP.Common
{
    [CreateAssetMenu(menuName = "StaticData/StaticGamePlayerData", fileName = "StaticGamePlayerData", order = 2)]
    public class StaticGamePlayerData : ScriptableObject
    {
        [Header("Color Parameters")]
        public Color HitColor;
        public Color InitialColor;

        [Header("Movement Parameters")]
        public float HitTime = 3f;
        public float Speed = 2f;
        public float PullDistance = 3f;

        [Header("Local Player Components")]
        public GameObject CameraPrefab;
        public GameObject PlayerPrefab;
    }
}
