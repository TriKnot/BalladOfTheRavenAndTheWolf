using UnityEngine;

namespace SharedBaseClasses
{
    [CreateAssetMenu(fileName = "HealthSystem", menuName = "SharedBaseClasses/HealthSystem")]
    public class ScriptableHealthSystem : ScriptableObject
    {
        [SerializeField] protected StatsBase _stats;
        [SerializeField] protected int _currentHealth;
        [SerializeField] private string _healSoundPath;
        [SerializeField] private string _damageSoundPath;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth { get; private set; }


        public delegate void HealthChanged();
        public event HealthChanged OnHealthChanged;

        public delegate void OnWin();
        public event OnWin OnWinEvent;
        
        public delegate void OnLose();
        public event OnLose OnLoseEvent;
        
        private void OnEnable()
        {
            ResetHealth();
        }
        
        public void ResetHealth()
        {
            MaxHealth = _stats.MaxHealth;
            _currentHealth = MaxHealth * _stats.StartingHealthPercent / 100;
            OnHealthChanged?.Invoke();
        }

        public void Damage(int damage)
        {
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            OnHealthChanged?.Invoke();
            if(!string.IsNullOrWhiteSpace(_damageSoundPath))
                FMODUnity.RuntimeManager.PlayOneShot(_damageSoundPath);

            if (GetHealthPercent() >= 1)
            {
                OnWinEvent?.Invoke();
            }
            else if (GetHealthPercent() <= 0)
            {
                OnLoseEvent?.Invoke();
            }
            
        }

        public void Heal(int value)
        {
            _currentHealth = Mathf.Min(_currentHealth + value, MaxHealth);
            OnHealthChanged?.Invoke();
            //if(_healSoundPath != "")
                //FMODUnity.RuntimeManager.PlayOneShot(_healSoundPath);
        }

        public float GetHealthPercent()
        {
            return (float)_currentHealth / MaxHealth;
        }
        
    }
}
