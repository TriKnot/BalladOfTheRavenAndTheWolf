using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.State
{
    public class PlayerSpawner : MonoBehaviour
    {
        public PlayerInputManager inputManager;
        public GameObject[] player;
        [SerializeField] private GameObject[] _spawnPoints;
        [SerializeField] private ObservablePlayerHolder _playerHolder;
        [SerializeField] private Camera _cam;
            
        private int _playerIndex;
        private int _maxPlayers;

        private void Awake()
        {
            _maxPlayers = inputManager.maxPlayerCount;
            inputManager.playerPrefab = player[_playerIndex];
        }

        private void OnEnable()
        {
            _playerHolder.OnPlayerManagerAdded += OnPlayerSpawn;
        }

        private void OnDisable()
        {
            _playerHolder.OnPlayerManagerAdded -= OnPlayerSpawn;
        }

        private void OnPlayerSpawn(PlayerManager manager)
        {
            manager.transform.position = _spawnPoints[_playerIndex].transform.position; //Set position
            
            _playerIndex++; // +1 for the index
            
            if (_playerIndex >= _maxPlayers) //Cheeck if we reached max players
            {
                _cam.gameObject.SetActive(false); //Disable third camera
                _playerIndex = 0; // Reset index if reached max amount of players
            }
            
            inputManager.playerPrefab = player[_playerIndex]; // Set prefab to new player object
        }
    }
}
