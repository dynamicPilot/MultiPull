using UnityEngine;

namespace MP.Common
{
    [CreateAssetMenu(menuName = "StaticData/StaticGamePlayerData", fileName = "StaticGamePlayerData", order = 2)]
    public class StaticGamePlayerData : ScriptableObject
    {
        [Header("color Parameters")]
        public Color HitColor;
        public Color InitialColor;

        [Header("Movement Parameters")]
        public float HitTime = 3f;
        public float Speed = 2f;
        public float PullDistance = 3f;
    }
}
