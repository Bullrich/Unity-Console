using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Mady by @Bullrich

namespace Blue.Console
{
    [RequireComponent (typeof(ConsoleOutput))]
    public class ConsoleGUI : MonoBehaviour
    {
        [Tooltip ("Prefab object")]
        public LogInfo logInfo;
        public Transform logScroll, popUpDetail;
        Transform logSection, actionSection;
        Text detailInformation;
        public ActionButtons actionButtons;

        public LogDetails logDetail;
        ConsoleGuiManager guiManager;
        private List<ConsoleActions.ActionContainer> actionsList;

        [SerializeField]
        private SwipeManager swipeManager;

        public void LogMessage(LogType type, string stackTrace, string logMessage)
        {
            LogInfo info = Instantiate (logInfo);
            info.gui = this;
            guiManager.LogMessage (type,
                stackTrace, logMessage, info);
        }

        private void Awake()
        {
            guiManager = new ConsoleGuiManager (
                logScroll.transform.parent.GetComponent<ScrollRect> (), logDetail);
            detailInformation = popUpDetail.GetChild (0).GetChild (0).GetComponent<Text> ();
            CleanConsole ();
            logSection = logScroll.transform.parent;
            actionSection = actionButtons.actionsContainer.transform.parent.parent;
            ConsoleActions.listUpdated += AddListElement;
            GetComponent<ConsoleOutput> ().init (this);
            new TestingConsole ();
        }

        void SetActions()
        {
            List<ConsoleActions.ActionContainer> actions = ConsoleActions.getActions ();
            foreach (ConsoleActions.ActionContainer acon in actions) {
                AddActionElement (acon);
            }
        }

        private void AddActionElement(ConsoleActions.ActionContainer acon)
        {
            ActionButtonBehavior actionButton = actionButtons.actionBtnPrefab;
            Transform parent = actionButtons.actionsContainer.transform;
            if (acon.actType != ConsoleActions.ActionContainer.ActionType._void) {
                actionButton = actionButtons.variablesBtnPrefab;
                parent = actionButtons.variablesContainer.transform;
            }
            ActionButtonBehavior spawnedBtn = Instantiate (actionButton);
            spawnedBtn.transform.SetParent (parent, false);
            spawnedBtn.Init (acon);
            guiManager.AddAction (spawnedBtn);
        }

        void Update()
        {
            if (swipeManager.didSwipe ())
                SwitchConsole ();
        }

        private void SwitchConsole(){
            Image backgroundImage = GetComponent<Image> ();
            backgroundImage.enabled = !backgroundImage.enabled;
            GameObject child = transform.GetChild (0).gameObject;
            child.SetActive (!child.activeSelf);
        }

        private void AddListElement(List<ConsoleActions.ActionContainer> actions)
        {
            if (actionsList == null) {
                actionsList = new List<ConsoleActions.ActionContainer> (actions);
                foreach (ConsoleActions.ActionContainer acon in actionsList)
                    AddActionElement (acon);
            } else {
                int newElements = actions.Count - actionsList.Count;
                if (newElements > 0)
                    for (int i = actionsList.Count; i < actions.Count; i++) {
                        AddActionElement (actions [i]);
                    }
                actionsList = new List<ConsoleActions.ActionContainer> (actions);
            }

        }

        #region ButtonFunctions

        public void ToggleActions()
        {
            actionSection.gameObject.SetActive (!actionSection.gameObject.activeSelf);
            logSection.gameObject.SetActive (!logSection.gameObject.activeSelf);
        }

        public void CleanConsole()
        {
            foreach (Transform t in logScroll)
                Destroy (t.gameObject);
            guiManager.ClearList ();
        }

        public void ShowDetail(LogInfo.ErrorDetail detail)
        {
            detailInformation.text = detail.logString + "\n\n" + detail.stackTrace;
            popUpDetail.gameObject.SetActive (true);
        }

        public void ClosePopUp()
        {
            popUpDetail.gameObject.SetActive (false);
        }

        public void CopyTextToClipboard()
        {
            TextEditor editor = new TextEditor ();
            editor.text = detailInformation.text;
            editor.SelectAll ();
            editor.Copy ();
        }

