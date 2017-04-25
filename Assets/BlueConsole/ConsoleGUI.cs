﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Mady by @Bullrich

namespace Blue.Console {
    public class ConsoleGUI : MonoBehaviour {
        [Tooltip("Prefab object")]
        public LogInfo logInfo;
        public Transform logScroll, popUpDetail;
        Text detailInformation;

        public LogDetails logDetail;
        ConsoleGuiManager guiManager;

        public void LogMessage(LogType type, string stackTrace, string logMessage) {
            LogInfo info = Instantiate(logInfo);
            info.gui = this;
            guiManager.LogMessage(type,
                stackTrace, logMessage, info);
        }

        private void Start() {
            guiManager = new ConsoleGuiManager(
                logScroll.transform.parent.GetComponent<ScrollRect>(), logDetail);
            detailInformation = popUpDetail.GetChild(0).GetChild(0).GetComponent<Text>();
            CleanConsole();
        }

        void Update() {
            if (Input.GetMouseButtonDown(1)) {
                Image backgroundImage = GetComponent<Image>();
                backgroundImage.enabled = !backgroundImage.enabled;
                GameObject child = transform.GetChild(0).gameObject;
                child.SetActive(!child.activeSelf);
            }
        }

        #region ButtonFunctions
        public void CleanConsole() {
            foreach (Transform t in logScroll)
                Destroy(t.gameObject);
            guiManager.ClearList();
        }

        public void ShowDetail(LogInfo.ErrorDetail detail) {
            detailInformation.text = detail.logString + "\n\n" + detail.stackTrace;
            popUpDetail.gameObject.SetActive(true);
        }

        public void ClosePopUp() {
            popUpDetail.gameObject.SetActive(false);
        }

        public void CopyTextToClipboard() {
            TextEditor editor = new TextEditor();
            editor.text = detailInformation.text;
            editor.SelectAll();
            editor.Copy();
        }

        public void FilterLogs(FilterAction alertButton) {
            guiManager.FilterList(alertButton.logType);
            Image buttonSprite = alertButton.transform.GetChild(0).GetComponent<Image>();
            buttonSprite.color = buttonSprite.color == Color.white ? Color.black : Color.white;
        }
        #endregion
    }

    [System.Serializable]
    public class LogDetails {
        public Sprite
            logErrorSprite,
            logWarningSprite,
            logInfoSprite;
    }
}