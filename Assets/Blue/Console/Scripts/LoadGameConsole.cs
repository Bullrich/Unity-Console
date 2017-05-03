using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mady by @Bullrich

namespace Blue.Console
{
    public class LoadGameConsole : MonoBehaviour
    {
        public GameObject gameConsole;

        void Awake()
        {
            if (GameObject.Find (gameConsole.name) == null) {
                StartCoroutine (InitConsole ());
            } else
                Debug.LogWarning ("Tried to spawn console, but it already exists!");
        }

        IEnumerator InitConsole(){
            GameObject console = Instantiate (gameConsole);
            console.name = gameConsole.name;
            ConsoleGUI guiConsole = console.transform.GetChild(0).GetComponent<ConsoleGUI> ();
            guiConsole.init ();
            guiConsole.ToggleActions ();

            DontDestroyOnLoad (console);
            yield return new WaitForEndOfFrame ();
            guiConsole.SwitchConsole ();
            guiConsole.ToggleActions ();
            Destroy (gameObject);
        }
    }
}