using Enemy.Spawner;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level.Lanes
{
    public class LaneManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private Heart.Heart _heart;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Lane  _lane;
        
        private void Awake()
        {
            _lane.Waypoints = _waypoints;
            _lane.SpawnPointTransform = _spawnPoint;
            _lane.Heart = _heart;
        }
    }
}
