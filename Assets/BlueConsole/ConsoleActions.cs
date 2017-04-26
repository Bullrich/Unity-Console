using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mady by @Bullrich

namespace Blue.Console {
    public class ConsoleActions {
        public delegate void booleanAction(bool boolInput);

        public delegate void voidAction();

        public delegate void intAction(int intInput);

        public delegate void floatAction(float floatInput);

        private static List<ActionContainer> actions;

        public static List<ActionContainer> getActions() {
            if (actions == null)
                actions = new List<ActionContainer>();
            return actions;
        }

        private static void AddActionToList(ActionContainer container) {
            if (!getActions().Contains(container))
                getActions().Add(container);
            else
                Debug.LogWarning(string.Format("Action {0} has already being added!", container.actionName));
        }

        public static void AddAction(booleanAction action, string actionName, bool defaultBooleanState) {
            ActionContainer acon = new ActionContainer(ActionContainer.ActionType._bool, action, actionName);
            acon.boolStartStatus = defaultBooleanState;
            AddActionToList(acon);
        }

        public static void AddAction(voidAction action, string actionName) {
            AddActionToList(new ActionContainer(ActionContainer.ActionType._void, action, actionName));
        }

        public static void AddAction(floatAction action, string actionName, float defaultFloatValue) {
            ActionContainer acon = new ActionContainer(ActionContainer.ActionType._float, action, actionName);
            acon.floatDefaultValue = defaultFloatValue;
            AddActionToList(acon);
        }

        public static void AddAction(intAction action, string actionName, int defaultIntValue) {
            ActionContainer acon = new ActionContainer(ActionContainer.ActionType._int, action, actionName);
            acon.intDefaultValue = defaultIntValue;
            AddActionToList(acon);
        }

        public class ActionContainer {
            public enum ActionType {
                _void,
                _bool,
                _int,
                _float
            }

            public ActionType actType;
            public System.Delegate action;
            public string actionName;

            public bool boolStartStatus;
            public int intDefaultValue;
            public float floatDefaultValue;

            public ActionContainer(ActionType type, System.Delegate delegateAction, string _actionName) {
                actType = type;
                action = delegateAction;
                actionName = _actionName;
            }
        }
    }

    public abstract class ActionButtonBehavior : MonoBehaviour {
        public abstract void Init(ConsoleActions.ActionContainer action);
    }
}