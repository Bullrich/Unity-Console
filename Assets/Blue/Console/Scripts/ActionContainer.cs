using System;

// Mady by @Bullrich

namespace Blue.Console
{
    public class ActionContainer
    {
        public enum ActionType
        {
            Void,
            Bool,
            Int,
            Float
        }

        public readonly ActionType actType;
        public readonly Delegate action;
        public readonly string actionName;

        public bool boolStartStatus;
        public int intDefaultValue;
        public float floatDefaultValue;

        public ActionContainer(ActionType type, Delegate delegateAction, string actionName)
        {
            actType = type;
            action = delegateAction;
            this.actionName = actionName;
        }
    }
}