using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;

namespace BasicTools
{
    public abstract class ErrorManager : Singleton<ErrorManager>
    {
        public abstract void ShowErrorMessage(string Message, System.Object sender);
        public abstract void ShowWarningMessage(string Message, System.Object sender);
    }
}

