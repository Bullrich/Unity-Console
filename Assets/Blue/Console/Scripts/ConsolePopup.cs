using System;
using System.Collections.Generic;
using Blue.Console.Container;
using UnityEngine;
using UnityEngine.UI;

// By @Bullrich
namespace Blue.Console
{
    public class ConsolePopup : MonoBehaviour
    {
        [SerializeField] public Alert[] alertsPreview;

        public void UpdateLogs(LogInfo[] logs)
        {
            foreach (Alert alert in alertsPreview)
            {
                alert.logText.text = CountLogs(alert.logType, logs).ToString();
            }
        }

        public void CleanLogs()
        {
            foreach (Alert alert in alertsPreview)
            {
                alert.logText.text = "0";
            }
        }

        private static int CountLogs(LogType logType, IEnumerable<LogInfo> logs)
        {
            int countedLogs = 0;
            foreach (LogInfo log in logs)
            {
                if (log.Type == logType)
                    countedLogs++;
            }

            return countedLogs;
        }

        [Serializable]
        public struct Alert
        {
            public LogType logType;
            public Text logText;
        }
    }
}