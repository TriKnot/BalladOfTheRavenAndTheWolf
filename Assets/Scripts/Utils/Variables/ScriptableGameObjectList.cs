using System.Collections.Generic;
using UnityEngine;

namespace Utils.Variables
{
    [CreateAssetMenu(fileName = "new ListVariable", menuName = "Variables/ListVariable")]
    public class ScriptableGameObjectList : ScriptableVariableBase<List<GameObject>>
    {
        
        public void AddRange(List<GameObject> change)
        {
            Value.AddRange(change);
        }
        
        public void Add(GameObject item)
        {
            Value.Add(item);
        }
        
        public void Remove(GameObject item)
        {
            Value.Remove(item);
        }
        
        public void Clear()
        {
            Value.Clear();
        }
        
        public bool Contains(GameObject item)
        {
            return Value.Contains(item);
        }
        
        public int Count => Value.Count;
        
        public GameObject GetClosestObject(Vector3 position)
        {
            if(Value.Count == 0)
                return null;
            
            GameObject closest = null;
            var closestDistance = Mathf.Infinity;
            foreach (var item in Value)
            {
                var distance = (item.transform.position - position).sqrMagnitude;
                if (distance > closestDistance) continue;
                closest = item;
                closestDistance = distance;
            }

            return closest;
        }
        
        public GameObject[] GetObjectsInRange(Vector3 position, float range)
        {
            var objectsInRange = new List<GameObject>();
            foreach (var item in Value)
            {
                var distance = (item.transform.position - position).sqrMagnitude;
                if (distance > range) continue;
                objectsInRange.Add(item);
            }

            return objectsInRange.ToArray();
        }
        
    }
}