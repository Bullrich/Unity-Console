using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Mady by @Bullrich

namespace Blue.Console
{
    public class LoadGameConsole : MonoBehaviour
    {
        public GameObject gameConsole;
        [SerializeField]
        public SwipeManager swipeOptions;

        private void Awake()
        {
            if (GameObject.Find(gameConsole.name) == null)
            {
                StartCoroutine(InitConsole());
            }
            else
                Debug.LogWarning("Tried to spawn console, but it already exists!");
        }

        private IEnumerator InitConsole()
        {
            const string _eventSystemName = "EventSystem";
            GameObject eventSystem = GameObject.Find(_eventSystemName);
            GameObject console = Instantiate(gameConsole);
            console.name = gameConsole.name;
            ConsoleGUI guiConsole = console.transform.GetChild(0).GetComponent<ConsoleGUI>();
            guiConsole.init(swipeOptions);
            guiConsole.ToggleActions();
            if (Screen.width > Screen.height)
            {
                CanvasScaler scaler = console.GetComponent<CanvasScaler>();
                scaler.referenceResolution = new Vector2(1800, 600);
            }

            DontDestroyOnLoad(console);
            yield return new WaitForEndOfFrame();
            guiConsole.SwitchConsole();
            guiConsole.ToggleActions();
            if (eventSystem == null)
            {
                GameObject _eventSystem = new GameObject(_eventSystemName);
                _eventSystem.AddComponent<EventSystem>();
                _eventSystem.AddComponent<StandaloneInputModule>();
                _eventSystem.transform.position = Vector3.zero;
                DontDestroyOnLoad(_eventSystem);
            }
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class SwipeManager
    {

        Vector2
            firstPressPos,
            secondPressPos,
            currentSwipe;

        public enum swDirection
        {
            left, right, down, up
        }

        public swDirection swipeDirection = swDirection.down;
        [Range(1, 4)]
        public int fingersNeed = 2;

        public KeyCode openConsoleKey = KeyCode.Tab;

        public bool didSwipe()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
                return SwipedInDirection ();
#else
            if (Input.GetKeyDown(openConsoleKey))
                return true;
            return false;
#endif
        }


#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)

            public bool SwipedInDirection()
            {
                if (Input.touches.Length > fingersNeed - 1) {
                    Touch t = Input.GetTouch (0);
                    if (t.phase == TouchPhase.Began) {
                        //save began touch 2d point
                        firstPressPos = new Vector2 (t.position.x, t.position.y);
                    }
                    if (t.phase == TouchPhase.Ended) {
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
                        } else if (swipeDirection == swDirection.down && currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                                //Debug.Log("down swipe");
                                return true;
                            } else if (swipeDirection == swDirection.left && currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                                    return true;
                                    //Debug.Log("left swipe");
                                } else if (swipeDirection == swDirection.right && currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
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