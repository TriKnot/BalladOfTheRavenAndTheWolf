using UnityEngine;

namespace Utils.Variables
{
    public class ScriptableVariableBase<T> : ScriptableObject
    {
        [SerializeField] private T _value;
        
        protected T CurrentValue;
        
        public T Value => CurrentValue;

        
        public virtual void ApplyChange(T change){}

        public virtual void SetValue(T newValue)
        {
            CurrentValue = newValue;
        }

        private void OnEnable()
        {
            CurrentValue = _value;
        }
    }
}
