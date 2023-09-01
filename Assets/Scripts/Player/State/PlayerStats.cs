using SharedBaseClasses;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/PlayerStats")]
    public class PlayerStats : CharacterStatsBase
    {
        [Header("Player Movement Specifics")]
        public float MinimumMovementSpeed;
        [Range(0, 0.2f)] public float MovementAcceleration = 0.1f;
        [Range(0, 0.2f)] public float MovementDeceleration = 0.007f;
        public bool UseGravity = true;
        public float Gravity = 9.81f;
        public int RotationSpeed = 10;
        
    }
}