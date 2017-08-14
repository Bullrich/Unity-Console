using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// by @Bullrich

namespace Blue.Console
{
    public class ConsoleGuiManager
    {
        private List<LogInfo>
            logsList = new List<LogInfo>(),
            pausedLogs = new List<LogInfo>();

        private List<LogType> blockedLogs = new List<LogType>();
        private List<ActionButtonBehavior> actionButtons = new List<ActionButtonBehavior>();
        private ScrollRect scrllRect;
        private readonly int logLimit;
        private Transform logContainer;
        private ConsoleGUI.LogDetails details;
        private bool listPaused;
        private ConsolePopup popup;

        public ConsoleGuiManager(ScrollRect scrollRect, ConsoleGUI.LogDetails logDetails, ConsolePopup _popup, int limit)
        {
            scrllRect = scrollRect;
            details = logDetails;
            logContainer = scrllRect.transform.GetChild(0);
            popup = _popup;
            logLimit = limit;
        }

        public void LogMessage(LogType type, string stackTrace, string logMessage, LogInfo newLog)
        {
            newLog.LoadLog(new LogInfo.ErrorDetail(logMessage, stackTrace, type, errorSprite(type)));
            newLog.transform.SetParent(logContainer, false);
            logsList.Add(newLog);
            if (listPaused)
            {
                pausedLogs.Add(newLog);
                newLog.gameObject.SetActive(false);
            }
            else if (blockedLogs.Contains(newLog.GetFilterLogType()))
                newLog.gameObject.SetActive(false);
            else if (!Input.GetMouseButton(0))
                scrllRect.velocity = new Vector2(scrllRect.velocity.x, 1000f);
            if(logsList.Count > logLimit)
                DeleteLog(logsList[0]);
            popup.UpdateLogs(getLogs());
        }

        private void DeleteLog(LogInfo log)
        {
            MonoBehaviour.Destroy(log.gameObject);
            logsList.Remove(log);
        }

        public LogInfo[] getLogs()
        {
            return logsList.ToArray();
        }

        public void AddAction(ActionButtonBehavior button)
        {
            button.GetComponent<RectTransform>().SetAsLastSibling();
            actionButtons.Add(button);
        }

        public void RemoveAction(string _actionName)
        {
            foreach (ActionButtonBehavior _actionButton in actionButtons)
            {
                if (_actionButton.GetActionName() == _actionName)
                {
                    MonoBehaviour.Destroy(_actionButton.gameObject, 0.1f);
                    actionButtons.Remove(_actionButton);
                    break;
                }
            }
        }

        public int LogsLength()
        {
            return logsList.Count;
        }

        public void ClearList()
        {
            logsList.Clear();
            popup.CleanLogs();
            GC.Collect();
        }

        public void PauseList()
        {
            listPaused = !listPaused;
            if (!listPaused)
            {
                foreach (LogInfo log in pausedLogs)
                {
                    if (!blockedLogs.Contains(log.GetFilterLogType()))
                        log.gameObject.SetActive(true);
                }
                pausedLogs.Clear();
            }
        }

        public void FilterList(LogType filter)
        {
            bool isBlocked = blockedLogs.Contains(filter);
            if (isBlocked)
                blockedLogs.Remove(filter);
            else
                blockedLogs.Add(filter);

            foreach (LogInfo log in logsList)
                if (log.GetFilterLogType() == filter)
                    if (!pausedLogs.Contains(log) && isBlocked)
                        log.gameObject.SetActive(isBlocked);
                    else
                        log.gameObject.SetActive(false);
        }

        public void FilterList(string _messageString)
        {
            bool shouldBeShown = false;
            if (_messageString == "" || _messageString == null)
                shouldBeShown = true;
            foreach (LogInfo log in logsList)
            {
                if (!log.logMessage.text.Contains(_messageString))
                    log.gameObject.SetActive(shouldBeShown);
            }
        }

        private Sprite errorSprite(LogType logType)
        {
            Sprite logSprite = details.logErrorSprite;
            if (logType == LogType.Log)
                logSprite = details.logInfoSprite;
            else if (logType == LogType.Warning)
                logSprite = details.logWarningSprite;
            return logSprite;
        }
    }
}