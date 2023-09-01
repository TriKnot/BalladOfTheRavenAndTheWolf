using System;
using FMOD.Studio;
using SharedBaseClasses;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager Instance { get; set; }

        [SerializeField] private string _musicPath;
        [SerializeField] private string _musicParamName;
        [SerializeField] private string _ambiencePath;
        [SerializeField] private string _ambienceParamName;
        [SerializeField] private ScriptableHealthSystem _heartSystem;
        [SerializeField] private bool _setIgnoreNextSceneLoad;
        private static bool _ignoreNextSceneLoad;
     
        private static EventInstance _musicInstance;
        private static EventInstance _ambienceInstance;
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (_heartSystem == null) return;
            SceneManager.sceneLoaded += OnSceneLoaded;
            OnHealthChanged();
        }

        private void OnEnable()
        {
            if (_heartSystem == null) return;
            _heartSystem.OnHealthChanged += OnHealthChanged;
            OnHealthChanged();
        }

        private void Start()
        {
            _ignoreNextSceneLoad = _setIgnoreNextSceneLoad;
            if (_heartSystem == null) return;
            OnHealthChanged();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            
            if (_heartSystem != null) 
            {
                _heartSystem.OnHealthChanged -= OnHealthChanged;
            }

            if (_ignoreNextSceneLoad) return;
            
            if(!string.IsNullOrWhiteSpace(_musicPath))
                _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            
            if(!string.IsNullOrWhiteSpace(_ambiencePath))
                _ambienceInstance.stop(STOP_MODE.ALLOWFADEOUT);
            
            
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (_ignoreNextSceneLoad) return;

            if(!string.IsNullOrWhiteSpace(_musicPath))
            {
                if (_musicInstance.isValid())
                    _musicInstance.stop(STOP_MODE.IMMEDIATE);
                _musicInstance = FMODUnity.RuntimeManager.CreateInstance(_musicPath);
                _musicInstance.start();
            }

            if(!string.IsNullOrWhiteSpace(_ambiencePath))
            {
                if (_ambienceInstance.isValid())
                    _ambienceInstance.stop(STOP_MODE.IMMEDIATE);
                _ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(_ambiencePath);
                _ambienceInstance.start();
            }
        }
    
        private void OnHealthChanged()
        {
            if(!string.IsNullOrWhiteSpace(_ambiencePath))
                _ambienceInstance.setParameterByName(_ambienceParamName, _heartSystem.GetHealthPercent());
            if(!string.IsNullOrWhiteSpace(_musicPath))
                _musicInstance.setParameterByName(_musicParamName, _heartSystem.GetHealthPercent());
        }
    }
}
