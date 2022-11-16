using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Common
{
    [CreateAssetMenu(menuName = "StaticData/StaticGamePlayerData", fileName = "StaticGamePlayerData", order = 2)]
    public class StaticGamePlayerData : ScriptableObject
    {
        public Color HitColor;
        public Color InitialColor;

        public float HitTime = 3f;

        public float Speed = 2f;
        public float PullDistance = 3f;
    }
}
