using Blue.Console.Container;
using UnityEngine;
using UnityEngine.UI;

// Mady by @Bullrich

namespace Blue.Console.Container
{
    public class LogInfo : MonoBehaviour, ILogInfo
    {
        public Text logMessage;
        public Image logType;
        [HideInInspector] public ConsoleGUI Gui;
        
        public string Log { get; private set; }
        public string Stack { get; private set; }
        public LogType Type { get; private set; }

        public void LoadLog(string log, string stack, LogType type, Sprite logSprite)
        {
            Log = log;
            Stack = stack;
            Type = type;
            PopulateLog(logSprite, log);
            
            logType.color = ColorOfLog(type);
        }

        private static Color ColorOfLog(LogType type)
        {
            Color logColor = Color.white;
            if (type == LogType.Assert || type == LogType.Error || type == LogType.Exception)
                logColor = Color.red;
            else if (type == LogType.Warning)
                logColor = Color.yellow;
            return logColor;
        }

        private void PopulateLog(Sprite type, string message)
        {
            logType.sprite = type;
            logMessage.text = message;
        }

        public void EnterDetailMode()
        {
            Gui.ShowDetail(this);
        }

        public LogType GetFilterLogType()
        {
            if (Type == LogType.Warning)
                return LogType.Warning;
            else if (Type == LogType.Log)
                return LogType.Log;
            else
                return LogType.Error;
        }
    }
}