using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//by @Bullrich

namespace Blue.Console
{
	public class Console : MonoBehaviour
	{
		GameObject console;
		public KeyCode toggleConsole = KeyCode.Tab;
		public Text commandsTxt;
		public InputField cmdInputs;
		public Scrollbar scrllBar;
		public delegate void voidCommand();
		[SerializeField]
		public Commands cmds;

		void Start ()
		{
			console = transform.GetChild (0).gameObject;
			scrllBar.value = 0;
            cmds.evtCommand.Invoke();
		}

		void Update ()
		{
			if (Input.GetKeyDown (toggleConsole))
				console.SetActive (!console.activeSelf);
		}

		public void WriteInput ()
		{
			WriteInConsole (cmdInputs.text);
			cmdInputs.text = "";
			cmdInputs.OnSelect (null);
		}

		void WriteInConsole (string text)
		{
			if (!string.IsNullOrEmpty (text)) {
				commandsTxt.text += text + "\n";
				scrllBar.value = 0;
			}
		}
	}

	[System.Serializable]
	public class Commands{
		public Console.voidCommand command;
		public UnityEngine.Events.UnityEvent evtCommand;
		[Tooltip("The string by which this command will be called")]
		public string commandText;
		[Tooltip("The information of this command that the help option will show")]
		public string helpInfo;
	}
}