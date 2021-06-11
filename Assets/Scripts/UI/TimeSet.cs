using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class TimeSet : MonoBehaviour
{
    [SerializeField]private InputField inputField;
    void Start()
    {
        if(inputField == null)
        {
            ErrorManager.Instance.ShowErrorMessage("InputField value not set");
        }
        TimeManager.Instance.TimeBinding.ValueChanged += ValueChangedOutside;
    }
    public void ValueChanged()
    {
        try
        {
            TimeManager.Instance.TimeBinding.ChangeValue(float.Parse(inputField.text), this);
        }
        catch(Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message);
        }
    }
    public void ValueChangedOutside(float value,object source)
    {
        if(source != (System.Object)this)
        {
            string specifier = "G";
            CultureInfo culture = new CultureInfo("en-US");
            inputField.text = value.ToString(specifier,culture);
        }
    }
}
