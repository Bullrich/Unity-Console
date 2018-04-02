using System.Collections.Generic;
using System.Linq;
using Blue.Console.Container;
using UnityEngine;

namespace Blue.Console.Handler
{
    public class ActionsHandler
    {
        private readonly Dictionary<string, ActionButtonBehavior> _actions;

        public ActionsHandler()
        {
            _actions = new Dictionary<string, ActionButtonBehavior>();
        }

        public void Add(ActionButtonBehavior action)
        {
            if (!ContainsKey(action))
                _actions.Add(action.ActionName, action);
            else
                Debug.LogWarning(string.Format("Action named \"{0}\" already added", action.ActionName));
        }

        public void Remove(ActionButtonBehavior action)
        {
            Remove(action.ActionName);
        }

        public void Remove(string actionName)
        {
            if (!_actions.ContainsKey(actionName)) return;
            
            ActionButtonBehavior action = _actions[actionName];
            Object.Destroy(action);
            _actions.Remove(actionName);
        }

        public ActionButtonBehavior[] GetActions()
        {
            return _actions.Values.ToArray();
        }

        private bool ContainsKey(ActionButtonBehavior action)
        {
            foreach (KeyValuePair<string, ActionButtonBehavior> act in _actions)
            {
                if (action == act.Value || action.ActionName == act.Key) return true;
            }

            return false;
        }
    }
}