using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// By @Bullrich
namespace Blue.Console
{
    [CustomEditor(typeof(LoadGameConsole))]
    public class LoadGameConsoleEditor : Editor
    {
        private LoadGameConsole _target;
        private SwipeManager _swipeManager;
        private bool _showSwipeOptions;

        private void OnEnable()
        {
            _target = (LoadGameConsole) target;
        }

        private bool _showDefault = false;

        public override void OnInspectorGUI()
        {
            ShowValues();
            Repaint();
        }

        private void ShowValues()
        {
            _target.gameConsole =
                (ConsoleContainer) EditorGUILayout.ObjectField("Console", _target.gameConsole, typeof(ConsoleContainer),
                    true);
            if (_target.gameConsole == null)
                EditorGUILayout.HelpBox("A Game Console Object must be added", MessageType.Error);
            else
            {
                EditorGUILayout.LabelField("Console Options", EditorStyles.centeredGreyMiniLabel);
                _target.startMinified =
                    EditorGUILayout.Toggle(
                        new GUIContent("Start console minified", "Show a minified version of the console on start"),
                        _target.startMinified);
                _target.limitOfLogs = EditorGUILayout.IntField(
                    new GUIContent("Limit of logs", "The maximium amount of logs the console can store"),
                    _target.limitOfLogs);
                SwipeOptions();
            }
        }

        private void SwipeOptions()
        {
            _swipeManager = _target.swipeOptions;
            _showSwipeOptions = EditorGUILayout.Foldout(_showSwipeOptions, "Swipe Options");
            if (!_showSwipeOptions) return;
            
            EditorGUILayout.BeginVertical("Box");
            _swipeManager.swipeDirection = (SwipeManager.swDirection) EditorGUILayout.EnumPopup("Swipe Direction",
                _swipeManager.swipeDirection);
            _swipeManager.fingersNeed = EditorGUILayout.IntSlider("Fingers need",_swipeManager.fingersNeed, 1, 4);
            _swipeManager.openConsoleKey = (KeyCode) EditorGUILayout.EnumPopup(
                new GUIContent("Open Console Key", "The key used to open the console"),
                _swipeManager.openConsoleKey);

            EditorGUILayout.EndVertical();
        }
    }
}