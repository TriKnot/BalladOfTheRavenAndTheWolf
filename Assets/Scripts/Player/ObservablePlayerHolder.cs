using Player.State;
using Player.State.Wolf;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "ObservablePlayerHolder", menuName = "Player/ObservablePlayerHolder", order = 0)]
    public class ObservablePlayerHolder : ScriptableObject
    {
       
        public PlayerManager RavenPlayerManager { get; private set; }
        public PlayerManager WolfPlayerManager { get; private set; }
        
        public delegate void PlayerManagerEvent(PlayerManager manager);
        public event PlayerManagerEvent OnPlayerManagerAdded;
        
        public delegate void PlayerManagerRemovedEvent(PlayerManager manager);
        public event PlayerManagerRemovedEvent OnPlayerManagerRemoved;

        public void AddManager(PlayerManager manager)
        {
            if (manager is WolfPlayerManager wolfPlayerManager)
            {
                WolfPlayerManager = wolfPlayerManager;
            }
            else 
            {
                RavenPlayerManager = manager;
            }
            OnPlayerManagerAdded?.Invoke(manager);
        }
        
        public void RemoveManager(PlayerManager manager)
        {
            if (manager is WolfPlayerManager wolfPlayerManager)
            {
                WolfPlayerManager = null;
            }
            else 
            {
                RavenPlayerManager = null;
            }
            OnPlayerManagerRemoved?.Invoke(manager);
        }
    }
}
