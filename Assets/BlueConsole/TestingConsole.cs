using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by @Bullrich

namespace game
{
	public class TestingConsole : MonoBehaviour {

		// Use this for initialization
		void Start ()
        {
            Blue.Console.Console.instance.AddCommand(new Blue.Console.Command("test", "This print macri").addVoidCommand(TestCommand));
            Blue.Console.Console.instance.AddCommand(new Blue.Console.Command("capital", "This mae the word in capital").addStringString(MakeInCapital));
        }

        void TestCommand()
        {
            print("Macri");
        }

        string MakeInCapital(string ble)
        {
            return ble.ToUpper();
        }
	}
}
