using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;
using TMPro;

public class ErrorManager : Singleton<ErrorManager>
{
    [SerializeField] ShowElement panelStateSwitcher;
    [SerializeField] TMP_Text messageBox;

    public void ShowErrorMessage(string Message,System.Object sender)
    {
        Debug.LogError(sender.ToString() + ":" +Message);
        panelStateSwitcher.Show();
        messageBox.text =sender.ToString() +":"+ Message;
    }
    public void ShowWarningMessage(string Message,Object sender)
    {
        Debug.LogWarning(sender.ToString() + ":" + Message);
    }
}
