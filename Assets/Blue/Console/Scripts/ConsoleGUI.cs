using System;
using System.Collections.Generic;
using Blue.Console.Container;
using Blue.Console.Handler;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable InconsistentNaming

// Mady by @Bullrich

namespace Blue.Console
{
    [RequireComponent(typeof(ConsoleOutput))]
    public class ConsoleGUI : MonoBehaviour
    {
        [Tooltip("Prefab object")] public LogInfo logInfo;
        public Transform logScroll, popUpDetail;
        private Transform logSection, actionSection;
        private Text detailInformation; 
        public ConsolePopup popup;
        public ActionButtons actionButtons;
        public Image backgroundImage;

        public LogDetails logDetail;
        private ConsoleManager _manager;
        private List<ActionContainer> actionsList;
        private SwipeManager openConsoleSettings;
        private bool _minifiedConsole;

        private ShareHandler _share;

        private string mailSubject;

        public void LogMessage(LogType type, string stackTrace, string message)
        {
            LogInfo info = Instantiate(logInfo);
            info.Gui = this;
            _manager.LogMessage(type,
                stackTrace, message, info);
        }

        public void init(SwipeManager swipe, bool minifyOnStart, int logLimit, string defaultMail)
        {
            openConsoleSettings = swipe;
            _manager = new ConsoleManager(
                logScroll.transform.parent.GetComponent<ScrollRect>(), logDetail, popup, logLimit);
            detailInformation = popUpDetail.GetChild(0).GetChild(0).GetComponent<Text>();
            CleanConsole();
            logSection = logScroll.transform.parent;
            actionSection = actionButtons.actionsContainer.transform.parent.parent;
            GameConsole.listUpdated += AddListElement;
            GameConsole.consoleMessage += WriteToConsole;
            GetComponent<ConsoleOutput>().init(this);
            _minifiedConsole = minifyOnStart;
            _share = new ShareHandler(defaultMail);

            if (!Debug.isDebugBuild)
                Debug.LogWarning("This isn't a development build! You won't be able to read the stack trace!");
        }

        private void AddActionElement(ActionContainer acon)
        {
            ActionButtonBehavior actionButton = actionButtons.actionBtnPrefab;
            Transform parent = actionButtons.actionsContainer.transform;
            if (acon.actType != ActionContainer.ActionType.Void)
            {
                actionButton = actionButtons.variablesBtnPrefab;
                parent = actionButtons.variablesContainer.transform;
            }
            ActionButtonBehavior spawnedBtn = Instantiate(actionButton);
            spawnedBtn.transform.SetParent(parent, false);
            spawnedBtn.Init(acon);
            _manager.AddAction(spawnedBtn);
        }

        private void RemoveActionElement(string _elementName)
        {
            _manager.RemoveAction(_elementName);
        }

        private void Update()
        {
            if (openConsoleSettings.didSwipe())
                SwitchConsole();
        }

        public void SwitchConsole()
        {
            backgroundImage.enabled = !backgroundImage.enabled;
            // Game console container object
            GameObject child = backgroundImage.gameObject.transform.GetChild(0).gameObject;
            child.SetActive(!child.activeSelf);
            if (_minifiedConsole)
                popup.gameObject.SetActive(!backgroundImage.enabled);
        }

        private void WriteToConsole(string title, string message)
        {
            LogMessage(LogType.Log, message, title);
        }

        // TODO: Move the logic to anoither script and document it
        private void AddListElement(List<ActionContainer> actions)
        {
            if (actionsList == null)
            {
                actionsList = new List<ActionContainer>(actions);
                foreach (ActionContainer acon in actionsList)
                    AddActionElement(acon);
            }
            else
            {
                int newElements = actions.Count - actionsList.Count;
                if (newElements > 0)
                    for (int i = actionsList.Count; i < actions.Count; i++)
                    {
                        AddActionElement(actions[i]);
                    }
                else if (newElements < 0)
                {
                    int ii = 0;
                    foreach (ActionContainer action in actionsList)
                    {
                        if (action.actionName != actions[ii].actionName)
                        {
                            RemoveActionElement(action.actionName);
                            break;
                        }
                        else if (ii + 1 > actions.Count - 1)
                        {
                            print(action.actionName + " is the last one!");
                            RemoveActionElement(action.actionName);
                        }
                        ii++;
                    }
                }
            }
            actionsList = new List<ActionContainer>(actions);
        }

        #region ButtonFunctions

        public void ToggleActions()
        {
            actionSection.gameObject.SetActive(!actionSection.gameObject.activeSelf);
            logSection.gameObject.SetActive(!logSection.gameObject.activeSelf);
        }

        public void CleanConsole()
        {
            foreach (Transform t in logScroll)
                Destroy(t.gameObject);
            _manager.ClearList();
        }

        public void ShowDetail(ILogInfo detail)
        {
            detailInformation.text = LimitLength(detail.Log + "\n\n" + detail.Stack, 3000);
            mailSubject = string.Format("[{0}] {1}", detail.Type.ToString(), detail.Log);
            popUpDetail.gameObject.SetActive(true);
        }

        private string LimitLength(string s, int l)
        {
            return s.Substring(0, s.Length > l ? l : s.Length);
        }

        public void ClosePopUp()
        {
            popUpDetail.gameObject.SetActive(false);
        }

        public void MinifiedConsole(bool minifiedStatus)
        {
            _minifiedConsole = minifiedStatus;
        }

        public void Share()
        {
            string textToSend = detailInformation.text;
            _share.Share(mailSubject, textToSend);
        }

        // TODO: Implement
        public void FilterLogs(FilterAction alertButton)
        {
            _manager.FilterList(alertButton.logType);
            Image buttonSprite = alertButton.transform.GetChild(0).GetComponent<Image>();
            Color defaultColor = Color.white;
            if (alertButton.logType == LogType.Error)
                defaultColor = Color.red;
            else if (alertButton.logType == LogType.Warning)
                defaultColor = Color.yellow;
            buttonSprite.color = buttonSprite.color == defaultColor ? Color.black : defaultColor;
        }

        public void FilterByString(string _filterMessage)
        {
            _manager.FilterList(_filterMessage);
        }

        public void PauseConsole()
        {
            _manager.PauseList();
        }

        #endregion

        [Serializable]
        public class ActionButtons
        {
            [Header("Actions")] public VerticalLayoutGroup actionsContainer;
            public ActionButtonBehavior actionBtnPrefab;
            [Header("Variables")] public VerticalLayoutGroup variablesContainer;
            public ActionButtonBehavior variablesBtnPrefab;
        }

        [Serializable]
        public class LogDetails
        {
            public Sprite
                logErrorSprite,
                logWarningSprite,
                logInfoSprite;
        }
    }
}