using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mady by @Bullrich

namespace Blue.Console {
	public class ConsoleOutput : MonoBehaviour {
        public string output = "";
        public string stack = "";
        ConsoleGUI gui;

        /*void OnEnable() {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable() {
            Application.logMessageReceived -= HandleLog;
        }*/

        public void init(ConsoleGUI cGui){
            gui = cGui;
            Application.logMessageReceived += HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type) {
            output = logString;
            stack = stackTrace;
            gui.LogMessage(type, stackTrace, logString);
        }
    }
}