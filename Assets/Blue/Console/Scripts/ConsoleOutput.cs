using UnityEngine;

// Mady by @Bullrich

namespace Blue.Console
{
    public class ConsoleOutput : MonoBehaviour
    {
        ConsoleGUI gui;

        public void init(ConsoleGUI cGui)
        {
            gui = cGui;
            Application.logMessageReceived += HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            gui.LogMessage(type, stackTrace, logString);
        }
    }
}