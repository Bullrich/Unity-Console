using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
//by @Bullrich

namespace Blue.Console
{
    public class Console : MonoBehaviour
    {
        GameObject console;
        [SerializeField]
        private KeyCode toggleConsole = KeyCode.Tab;
        [SerializeField]
        private Text commandsTxt;
        [SerializeField]
        private InputField cmdInputs;
        [SerializeField]
        private Scrollbar scrllBar;

        public static Console instance;

        Dictionary<string, Command> commands = new Dictionary<string, Command>();

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void AddCommand(Command newCommand)
        {
            if (newCommand.commandType != Command.TypeOfCommand.NOTHING)
            {
                if (!commands.ContainsKey(newCommand.commandKey))
                    commands.Add(newCommand.commandKey, newCommand);
                else
                    Debug.LogError("Key " + newCommand.commandKey + " already exist!");
            }
            else
                Debug.LogError("Please be sure to add a delegate to the command!");
        }

        void Start()
        {
            console = transform.GetChild(0).gameObject;
            scrllBar.value = 0;
        }
        void Update()
        {
            if (Input.GetKeyDown(toggleConsole))
            {
                console.SetActive(!console.activeSelf);
                cmdInputs.OnSelect(null);
            }
        }

        public void WriteInput()
        {
            WriteInConsole(cmdInputs.text);
            ExecuteFunction(cmdInputs.text);    
            cmdInputs.text = "";
            cmdInputs.OnSelect(null);
        }

        public void WriteInConsole(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                commandsTxt.text += "\n" + text;
                scrllBar.value = 0;
            }
        }

        #region Command Execution
        void ExecuteFunction(string execCommand)
        {
            if (!string.IsNullOrEmpty(execCommand))
            {
                int count = execCommand.Split(' ').Length - 1;
                if (execCommand.Contains(" "))
                {
                    var commandSplitted = execCommand.Split(new char[] { ' ' }, 2);
                    ExecuteParameterCommand(commandSplitted[0], commandSplitted[1]);
                }
                else
                    ExecuteVoidCommand(execCommand);
            }
        }

        void ExecuteVoidCommand(string singleCommand)
        {
            if (commands.ContainsKey(singleCommand))
            {
                Command cmdToExecute = commands[singleCommand];
                if (cmdToExecute.commandType == Command.TypeOfCommand.voidCommand)
                    cmdToExecute._voidCmd();
                else
                    WriteInConsole(cmdToExecute._stringVoid());
            }
            else
                WriteInConsole("ERROR: Command <" + singleCommand + "> not found!");
        }
        void ExecuteParameterCommand(string commandKey, string parameters)
        {
            if (commands.ContainsKey(commandKey)) {
                Command cmdToExecute = commands[commandKey];
                if (cmdToExecute.commandType == Command.TypeOfCommand.stringString)
                    WriteInConsole(cmdToExecute._stringString(parameters));
                else
                    cmdToExecute._voidString(parameters);
            }
            else
                WriteInConsole("ERROR: Command <" + commandKey + "> with parameters not found!");
        }
#endregion
    }

    public class Command
    {
        /// <summary>The string by which this command will be called</summary>
		public string commandKey;
        /// <summary>The information of this command that the help option will show</summary>
		public string helpInfo;

        public delegate string stringVoid();
        public delegate string stringString(string consoleInput);
        public delegate void voidCmd();
        public delegate void voidString(string consoleInput);

        public stringVoid _stringVoid;
        public stringString _stringString;
        public voidCmd _voidCmd;
        public voidString _voidString;

        public enum TypeOfCommand
        {
            voidCommand, stringCommand, voidString, stringString, NOTHING
        }
        public TypeOfCommand commandType = TypeOfCommand.NOTHING;

        public Command(string commandInput, string helpTooltip)
        {
            commandKey = commandInput;
            helpInfo = helpTooltip;
        }

        #region Add Command Methods
        public Command addVoidCommand(voidCmd voidCommand)
        {
            _voidCmd = voidCommand;
            if (commandType == TypeOfCommand.NOTHING) commandType = TypeOfCommand.voidCommand;
            else Debug.LogError("More than one method was sent, error 12");
            return this;
        }
        public Command addStringVoid(stringVoid stringVoidCmd)
        {
            _stringVoid = stringVoidCmd;
            if (commandType == TypeOfCommand.NOTHING) commandType = TypeOfCommand.stringCommand;
            else Debug.LogError("More than one method was sent, error 12");
            return this;
        }

        public Command addStringString(stringString stringStringCmd)
        {
            _stringString = stringStringCmd;
            if (commandType == TypeOfCommand.NOTHING) commandType = TypeOfCommand.stringString;
            else Debug.LogError("More than one method was sent, error 12");
            return this;
        }

        public Command addVoidString(voidString voidStringCmd)
        {
            _voidString = voidStringCmd;
            if (commandType == TypeOfCommand.NOTHING) commandType = TypeOfCommand.voidString;
            else Debug.LogError("More than one method was sent, error 12");
            return this;
        }
        #endregion
    }
}