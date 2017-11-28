using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{

    public bool takeScreenshot = false;
	
	// Update is called once per frame
	void Update () {
	    if (takeScreenshot)
	    {
	        string screenshotName =
	            "screenshot" + (Time.timeSinceLevelLoad).ToString().Replace(".", string.Empty) + ".png";
	        Application.CaptureScreenshot(screenshotName);
	        takeScreenshot = false;
	        print("Saved screenshot as " + screenshotName);
	    }
	}
}
