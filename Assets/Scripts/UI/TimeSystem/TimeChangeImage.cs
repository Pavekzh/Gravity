using UnityEngine;
using System.Collections;
using Assets.Library;

public class TimeChangeImage : ChangeImage,IReactPhysicsState
{

    // Use this for initialization
    void Start()
    {
        TimeRefreshed();
        TimeManager.Instance.TimeSettingsRefreshed += TimeRefreshed;
    }
    private void TimeRefreshed()
    {
        TimeManager.Instance.AddObserver(this);
    }
    public void UpdatePhysicsState(bool state)
    {
        if(state == true)
        {
            imageField.sprite = defaultState;
            isDefaultState = true;
        }
        else
        {
            imageField.sprite = changedState;
            isDefaultState = false;
        }
    }
}
