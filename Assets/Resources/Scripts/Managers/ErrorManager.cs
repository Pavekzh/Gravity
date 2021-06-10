using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;

public class ErrorManager : Singleton<ErrorManager>
{

    public void ShowErrorMessage(string Message)
    {
        Debug.LogError(Message);
    }
    public void ShowWarningMessage(string Message)
    {
        Debug.LogWarning(Message);
    }
}
