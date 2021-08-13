using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    bool valueChangeInteract = true;
    private void Start()
    {
        if(slider == null)
        {
            GenericErrorManager.Instance.ShowErrorMessage("Slider value not set",this);
        }
        TimeManager.Instance.TimeBinding.ValueChanged += ValueChangedOutside;
    }
    public void SetTimeScale()
    {
        if(valueChangeInteract)
            TimeManager.Instance.TimeBinding.ChangeValue(slider.value,this);
    }
    private void ValueChangedOutside(float value,object source)
    {
        if(source != (System.Object)this)
        {
            valueChangeInteract = false;
            slider.value = value;
        }
        valueChangeInteract = true;
    }
    private void OnEnable()
    {
        valueChangeInteract = false;
        slider.value = TimeManager.Instance.TimeScale;
        valueChangeInteract = true;
    }
}
