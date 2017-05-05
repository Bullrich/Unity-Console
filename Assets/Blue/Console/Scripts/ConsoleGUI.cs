using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Mady by @Bullrich

namespace Blue.Console
{
    [RequireComponent(typeof(ConsoleOutput))]
    public class ConsoleGUI : MonoBehaviour
    {
        [Tooltip("Prefab object")]
        public LogInfo logInfo;
        public Transform logScroll, popUpDetail;
        Transform logSection, actionSection;
        Text detailInformation;
        public ActionButtons actionButtons;

        public LogDetails logDetail;
        ConsoleGuiManager guiManager;
        private List<ActionContainer> actionsList;
        SwipeManager openConsoleSettings;
        private string mailSubject;
        public string DefaultMailDirectory = "example@gmail.com";

        public void LogMessage(LogType type, string stackTrace, string logMessage)
        {
            LogInfo info = Instantiate(logInfo);
            info.gui = this;
            guiManager.LogMessage(type,
                stackTrace, logMessage, info);
        }

        public void init(SwipeManager swipe)
        {
            openConsoleSettings = swipe;
            guiManager = new ConsoleGuiManager(
                logScroll.transform.parent.GetComponent<ScrollRect>(), logDetail);
            detailInformation = popUpDetail.GetChild(0).GetChild(0).GetComponent<Text>();
            CleanConsole();
            logSection = logScroll.transform.parent;
            actionSection = actionButtons.actionsContainer.transform.parent.parent;
            ConsoleActions.listUpdated += AddListElement;
            GetComponent<ConsoleOutput>().init(this);
            //new TestingConsole();
            if (!Debug.isDebugBuild)
                Debug.LogWarning("This isn't a development build! You won't be able to read the stack trace!");
        }

        void SetActions()
        {
            List<ActionContainer> actions = new List<ActionContainer>(ConsoleActions.getActions());
            foreach (ActionContainer acon in actions)
            {
                AddActionElement(acon);
            }
        }


        private void AddActionElement(ActionContainer acon)
        {
            ActionButtonBehavior actionButton = actionButtons.actionBtnPrefab;
            Transform parent = actionButtons.actionsContainer.transform;
            if (acon.actType != ActionContainer.ActionType._void)
            {
                actionButton = actionButtons.variablesBtnPrefab;
                parent = actionButtons.variablesContainer.transform;
            }
            ActionButtonBehavior spawnedBtn = Instantiate(actionButton);
            spawnedBtn.transform.SetParent(parent, false);
            spawnedBtn.Init(acon);
            guiManager.AddAction(spawnedBtn);
        }

        void Update()
        {
            if (openConsoleSettings.didSwipe())
                SwitchConsole();
        }

        public void SwitchConsole()
        {
            Image backgroundImage = GetComponent<Image>();
            backgroundImage.enabled = !backgroundImage.enabled;
            GameObject child = transform.GetChild(0).gameObject;
            child.SetActive(!child.activeSelf);
        }

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
                actionsList = new List<ActionContainer>(actions);
            }
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
            guiManager.ClearList();
        }

        public void ShowDetail(LogInfo.ErrorDetail detail)
        {
            detailInformation.text = detail.logString + "\n\n" + detail.stackTrace;
            mailSubject = string.Format("[{0}] {1}", detail.errorType.ToString(), detail.logString);
            popUpDetail.gameObject.SetActive(true);
        }

        public void ClosePopUp()
        {
            popUpDetail.gameObject.SetActive(false);
        }

        public void CopyTextToClipboard()
        {
            string textToSend = detailInformation.text;
#if !UNITY_ANDROID
            SendEmail(textToSend);
#else
            ShareTextOnAndroid(Application.productName, textToSend);
#endif
        }
        void SendEmail(string messageBody)
        {
            string email = DefaultMailDirectory;
            string subject = MyEscapeURL(mailSubject);
            string body = MyEscapeURL(messageBody);
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }
        string MyEscapeURL(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }

#if UNITY_ANDROID
        [System.Obsolete("Deprecated because SendEmail works better and it's multiplatform")]
        private void ShareTextOnAndroid(string messageTitle, string messageBody)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), messageTitle);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), messageBody);
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
        }
#endif

        public void FilterLogs(FilterAction alertButton)
        {
            guiManager.FilterList(alertButton.logType);
            Image buttonSprite = alertButton.transform.GetChild(0).GetComponent<Image>();
            Color defaultColor = Color.white;
            if (alertButton.logType == LogType.Error)
                defaultColor = Color.red;
            else if (alertButton.logType == LogType.Warning)
                defaultColor = Color.yellow;
            buttonSprite.color = buttonSprite.color == defaultColor ? Color.black : defaultColor;
        }

        public void PauseConsole()
        {
            guiManager.PauseList();
        }

        #endregion

        [System.Serializable]
        public class ActionButtons
        {
            [Header("Actions")]
            public VerticalLayoutGroup actionsContainer;
            public ActionButtonBehavior actionBtnPrefab;
            [Header("Variables")]
            public VerticalLayoutGroup variablesContainer;
            public ActionButtonBehavior variablesBtnPrefab;
        }

        [System.Serializable]
        public class LogDetails
        {
            public Sprite
                logErrorSprite,
                logWarningSprite,
                logInfoSprite;
        }

        private class TestingConsole
        {
            public TestingConsole()
            {
                AddActions();
            }

            private void AddActions()
            {
                Debug.Log(Debug.isDebugBuild);
                ConsoleActions.AddAction(Ble, "ACTION", 3);
                ConsoleActions.AddAction(Bla, "This is a bool", false);
                ConsoleActions.AddAction(Blu, "Print in console");
                ConsoleActions.AddAction(error, "Print an error");
                ConsoleActions.AddAction(warning, "Print a warning");
                ConsoleActions.AddAction(SeveralErrors, "Throw several errors!");
            }

            void Ble(int ja)
            {
                Debug.Log(ja + " HOLAAAA");
            }

            void Bla(bool lol)
            {
                Debug.Log("Value is " + lol);
            }

            void Blu()
            {
                Debug.Log("This print in console");
            }

            void error()
            {
                Debug.LogError("This is an exception!");
            }

            void warning()
            {
                Debug.LogWarning("This is a warning");
            }

            void SeveralErrors()
            {
                int randomValues = Random.Range(3, 14);
                for (int i = 0; i < randomValues; i++)
                {
                    int newRand = Random.Range(0, 3);
                    switch (newRand)
                    {
                        case 0:
                            Debug.Log("This is " + i + " a log!");
                            break;
                        case 1:
                            Debug.LogWarning("This is " + i + " warning!");
                            break;
                        case 2:
                            Debug.LogError(i + " | This is an assets known as a kind of error!");
                            break;
                    }
                }
            }
        }
    }
}