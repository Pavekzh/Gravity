﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BasicTools;
using UIExtended;

namespace Assets.Services
{
    public class CommonErrorManager : ErrorManager
    {
        [SerializeField] ShowElement panelStateSwitcher;
        [SerializeField] TMP_Text messageBox;

        public override void ShowErrorMessage(string Message, System.Object sender)
        {
            Debug.LogError(sender.ToString() + ":" + Message);
            panelStateSwitcher.Show();
            messageBox.text = sender.ToString() + ":" + Message;
        }

        public override void ShowWarningMessage(string Message, System.Object sender)
        {
            Debug.LogWarning(sender.ToString() + ":" + Message);
        }

        public void Close()
        {
            panelStateSwitcher.Hide();
        }
    }
}
