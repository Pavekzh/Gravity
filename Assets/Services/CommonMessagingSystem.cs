using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BasicTools;
using UIExtended;

namespace Assets.Services
{
    public class CommonMessagingSystem : MessagingSystem
    {
        [Header("Error")]
        [SerializeField] ShowElement errorPanel;
        [SerializeField] TMP_Text errorMessageBox;

        [Header("Message")]
        [SerializeField] ShowElement messagePanel;
        [SerializeField] TMP_Text messageBox;

        public override void ShowErrorMessage(string Message, System.Object sender)
        {
            base.ShowErrorMessage(Message,sender);
            errorPanel.Show();
            errorMessageBox.text = "Error: (" + sender.ToString() + ")" + Message;
        }

        public override void ShowMessage(string Message, object sender)
        {
            base.ShowMessage(Message, sender);
            messagePanel.Show();
            messageBox.text = Message;
        }

        public void CloseErrorPanel()
        {
            errorPanel.Hide();
        }

        public void CloseMessagePanel()
        {
            messagePanel.Hide();
        }
    }
}
