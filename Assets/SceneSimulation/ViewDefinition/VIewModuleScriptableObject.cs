using System;
using UnityEngine;
using Assets.SceneSimulation;
using Assets.SceneEditor.Models;


public abstract class ViewModuleScriptableObject : ScriptableObject
{
    public abstract IViewModuleData CreateModuleData();
    protected abstract void UpdateViewModule(IViewModuleData moduleData);

    public void UpdateModule(IViewModuleData moduleData)
    {
        this.UpdateViewModule(moduleData);
        moduleData.UpdateView();
    }
}

