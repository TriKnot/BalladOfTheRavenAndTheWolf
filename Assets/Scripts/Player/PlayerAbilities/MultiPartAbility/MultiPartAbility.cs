using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerAbilities.MultiPartAbility
{
    [CreateAssetMenu(fileName = "MultiPartAbility", menuName = "Player/MultiPartAbility", order = 0)]
    public class MultiPartAbility : PlayerAbilityBase
    {
        [SerializeField] private PlayerAbilityBase[] _abilities;
        [SerializeField, Tooltip("Time before ability is reset to first, starts counting when ability cooldown is over.")] 
        private float _resetAbilityTime = 1f;
        private float _resetAbilityTimer;
        
        [SerializeField, Tooltip("Time before each attack where input is buffered and still counts as attack input.")] 
        private float _inputBufferTime = 1f;
        private float _inputBufferTimer;
        InputAction.CallbackContext _bufferedContext;
        private bool _shouldAttack;
        
        private int currentAbilityIndex;
        
        
        public override void InitAbility(PlayerAbilityManager abilityManager)
        {
            manager = abilityManager;
            playerStats = abilityManager.PlayerManager.Stats;
            foreach (var ability in _abilities)
            {
                ability.InitAbility(abilityManager);
            }
        }

        public override void MakeUpdate()
        {
            foreach (var ability in _abilities)
            {
                ability.MakeUpdate();
            }
            CountDownResetTimer();
            CountDownInputBufferTimer();
            
            if(manager.PlayerManager.State == PlayerState.Grounded && _shouldAttack && !IsOnCooldown())
                Attack(_bufferedContext);
        }

        private void CountDownInputBufferTimer()
        {
            if(_inputBufferTimer <= 0)
            {
                _shouldAttack = false;
                return;
            }
            _inputBufferTimer = Mathf.Max(_inputBufferTimer - Time.deltaTime, 0);
        }

        private void CountDownResetTimer()
        {   
            if(_resetAbilityTimer <= 0) return;
            if(IsOnCooldown()) return;
            _resetAbilityTimer = Mathf.Max(_resetAbilityTimer - Time.deltaTime, 0);
            if (_resetAbilityTimer > 0) return;
            currentAbilityIndex = 0;
        }

        public override void Act(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            _shouldAttack = true;
            _inputBufferTimer = _inputBufferTime;
            _bufferedContext = context;
        }

        private void Attack(InputAction.CallbackContext context)
        {
            _abilities[currentAbilityIndex].Act(context);
            currentAbilityIndex++;
            currentAbilityIndex %= _abilities.Length;
            _resetAbilityTimer = _resetAbilityTime;
            _shouldAttack = false;
        }
        
        public override bool IsOnCooldown()
        {
            return _abilities.Any(ability => ability.IsOnCooldown());
        }
    }
}
