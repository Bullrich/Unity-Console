using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mady by @Bullrich

namespace Blue.Console
{
    public class ConsoleActions
    {
        public delegate void booleanAction (bool boolInput);

        public delegate void voidAction ();

        public delegate void intAction (int intInput);

        public delegate void floatAction (float floatInput);

        static List<ActionContainer> actions {
            get {
                if (actions == null)
                    actions = new List<ActionContainer> ();
                return actions;
            }
            set {
                actions = value;
            }
        }

        private static void AddActionToList(ActionContainer container)
        {
            if (!actions.Contains (container))
                actions.Add (container);
            else
                Debug.LogWarning (string.Format ("Action {0} has already being added!", container.actionName));
        }

        public static void AddAction(booleanAction action, string actionName)
        {
            AddActionToList (new ActionContainer (ActionContainer.ActionType._bool, action, actionName));
        }

        public static void AddAction(voidAction action, string actionName)
        {
            AddActionToList (new ActionContainer (ActionContainer.ActionType._void, action, actionName));
        }

        public static void AddAction(floatAction action, string actionName)
        {
            AddActionToList (new ActionContainer (ActionContainer.ActionType._float, action, actionName));
        }

        public static void AddAction(intAction action, string actionName)
        {
            AddActionToList (new ActionContainer (ActionContainer.ActionType._int, action, actionName));
        }

        public class ActionContainer
        {
            public enum ActionType
            {
                _void,
                _bool,
                _int,
                _float
            }

            public ActionType actType;
            public System.Delegate action;
            public string actionName;

            public ActionContainer (ActionType type, System.Delegate delegateAction, string _actionName)
            {
                actType = type;
                action = delegateAction;
                actionName = _actionName;
                /*
                try{
                action.DynamicInvoke (123);
                }
                catch(System.Exception e){
                    Debug.Log(e);
                }
                //*/
            }
        }
    }
}