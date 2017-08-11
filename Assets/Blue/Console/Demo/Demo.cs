﻿using UnityEngine;

// By @Bullrich
namespace Blue.Console.Demo
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] private bool showLog;

        public void Start()
        {
            AddActions();
        }

        private void AddActions()
        {
            Debug.Log(Debug.isDebugBuild);
            Debug.Log("Adding methods!");
            GameConsole.AddAction(Ble, "ACTION", 3);
            GameConsole.AddAction(Bla, "This is a bool");
            GameConsole.AddAction(Blu, "Print in console");
            GameConsole.AddAction(error, "Print an error");
            GameConsole.AddAction(DeleteAll, "Delete all!");
            GameConsole.AddAction(error, "Print an error");
            GameConsole.AddAction(error, "Print an error");
            GameConsole.AddAction(warning, "Print a warning");
            GameConsole.AddAction(SeveralErrors, "Throw several errors!");
            GameConsole.AddAction(CustomMessage, "Write a custom message");
        }

        private void Update()
        {
            if (showLog)
            {
                Debug.Log(Time.time);
                showLog = false;
            }
        }

        private void Ble(int ja)
        {
            Debug.Log(ja + " HOLAAAA");
            GameConsole.RemoveAction("ACTION");
        }

        private void DeleteAll()
        {
            GameConsole.RemoveAction("ACTION");
            GameConsole.RemoveAction("This is a bool");
            GameConsole.RemoveAction("Print in console");
            GameConsole.RemoveAction("Print an error");
            GameConsole.RemoveAction("Throw several errors!");
        }

        private void Bla(bool lol)
        {
            Debug.Log("Value is " + lol);
        }

        private void Blu()
        {
            Debug.Log("This print in console");
        }

        private void error()
        {
            Debug.LogError("This is an exception!");
        }

        private void warning()
        {
            Debug.LogWarning("This is a warning");
        }

        private void CustomMessage()
        {
            GameConsole.WriteMessage("Example custom title", "Here goes a custom message");
        }

        private void SeveralErrors()
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
                    default:
                        Debug.LogError(i + " = i, this shouldn't happen");
                        break;
                }
            }
        }
    }
}