        public void FilterLogs(FilterAction alertButton)
        {
            guiManager.FilterList (alertButton.logType);
            Image buttonSprite = alertButton.transform.GetChild (0).GetComponent<Image> ();
            Color defaultColor = Color.white;
            if (alertButton.logType == LogType.Error)
                defaultColor = Color.red;
            else if (alertButton.logType == LogType.Warning)
                    defaultColor = Color.yellow;
            buttonSprite.color = buttonSprite.color == defaultColor ? Color.black : defaultColor;
        }

        public void PauseConsole()
        {
            guiManager.PauseList ();
        }

        #endregion

        [System.Serializable]
        public class ActionButtons
        {
            [Header ("Actions")]
            public VerticalLayoutGroup actionsContainer;
            public ActionButtonBehavior actionBtnPrefab;
            [Header ("Variables")]
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

            public TestingConsole ()
            {
                AddActions ();
            }

            private void AddActions()
            {

                Debug.Log (Debug.isDebugBuild);
                ConsoleActions.AddAction (Ble, "ACTION", 3);
                ConsoleActions.AddAction (Bla, "This is a bool", false);
                ConsoleActions.AddAction (Blu, "Print in console");
                ConsoleActions.AddAction (error, "Print an error");
                ConsoleActions.AddAction (warning, "Print a warning");
                ConsoleActions.AddAction (SeveralErrors, "Throw several errors!");
            }

            void Ble(int ja)
            {
                Debug.Log (ja + " HOLAAAA");
            }

            void Bla(bool lol)
            {
                Debug.Log ("Value is " + lol);
            }

            void Blu()
            {
                Debug.Log ("This print in console");
            }

            void error()
            {
                Debug.LogError ("This is an exception!");
            }

            void warning()
            {
                Debug.LogWarning ("This is a warning");
            }

            void SeveralErrors()
            {
                int randomValues = Random.Range (3, 14);
                for (int i = 0; i < randomValues; i++) {
                    int newRand = Random.Range (0, 3);
                    switch (newRand) {
                    case 0:
                        Debug.Log ("This is " + i + " a log!");
                        break;
                    case 1:
                        Debug.LogWarning ("This is " + i + " warning!");
                        break;
                    case 2:
                        Debug.LogError (i + " | This is an assets known as a kind of error!");
                        break;
                    }
                }
            }
        }

        [System.Serializable]
        private class SwipeManager
        {
                
            Vector2 firstPressPos;
            Vector2 secondPressPos;
            Vector2 currentSwipe;

            public enum swDirection
            {
                left,
right,
down,
up

            }

            public swDirection swipeDirection = swDirection.down;
            [Range(1, 4)]
            public int fingersNeed = 2;

            public bool didSwipe()
            {
                return SwipedInDirection ();
            }

            #if !UNITY_EDITOR || UNITY_STANDALONE
            
                public void Swipe()
                {
                if(Input.GetMouseButtonDown(0))
                {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
                }
                if(Input.GetMouseButtonUp(0))
                {
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);

                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                Debug.Log("up swipe");
                }
                //swipe down
                if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                Debug.Log("down swipe");
                }
                //swipe left
                if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                Debug.Log("left swipe");
                }
                //swipe right
                if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                Debug.Log("right swipe");
                }
                }
                }
                #endif

            #if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)

            public bool SwipedInDirection()
            {
                if (Input.touches.Length > fingersNeed - 1) {
                    Touch t = Input.GetTouch (0);
                    if (t.phase == TouchPhase.Began) {
                        //save began touch 2d point
                        firstPressPos = new Vector2 (t.position.x, t.position.y);
                    }
                    if (t.phase == TouchPhase.Ended) {
                        print ("ENTERED");
                        //save ended touch 2d point
                        secondPressPos = new Vector2 (t.position.x, t.position.y);

                        //create vector from the two points
                        currentSwipe = new Vector3 (secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                        //normalize the 2d vector
                        currentSwipe.Normalize ();

                        //swipe upwards
                        if (swipeDirection == swDirection.up && currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                            return true;
                            //Debug.Log("up swipe");
                        }
                        //swipe down
                        else if (swipeDirection == swDirection.down && currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                            //Debug.Log("down swipe");
                            return true;
                        }
                        //swipe left
                        else if (swipeDirection == swDirection.left && currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                            return true;
                            //Debug.Log("left swipe");
                        }
                        //swipe right
                        else if (swipeDirection == swDirection.right && currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                            return true;
                            //Debug.Log("right swipe");
                        }
                    }
                }
                return false;
            }

            #endif
        }
    }
}