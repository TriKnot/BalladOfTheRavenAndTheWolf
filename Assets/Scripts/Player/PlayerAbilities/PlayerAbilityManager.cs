using System.Collections;
using System.Collections.Generic;
using Player.State;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player.PlayerAbilities
{
    public class PlayerAbilityManager : MonoBehaviour
    {
        public Transform BodyTransform;
        public PlayerManager PlayerManager { get; private set; }

        [Header("Player Abilities")]
        [SerializeField] private ButtonAbility[] _buttonNorthAbility;
        [SerializeField] private ButtonAbility[] _buttonSouthAbility;
        [SerializeField] private ButtonAbility[] _buttonEastAbility;
        [SerializeField] private ButtonAbility[] _buttonWestAbility;
        [SerializeField] private ButtonAbility[] _buttonLeftBumperAbility;
        [SerializeField] private ButtonAbility[] _buttonRightBumperAbility;
        [SerializeField] private ButtonAbility[] _buttonLeftTriggerAbility;
        [SerializeField] private ButtonAbility[] _buttonRightTriggerAbility;
        [SerializeField] private ButtonAbility[] _buttonLeftStickAbility;
        [SerializeField] private ButtonAbility[] _buttonRightStickAbility;
        [SerializeField] private ButtonAbility[] _buttonStartAbility;
        [SerializeField] private ButtonAbility[] _buttonSelectAbility;
        
        private List<ButtonAbility[]> _playerActions;
        
        private void Awake()
        {
            // Get Components
            PlayerManager = GetComponent<PlayerManager>();
            
            _playerActions = new List<ButtonAbility[]>
            {
                _buttonNorthAbility,
                _buttonSouthAbility,
                _buttonEastAbility,
                _buttonWestAbility,
                _buttonLeftBumperAbility,
                _buttonRightBumperAbility,
                _buttonLeftTriggerAbility,
                _buttonRightTriggerAbility,
                _buttonLeftStickAbility,
                _buttonRightStickAbility,
                _buttonStartAbility,
                _buttonSelectAbility
            };
            
            for(int i = _playerActions.Count - 1; i >= 0; i--)
            {
                if(_playerActions[i] == null)
                    _playerActions.RemoveAt(i);
            }
            
        }
        
        private void OnEnable()
        {
            foreach (var playerAction in _playerActions)
            {
                foreach (var action in playerAction)
                {
                    action.Ability.InitAbility(this);
                }
            }
        }

        private void Update()
        {
            foreach (var playerAction in _playerActions)
            {
                foreach (var action in playerAction)
                {
                    action.Ability.MakeUpdate();
                }
            }
        }
        
        public void NorthAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonNorthAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void SouthAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonSouthAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void EastAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonEastAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void WestAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonWestAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void LeftBumperAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonLeftBumperAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void RightBumperAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonRightBumperAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void LeftTriggerAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonLeftTriggerAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void RightTriggerAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonRightTriggerAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void LeftStickAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonLeftStickAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void RightStickAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonRightStickAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void StartAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonStartAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public void SelectAction(InputAction.CallbackContext context)
        {
            foreach (var ability in _buttonSelectAbility)
            {
                if (ability.CanUseState == PlayerManager.State)
                {
                    ability.Ability.Act(context);
                }
            }
        }
        
        public Coroutine TriggerCoroutine(IEnumerator coroutine)
        {
            return coroutine == null ? null : StartCoroutine(coroutine);
        }
        
        public void CancelCoroutine(Coroutine coroutine)
        {
            if (coroutine == null) return;
            
            StopCoroutine(coroutine);
        }
        
    }
}
