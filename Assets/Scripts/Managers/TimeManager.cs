using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;
using System;
using BasicTools;

public class TimeManager : Singleton<TimeManager>
{
    private List<IReactPhysicsState> modules = new List<IReactPhysicsState>();
    private float defaultFixedDeltaTime;

    public Binding<float> TimeBinding { get; private set; }
    [SerializeField] private float timeScale;
    [SerializeField] private bool isPhysicsEnabled = true;
    [SerializeField] private float maxTimeScale = 2;

    public float TimeScale { get => timeScale; }
    public bool IsPhysicsEnabled { get => isPhysicsEnabled; }

    public event SceneStateManager.SceneRefreshHandler TimeSettingsRefreshed;
    void RefreshSettings()
    {
        this.modules = new List<IReactPhysicsState>();
        TimeSettingsRefreshed?.Invoke();
    }
    public void AddObserver(IReactPhysicsState Module)
    {
        this.modules.Add(Module);
        Module.UpdatePhysicsState(this.isPhysicsEnabled);
    }
    public void RemoveObserver(IReactPhysicsState Module)
    {
        this.modules.Remove(Module);
    }

    //method for external pause game
    public void StopPhysics()
    {
        TimeBinding.ChangeValue(0,this);
        LocalStopPhysics();
    }
    //method for external resume game
    public void ResumePhysics()
    {

        TimeBinding.ChangeValue(Time.timeScale, this);
        LocalResumePhysics();
    }
    public void ChangePhysicsState()
    {
        if (isPhysicsEnabled)
        {
            TimeBinding.ChangeValue(0, this);
            LocalStopPhysics();
        }
        else
        {
            TimeBinding.ChangeValue(Time.timeScale, this);
            LocalResumePhysics();
        }
    }

    private bool ValidateTimeChanges(float value, object source)
    {
        if(value <= maxTimeScale)
        {
            return true;
        }
        else
        {
            ResetTimeScale(maxTimeScale, null);
            TimeBinding.ChangeValue(maxTimeScale,this);
            return false;
        }
    }
    private void Start()
    {
        TimeBinding = new Binding<float>();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        TimeBinding = new Binding<float>();
        TimeBinding.ValueChanged += ResetTimeScale;
        TimeBinding.ValidateValue += ValidateTimeChanges;
        SceneStateManager.Instance.SceneRefreshed += RefreshSettings;
        if(isPhysicsEnabled == false)
        {
            LocalStopPhysics();
        }
        else
        {
            LocalResumePhysics();
        }
    }
    private void ResetTimeScale(float value,object source)
    {
        if(source != (System.Object)this)
        {
            if (value != 0 && timeScale != 0)
            {
                timeScale = value;
                Time.timeScale = value;
                Time.fixedDeltaTime = defaultFixedDeltaTime * value;
            }
            else if (value != 0 && timeScale == 0)
            {
                timeScale = value;
                Time.timeScale = value;
                Time.fixedDeltaTime = defaultFixedDeltaTime * value;
                LocalResumePhysics();
            }
            else
            {
                LocalStopPhysics();
            }
        }
    }


    //method used when time stoped by setting TimeScale to 0
    private void LocalStopPhysics()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        foreach (IReactPhysicsState module in modules)
        {
            module.UpdatePhysicsState(false);
        }
        timeScale = 0;
        isPhysicsEnabled = false;
    }
    //method used when time resume by setting TimeScale not to 0
    private void LocalResumePhysics()
    {
        foreach (IReactPhysicsState module in modules)
        {
            module.UpdatePhysicsState(true);
        }
        timeScale = Time.timeScale;
        isPhysicsEnabled = true;
    }

}
