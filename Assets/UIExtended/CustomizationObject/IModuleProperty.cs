using UnityEngine;
using System.Collections;
using TMPro;

namespace UIExtended
{
    public interface IModuleProperty
    {
        string[] Labels { get; }
        string ValueLabel { get; }

        void AddInputField(TMP_InputField InputField, uint Index);
    }
}