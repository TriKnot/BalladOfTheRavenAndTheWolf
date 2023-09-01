using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "new ScriptableObjectPool", menuName = "ScriptableObjectPool", order = 0)]
    public class ScriptableGameObjectPool : ScriptableObject
    {
        public GameObject Prefab;
        public int PoolSize;
        private Queue<GameObject> _pool;
        private Transform _parent;

        public delegate void OnPull();
        public event OnPull OnPullEvent;

        [SerializeField] private int LargestPoolSizeReached;

        public void Init()
        {
            _pool = new Queue<GameObject>();
            _parent = new GameObject(Prefab.name + " Pool").transform;
            for (int i = 0; i < PoolSize; i++)
            {
                CreateNewObject();
            }
        }
        
        private void CreateNewObject()
        {
            var obj = Instantiate(Prefab, _parent);
            obj.SetActive(false);
            _pool.Enqueue(obj);
            LargestPoolSizeReached = _pool.Count;
        }
        
        public GameObject GetPooledObject()
        {
            if (_pool == null)
            {
                Init();
                Debug.LogWarning("Pool was not initialized, initializing now");
                return null;
            }
            if(_pool.Count == 0)
                CreateNewObject();
            
            var obj = _pool.Dequeue();
            obj.SetActive(true);
            OnPullEvent?.Invoke();
            return obj;
        }
        
        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }

    }
}