using UnityEngine;

namespace Enemy.Spawner
{
    [CreateAssetMenu(fileName = "EnemyWave", menuName = "Enemy/EnemyWave")]
    public class EnemyWave : ScriptableObject
    {
        public GameObject[] EnemyPrefabs;
    }
}
