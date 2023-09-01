using System.Collections;
using SharedBaseClasses;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Variables;

namespace Enemy.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private ScriptableGameObjectList _attackTargets;
        [FormerlySerializedAs("_heartHealth")] [SerializeField] private ScriptableHealthSystem _heartScriptableHealth;
        
        [SerializeField] private DifficultySection[] _sections;
        
        private DifficultySection _currentSection;

        private int _totalWaveNumber;

        [Header("Trigger Indicator UI")] [SerializeField]
        private GameObject _UIPrefab;

        private UIIndicator _UIIndicator;
        
        private void Start()
        {
            StartCoroutine(SpawnWave(0));
            GetUIComponents();
        }

        private IEnumerator SpawnWave(int waveNumber)
        {
            while (true)
            {
                _totalWaveNumber++;
                var section = GetCurrentSection();
                if(_currentSection == null || _currentSection != section)
                {
                    _currentSection = section;
                    waveNumber = 0;
                }                
                var waveManager = _currentSection.EnemyWaves[waveNumber];
                var wave = waveManager.EnemyWave;
                var wait = new WaitForSeconds(waveManager.SpawnInterval);

                waveManager.WaveNumber = waveNumber;
                waveManager.AttackTargets = _attackTargets;
                waveManager.WaveNumber = _totalWaveNumber;

                yield return new WaitForSeconds(waveManager.SpawnDelay);

                foreach (var enemyPrefab in wave.EnemyPrefabs)
                {
                    _UIIndicator.ShowSpawnIndicator(waveManager.Lane);
                    var enemyManager = Instantiate(enemyPrefab, waveManager.Lane.SpawnPoint, Quaternion.identity).GetComponent<EnemyManager>();
                    enemyManager.WaveManager = waveManager;
                    yield return wait;
                }

                waveNumber++;
                waveNumber %= _currentSection.EnemyWaves.Length;
            }
        }

        private DifficultySection GetCurrentSection()
        {
            foreach (var section in _sections)
            {
                if (_heartScriptableHealth.GetHealthPercent() <= section.MaxHealthPercentage)
                    return section;
            }
            return _sections[^1];
        }
        
        [System.Serializable]
        private class DifficultySection
        {
            [Range(0,1)]
            public float MaxHealthPercentage;
            public EnemyWaveManager[] EnemyWaves;
        }
        
        private void GetUIComponents()
        {
            _UIIndicator = _UIPrefab.GetComponent<UIIndicator>();
        }
    }
}
