using System;
using Blue.Console.Container;
using UnityEngine;
using UnityEngine.UI;

// by @Bullrich

namespace Blue.Console
{
    public class ConsoleVariableButton : ActionButtonBehavior
    {
        // Variable
        public Text variableText;

        private InputField _field;
        private Toggle _toggle;
        private Delegate _variableAction;
        private bool _isIntVariable;

        public override void Init(ActionContainer action)
        {
            SetVariableInputs();
            ActionName = action.actionName;
            SetAction(action);
        }

        private void SetVariableInputs()
        {
            foreach (Transform t in transform)
            {
                if (t.GetComponent<InputField>() != null)
                    _field = t.GetComponent<InputField>();
                else if (t.GetComponent<Toggle>() != null)
                    _toggle = t.GetComponent<Toggle>();
            }
        }

        private void SetAction(ActionContainer action)
        {
            variableText.text = action.actionName;
            _variableAction = action.action;
            switch (action.actType)
            {
                case ActionContainer.ActionType.Bool:
                    Destroy(_field.gameObject);
                    _toggle.isOn = action.boolStartStatus;
                    break;
                case ActionContainer.ActionType.Int:
                    Destroy(_toggle.gameObject);
                    _field.contentType = InputField.ContentType.IntegerNumber;
                    _isIntVariable = true;
                    _field.placeholder.GetComponent<Text>().text = action.intDefaultValue.ToString();
                    break;
                case ActionContainer.ActionType.Float:
                    Destroy(_toggle.gameObject);
                    _field.contentType = InputField.ContentType.DecimalNumber;
                    _isIntVariable = false;
                    _field.placeholder.GetComponent<Text>().text = action.floatDefaultValue.ToString();
                    break;
            }
        }

        public void BooleanAction(bool boolean)
        {
            _variableAction.DynamicInvoke(boolean);
        }

        public void OnInputEnd()
        {
            string value = _field.text;
            if (_isIntVariable)
                _variableAction.DynamicInvoke(int.Parse(value));
            else
                _variableAction.DynamicInvoke(float.Parse(value));
        }
    }
}