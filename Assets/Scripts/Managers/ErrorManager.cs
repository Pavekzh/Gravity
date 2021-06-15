using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;

public class ErrorManager : Singleton<ErrorManager>
{

    public void ShowErrorMessage(string Message,System.Object sender)
    {
        Debug.LogError(sender.ToString() + ":" +Message);
    }
    public void ShowWarningMessage(string Message,Object sender)
    {
        Debug.LogWarning(sender.ToString() + ":" + Message);
    }
}
