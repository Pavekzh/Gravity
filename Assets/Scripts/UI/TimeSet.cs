using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using TMPro;

public class TimeSet : MonoBehaviour
{
    [SerializeField]private TMP_InputField inputField;
    void Start()
    {
        if(inputField == null)
        {
            ErrorManager.Instance.ShowErrorMessage("InputField value not set",this);
        }
        TimeManager.Instance.TimeBinding.ValueChanged += ValueChangedOutside;
    }
    public void ValueChanged()
    {
        try
        {
            TimeManager.Instance.TimeBinding.ChangeValue(float.Parse(inputField.text, new CultureInfo("en-US")), this);
        }
        catch(Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message,this);
        }
    }
    public void ValueChangedOutside(float value,object source)
    {
        if(source != (System.Object)this)
        {
            string specifier = "G";
            CultureInfo culture = new CultureInfo("en-US");
            string text = value.ToString(specifier,culture);

            if (text.Length > 3)
                inputField.text = text.Substring(0, 3);
            else
                inputField.text = text;
        }
    }
    private void OnEnable()
    {
        string specifier = "G";
        CultureInfo culture = new CultureInfo("en-US");
        string text = TimeManager.Instance.TimeScale.ToString(specifier, culture);

        if (text.Length > 3)
            inputField.text = text.Substring(0, 3);
        else
            inputField.text = text;
    }
}
