using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// by @Bullrich

namespace Blue.Console
{
    public class ConsoleActionButton : ActionButtonBehavior
    {
        // Action
        private System.Delegate buttonAction;

        public override void Init(ActionContainer action)
        {
            buttonAction = action.action;
            actionName = action.actionName;
            transform.GetChild(0).GetComponent<Text>().text = action.actionName;
        }
        public void ButtonAction()
        {
            buttonAction.DynamicInvoke();
        }

    }
}
