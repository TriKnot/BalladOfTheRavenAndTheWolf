
using UnityEngine;

namespace SharedBaseClasses
{
    [CreateAssetMenu(fileName = "Stats", menuName = "SharedBaseClasses/Stats", order = 0)]
    public class StatsBase : ScriptableObject
    {
        public int MaxHealth;
        [Range(0, 100), Tooltip("The percentage of health to starts with. Rounds down to the nearest integer.")]
        public int StartingHealthPercent = 50;

    }
}
