using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;

namespace BasicTools
{
    public class MessagingSystem : Singleton<MessagingSystem>
    {
        public virtual void ShowErrorMessage(string Message, System.Object sender)
        {
            Debug.LogError(sender.ToString() + ": " + Message);
        }
        public virtual void ShowWarningMessage(string Message, System.Object sender)
        {
            Debug.LogWarning(sender.ToString() + ": " + Message);
        }
        public virtual void ShowMessage(string Message,System.Object sender)
        {
            Debug.Log(sender.ToString() + ": " + Message);
        }
    }
}

