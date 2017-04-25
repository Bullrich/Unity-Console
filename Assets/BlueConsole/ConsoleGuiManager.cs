using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// by @Bullrich

namespace Blue.Console {
    public class ConsoleGuiManager {
        List<LogInfo> logsList = new List<LogInfo>();
        List<LogType> blockedLogs = new List<LogType>();
        ScrollRect scrllRect;
        Transform logContainer;
        LogDetails details;

        public ConsoleGuiManager(ScrollRect scrollRect, LogDetails logDetails) {
            scrllRect = scrollRect;
            details = logDetails;
            logContainer = scrllRect.transform.GetChild(0);
            Debug.Log(logContainer.name);
        }

        public void LogMessage(LogType type, string stackTrace, string logMessage, LogInfo newLog) {
            newLog.LoadLog(new LogInfo.ErrorDetail(logMessage, stackTrace, type, errorSprite(type)));
            newLog.transform.SetParent(logContainer, false);
            logsList.Add(newLog);
            if (blockedLogs.Contains(type))
                newLog.gameObject.SetActive(false);
            else if (!Input.GetMouseButton(0))
                scrllRect.velocity = new Vector2(scrllRect.velocity.x, 1000f);
        }

        public void ClearList() {
            logsList.Clear();
        }

        // TODO Fix this part that isn't working right
        public void FilterList(LogType filter) {
            bool isBlocked = !blockedLogs.Contains(filter);
            foreach (LogInfo log in logsList)
                if (log.GetFilterLogType() != filter)
                    log.gameObject.SetActive(!isBlocked);
            if (isBlocked)
                blockedLogs.Remove(filter);
            else
                blockedLogs.Add(filter);
        }

        Sprite errorSprite(LogType logType) {
            Sprite logSprite = null;
            switch (logType) {
                case LogType.Error:
                    logSprite = details.logErrorSprite;
                    break;
                case LogType.Assert:
                    logSprite = details.logErrorSprite;
                    break;
                case LogType.Warning:
                    logSprite = details.logWarningSprite;
                    break;
                case LogType.Log:
                    logSprite = details.logInfoSprite;
                    break;
                case LogType.Exception:
                    logSprite = details.logErrorSprite;
                    break;
                default:
                    logSprite = details.logInfoSprite;
                    break;
            }
            return logSprite;
        }
    }
}