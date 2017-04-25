using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mady by @Bullrich

namespace Blue.Console {
	public class ConsoleOutput : MonoBehaviour {
        public string output = "";
        public string stack = "";

        void OnEnable() {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable() {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type) {
            output = logString;
            stack = stackTrace;

            gui.LogMessage(type, stackTrace, logString);
        }

        ConsoleGUI gui;
        void Start(){
            gui = GetComponent<ConsoleGUI> ();
            InvokeRepeating ("test", 1f, 1f);
        }

        private void Update() {
            if (Input.GetKey(KeyCode.J))
                canShow = !canShow;
        }
        public bool canShow = false;
        void test(){
            if (canShow) {
                print("random: " + Random.Range(0, 12));
                Debug.LogAssertion("This is a assertion");
                Debug.LogWarning("WARNING");
            }   
        }
    }
}