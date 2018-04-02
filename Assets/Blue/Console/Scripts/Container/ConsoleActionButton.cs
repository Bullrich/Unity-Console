using System;
using Blue.Console.Container;
using UnityEngine.UI;

// by @Bullrich

namespace Blue.Console
{
    public class ConsoleActionButton : ActionButtonBehavior
    {
        // Action
        private Delegate _buttonAction;

        public override void Init(ActionContainer action)
        {
            _buttonAction = action.action;
            ActionName = action.actionName;
            transform.GetChild(0).GetComponent<Text>().text = action.actionName;
        }
        public void ButtonAction()
        {
            _buttonAction.DynamicInvoke();
        }

    }
}
