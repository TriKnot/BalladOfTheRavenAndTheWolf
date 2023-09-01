using UnityEngine;

namespace Utils
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField] private ScriptableGameObjectPool[] _pools;
        
        private void Awake()
        {
            foreach (var pool in _pools)
            {
                pool.Init();
            }
        }
    }
}
