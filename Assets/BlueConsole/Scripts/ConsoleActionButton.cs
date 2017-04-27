using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// by @Bullrich

namespace Blue.Console {
    public class ConsoleActionButton : ActionButtonBehavior {

        // Action
        System.Delegate buttonAction;
        InputField field;

        public override void Init(ConsoleActions.ActionContainer action) {
            buttonAction = action.action;
            transform.GetChild(0).GetComponent<Text>().text = action.actionName;
        }

        public void ButtonAction() {
            buttonAction.DynamicInvoke();
        }

    }
}
