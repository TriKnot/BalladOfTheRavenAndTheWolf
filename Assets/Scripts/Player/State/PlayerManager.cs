using System;
using Player.PlayerAbilities;
using UnityEngine;

namespace Player.State
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerState State { get; private set; } = PlayerState.Grounded;
        [SerializeField] private PlayerStats _stats;
        [SerializeField] private ObservablePlayerHolder _observablePlayerHolder;
        [SerializeField] private GameObject _playerModelObject;
        public GameObject PlayerModelObject => _playerModelObject;
        [SerializeField] private Camera _camera;
        public Camera Camera => _camera;
        
        public Animator Animator { get; private set; }
        
        public PlayerStats Stats => _stats;
        
        [SerializeField] private string _groundedBoolName;
        [SerializeField] private string _aerialBoolName;
        [SerializeField] private string _lockedBoolName;
        private int _groundedBoolHash => Animator.StringToHash(_groundedBoolName);
        private int _aerialBoolHash => Animator.StringToHash(_aerialBoolName);
        private int _lockedBoolHash => Animator.StringToHash(_lockedBoolName);

        public void SetPlayerState(PlayerState state)
        {
            State = state;
            Animator.SetBool(_aerialBoolHash, false);
            Animator.SetBool(_lockedBoolHash, false);
            Animator.SetBool(_groundedBoolHash, false);
            switch (state)
            {
                case PlayerState.Grounded:
                    Animator.SetBool(_groundedBoolHash, true);
                    break;
                case PlayerState.Aerial:
                    Animator.SetBool(_aerialBoolHash, true);
                    break;
                case PlayerState.Locked:
                    Animator.SetBool(_lockedBoolHash, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            _observablePlayerHolder.AddManager(this);
            Init();
        }
        
        private void OnDisable()
        {
            _observablePlayerHolder.RemoveManager(null);
        }
        
        protected virtual void Init()
        {
        }
    }
}
