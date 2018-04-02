using System;
using System.Collections.Generic;
using Blue.Console.Container;
using Blue.Console.Handler;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// by @Bullrich

namespace Blue.Console
{
    public class ConsoleManager
    {
        private readonly List<LogInfo> _pausedLogs = new List<LogInfo>();

        private readonly List<LogType> _blockedLogs = new List<LogType>();
        private readonly ScrollRect _scrllRect;
        private readonly int _logLimit;
        private readonly Transform _logContainer;
        private readonly ConsoleGUI.LogDetails _details;
        private bool _listPaused;
        private readonly ConsolePopup _popup;
        private readonly LogBuffer _logBuffer;

        private readonly ActionsHandler _actions = new ActionsHandler();

        public ConsoleManager(ScrollRect scrollRect, ConsoleGUI.LogDetails logDetails, ConsolePopup _popup, int limit)
        {
            _scrllRect = scrollRect;
            _details = logDetails;
            _logContainer = _scrllRect.transform.GetChild(0);
            this._popup = _popup;
            _logLimit = limit;
            _logBuffer= new LogBuffer(limit);
        }

        public void LogMessage(LogType type, string stackTrace, string logMessage, LogInfo newLog)
        {
            newLog.LoadLog(logMessage, stackTrace, type, ErrorSprite(type));
            newLog.transform.SetParent(_logContainer, false);
            _logBuffer.Push(newLog);
            if (_listPaused)
            {
                _pausedLogs.Add(newLog);
                newLog.gameObject.SetActive(false);
            }
            else if (_blockedLogs.Contains(newLog.GetFilterLogType()))
                newLog.gameObject.SetActive(false);
            else if (!Input.GetMouseButton(0))
                _scrllRect.velocity = new Vector2(_scrllRect.velocity.x, 1000f);

            _popup.UpdateLogs(_logBuffer.GetLogs());
        }

        public LogInfo[] Logs
        {
            get { return _logBuffer.GetLogs(); }
        }

        public void AddAction(ActionButtonBehavior button)
        {
            button.GetComponent<RectTransform>().SetAsLastSibling();
            _actions.Add(button);
        }

        public void RemoveAction(string actionName)
        {
            _actions.Remove(actionName);
        }

        public int LogsLength()
        {
            return _logBuffer.Count;
        }

        public void ClearList()
        {
            _logBuffer.Clear();
            _popup.CleanLogs();
            // Added to delete all the garbage from the multiple destroys
            GC.Collect();
        }

        public void PauseList()
        {
            _listPaused = !_listPaused;
            if (!_listPaused)
            {
                foreach (LogInfo log in _pausedLogs)
                {
                    if (!_blockedLogs.Contains(log.GetFilterLogType()))
                        log.gameObject.SetActive(true);
                }

                _pausedLogs.Clear();
            }
        }

        public void FilterList(LogType filter)
        {
            bool isBlocked = _blockedLogs.Contains(filter);
            if (isBlocked)
                _blockedLogs.Remove(filter);
            else
                _blockedLogs.Add(filter);

            foreach (LogInfo log in _logBuffer.GetLogs())
                if (log.GetFilterLogType() == filter)
                    if (!_pausedLogs.Contains(log) && isBlocked)
                        log.gameObject.SetActive(true);
                    else
                        log.gameObject.SetActive(false);
        }

        public void FilterList([NotNull] string messageString)
        {
            bool shouldBeShown = string.IsNullOrEmpty(messageString);
            foreach (LogInfo log in _logBuffer.GetLogs())
            {
                if (!log.logMessage.text.Contains(messageString))
                    log.gameObject.SetActive(shouldBeShown);
            }
        }

        private Sprite ErrorSprite(LogType logType)
        {
            Sprite logSprite = _details.logErrorSprite;
            if (logType == LogType.Log)
                logSprite = _details.logInfoSprite;
            else if (logType == LogType.Warning)
                logSprite = _details.logWarningSprite;
            return logSprite;
        }
    }
}