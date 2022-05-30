using System;
using UnityEngine;
using Assets.SceneSimulation;
using Assets.SceneEditor.Models;


public abstract class ViewModuleScriptableObject : ScriptableObject
{
    public abstract IViewModuleData CreateModuleData();
}

