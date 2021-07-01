using UnityEngine;
using System.Collections;
using Assets.Library;

public class TimeChangeImage : ChangeImage,IReactPhysicsState
{
    // Use this for initialization
    protected override void Start()
    {
        TimeRefreshed();
        TimeManager.Instance.TimeSettingsRefreshed += TimeRefreshed;
        base.Start();
    }
    private void TimeRefreshed()
    {
        TimeManager.Instance.AddObserver(this);
    }
    public void UpdatePhysicsState(bool state)
    {
        if(state == true)
        {
            ChangeState(State.Default);
        }
        else
        {
            ChangeState(State.Changed);
        }
    }
}
