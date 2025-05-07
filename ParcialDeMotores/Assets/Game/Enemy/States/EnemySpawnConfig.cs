using UnityEngine;

namespace Game.Enemy.States
{
    [System.Serializable]
    public class EnemySpawnConfig : MonoBehaviour
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float spawnProbability;
    }
}