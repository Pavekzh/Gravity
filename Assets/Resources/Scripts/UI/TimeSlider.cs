using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Start()
    {
        if(slider == null)
        {
            ErrorManager.Instance.ShowErrorMessage("Slider value not set");
        }
        TimeManager.Instance.TimeBinding.ValueChanged += ValueChangedOutside;
    }
    public void SetTimeScale()
    {
        TimeManager.Instance.TimeBinding.ChangeValue(slider.value,this);
    }
    private void ValueChangedOutside(float value,object source)
    {
        if(source != (System.Object)this)
        {
            slider.value = value;
        }

    }
}
