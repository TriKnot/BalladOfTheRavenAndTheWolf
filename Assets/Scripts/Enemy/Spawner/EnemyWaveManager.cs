using UnityEngine;
using Utils.Variables;

namespace Enemy.Spawner
{
    [System.Serializable]
    public class EnemyWaveManager
    {
        [HideInInspector] public int WaveNumber;
        [HideInInspector] public ScriptableGameObjectList AttackTargets;

        [SerializeField] private EnemyWave _enemyWave;
        public EnemyWave EnemyWave => _enemyWave;
        
        [SerializeField] private float _spawnDelay = 10f;
        public float SpawnDelay => _spawnDelay;
        
        [SerializeField] private float _spawnInterval = 1f;
        public float SpawnInterval => _spawnInterval;
        
        [SerializeField] private Lane _lane;
        public Lane Lane => _lane;
        
        
    }
}
