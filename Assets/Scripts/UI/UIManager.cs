using System;
using System.Collections;
using System.Linq;
using Player;
using Player.PlayerAbilities.Raven;
using Player.State;
using SharedBaseClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private ScriptableGameObjectPool _pool;
        [SerializeField] private int _waitForSeconds = 3;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private Button _firstHighlightedButton;
        [SerializeField] private Button _firstHighlightedButtonWin;
        [SerializeField] private ScriptableHealthSystem _heartScriptableHealthSystem;
        [SerializeField] private ObservablePlayerHolder _holder;
        [SerializeField] private GameObject _joinCenter;
        [SerializeField] private GameObject _joinRight;
        [SerializeField] private GameObject _separator;
        [SerializeField] private GameObject _weakenFoe;

        private PlayerInput _playerInput;
        private bool _inMenu;


        private void Awake()
        {
            _pool.OnPullEvent += OnFirstPull;
            
            _joinCenter.SetActive(true);
            _joinRight.SetActive(false);
        }

        private void OnEnable()
        {
            _heartScriptableHealthSystem.OnHealthChanged += OnHeartScriptableHealthChanged;
            _gameOverScreen.SetActive(false);
            _winScreen.SetActive(false);
            _inMenu = false;

            _holder.OnPlayerManagerAdded += PressToJoinController;
        }
        
        private void OnDisable()
        {
            _heartScriptableHealthSystem.OnHealthChanged -= OnHeartScriptableHealthChanged;
            _holder.OnPlayerManagerAdded -= PressToJoinController;
        }

        private void OnHeartScriptableHealthChanged()
        {
            if (_heartScriptableHealthSystem.CurrentHealth <= 0)
            {
                StartCoroutine(ActivateGameOverScreen());
            }
            if(_heartScriptableHealthSystem.GetHealthPercent() >= 1f)
            {
                StartCoroutine(ActivateWinScreen());
            }
        }
        
        private IEnumerator ActivateGameOverScreen()
        {
            _separator.SetActive(false);
            yield return new WaitForSeconds(_waitForSeconds);
            _gameOverScreen.SetActive(true);
            _firstHighlightedButton.Select();
            _inMenu = true;
        }
        private IEnumerator ActivateWinScreen()
        {
            _separator.SetActive(false);
            yield return new WaitForSeconds(_waitForSeconds);
            _winScreen.SetActive(true);
            _firstHighlightedButtonWin.Select();
            _inMenu = true;
        }

        private void UnlockCursor()
        {
            if (_inMenu && Input.GetAxis("Mouse X") > 0 || _inMenu && Input.GetAxis("Mouse Y") > 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void Update()
        {
            UnlockCursor();
        }

        private void PressToJoinController(PlayerManager playerManager)
        {
            if (_holder.WolfPlayerManager != null)
            {
                _separator.SetActive(true);
                _joinCenter.SetActive(false);
                _joinRight.SetActive(true);
            }
            if (_holder.RavenPlayerManager != null)
            {
                _joinCenter.SetActive(false);
                _joinRight.SetActive(false);
                _weakenFoe.SetActive(true);
            }
        }

        private void OnFirstPull()
        {
            _weakenFoe.SetActive(false);
            
            _pool.OnPullEvent -= OnFirstPull;
        }
    }
}
