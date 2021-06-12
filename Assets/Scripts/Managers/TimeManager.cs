using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;
using System;

public class TimeManager : Singleton<TimeManager>
{
    private List<Module> modules = new List<Module>();
    private float defaultFixedDeltaTime;

    public Binding<float> TimeBinding { get; private set; }
    [SerializeField]private float timeScale;

    void RefreshSettings()
    {
        this.modules = new List<Module>();
    }
    public void AddObserver(Module Module)
    {
        this.modules.Add(Module);
    }
    public void RemoveObserver(Module Module)
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

    private void Start()
    {
        TimeBinding = new Binding<float>();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        TimeBinding = new Binding<float>();
        TimeBinding.ValueChanged += ResetTimeScale;
        SceneStateManager.Instance.OnSceneRefresh += RefreshSettings;
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

        foreach (Module module in modules)
        {
            module.UpdatePhysicsState(false);
        }
        timeScale = 0;
    }
    //method used when time resume by setting TimeScale not to 0
    private void LocalResumePhysics()
    {
        foreach (Module module in modules)
        {
            module.UpdatePhysicsState(true);
        }
        timeScale = Time.timeScale;
    }
}
