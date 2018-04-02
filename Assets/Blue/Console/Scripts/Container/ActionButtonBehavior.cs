using UnityEngine;

namespace Blue.Console.Container
{
    
    // By @Bullrich
    
    public abstract class ActionButtonBehavior : MonoBehaviour
    {
        public string ActionName { get; protected set; }
        public abstract void Init(ActionContainer action);
    }
}