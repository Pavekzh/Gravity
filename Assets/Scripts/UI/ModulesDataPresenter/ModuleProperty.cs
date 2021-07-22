using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Assets.Library;
using TMPro;
using System.Globalization;

public class ModuleProperty<T>:IModuleProperty
{
    public delegate string[] ConvertToInput(T value);
    public delegate T ConvertFromInput(string[] input);

    private Binding<T> binding;
    private ConvertFromInput convertMethodFrom;
    private ConvertToInput converMethodTo;
    private TMP_InputField[] inputFields;
    private string[] startValueRepresentation;

    private string[] InputTexts
    {
        get
        {
            string[] inputTexts = new string[inputFields.Length];
            for(int i = 0;i < inputFields.Length; i++)
            {
                inputTexts[i] = inputFields[i].text;
            }
            return inputTexts;
        }
        set
        {
            if (value.Length != inputFields.Length)
                throw new System.Exception("inputTexts array must have same length with inputFields");
            else
            {
                for (int i = 0; i < value.Length; i++)
                {
                    inputFields[i].text = value[i];
                }
            }
        }
    }

    public string[] Labels { get; private set; }
    public string ValueLabel { get; private set; }

    private bool inputEntering = false;

    public void AddInputField(TMP_InputField inputField,uint index)
    {
        if (index < inputFields.Length)
        {
            inputFields[index] = inputField;
            inputField.text = startValueRepresentation[index];
            UnityAction<string> action = new UnityAction<string>(InputChanged);
            inputField.onValueChanged.AddListener(action);
            UnityAction<string> selectAction = new UnityAction<string>(InputFieldSelected);
            inputField.onSelect.AddListener(selectAction);
            UnityAction<string> deselectAction = new UnityAction<string>(InputFieldDeselected);
            inputField.onDeselect.AddListener(deselectAction);
        }
        else
            throw new System.IndexOutOfRangeException("Index was out of range");
    }
    public ModuleProperty(string[] Labels,string ValueLabel,ConvertFromInput ConvertFromInput,ConvertToInput ConvertToInput,Binding<T> binding,T startValue)
    {
        this.inputFields = new TMP_InputField[Labels.Length];
        this.Labels = Labels;
        this.ValueLabel = ValueLabel;
        this.convertMethodFrom = ConvertFromInput;
        this.converMethodTo = ConvertToInput;
        this.startValueRepresentation = ConvertToInput(startValue);
        this.binding = binding;
        this.binding.ValueChanged += ValueChangedOutside;
    }
    private void InputChanged(string changedValue)
    {
        binding.ChangeValue(convertMethodFrom(InputTexts), this);
    }
    private void ValueChangedOutside(T value,object source)
    {
        if (source != this && inputEntering != true)
        {
            InputTexts = converMethodTo(value);
        }
    }
    private void InputFieldSelected(string value)
    {
        inputEntering = true;
    }
    private void InputFieldDeselected(string value)
    {
        inputEntering = false;
    }

    private static CultureInfo culture = new CultureInfo("en-US");

    public static float FloatFromInputConverter(string[] input)
    {
        if (input[0] != "" && input[0] != "-")
            return float.Parse(input[0], culture);
        else
            return 0;
    }
    public static string[] FloatToInputConverter(float value)
    {
        return new string[1] { value.ToString() };
    }

    public static Vector2 Vector2FromInputConverter(string[] input)
    {
        float x = 0;
        float y = 0;

        if (input[0] != "" && input[0] != "-")
            x = float.Parse(input[0], culture);
        if (input[1] != "" && input[1] != "-")
            y = float.Parse(input[1], culture);

        return new Vector2(x,y);
    }
    public static string[] Vector2ToInputConverter(Vector2 value)
    {
        return new string[2] {value.x.ToString(culture),value.y.ToString(culture) };
    }
}
