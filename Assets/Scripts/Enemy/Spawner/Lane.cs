using Level.Heart;
using UnityEngine;

namespace Enemy.Spawner
{
    [CreateAssetMenu(fileName = "Lane", menuName = "Level/Lane", order = 0)]
    public class Lane : ScriptableObject
    {
        [HideInInspector] public Transform[] Waypoints;
        [HideInInspector] public Heart Heart;
        [HideInInspector] public Transform SpawnPointTransform { private get; set; }
        [HideInInspector] public Vector3 SpawnPoint => SpawnPointTransform.position;
    }
}
