using System;
using UnityEngine;
using Assets.SceneSimulation;
using Assets.SceneEditor.Models;


public abstract class ViewModuleScriptableObject : ScriptableObject
{
    public abstract ViewModuleData CreateModuleData();
    protected abstract void UpdateViewModule(ViewModuleData moduleData);

    public void UpdateModule(ViewModuleData moduleData)
    {
        this.UpdateViewModule(moduleData);
        moduleData.UpdateView();
    }
}

