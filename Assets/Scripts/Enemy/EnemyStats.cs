using SharedBaseClasses;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy/EnemyStats")]
    public class EnemyStats : CharacterStatsBase
    {
        public float DetectionRadius;
        [Tooltip("The amount of blight the enemy will heal the player when killed")]
        public int BlightHealAmount;
        [Header("Navigation")]
        [Tooltip("The acceleration of the enemy in units per second squared")]
        public float Acceleration;
        [Tooltip("The turning speed of the enemy in degrees per second")]
        public float TurnSpeed;
        [Tooltip("The distance at which the enemy will stop from the target")]
        public float StoppingDistance;
        [Tooltip("The minimum distance at which the enemy will stop from the target, measured from the center of the enemy's collider")]
        public float AvoidanceRadius;
        [Tooltip("The height of the enemy's avoidance sphere")]
        public float AvoidanceHeight;
        [Tooltip("The priority of the enemy when pushing other enemies away from the target, lower values have higher priority")]
        public int PushPriority;
    }
}
