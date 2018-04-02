using System;
using UnityEngine;

namespace Blue.Console.Handler
{
    public class ShareHandler
    {
        private readonly string _defaultMailDirectory;
        private string _mailSubject;

        public ShareHandler(string defaulMail = "example@gmail.com")
        {
            _defaultMailDirectory = defaulMail;
        }

        public void Share(string subject, string text)
        {
            _mailSubject = subject;
#if UNITY_ANDROID
            SendEmail(text);
#else
            ShareTextOnAndroid(Application.productName, text);
#endif
        }
        
        private void SendEmail(string messageBody)
        {
            string email = _defaultMailDirectory;
            string subject = MyEscapeUrl(_mailSubject);
            string body = MyEscapeUrl(messageBody);
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        private static string MyEscapeUrl(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }

#if UNITY_ANDROID
        private void ShareTextOnAndroid(string messageTitle, string messageBody)
        {
            try
            {
                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
                intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
                intentObject.Call<AndroidJavaObject>("setType", "text/plain");
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"),
                    messageTitle);
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
                    messageBody);
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("startActivity", intentObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                SendEmail(messageBody);
            }
        }
#endif
    }
}