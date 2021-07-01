using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;
using System;

[Serializable]
public class Planet : MonoBehaviour
{
    [SerializeField]private List<Module> modules;
    public List<Module> Modules { get => modules; set => modules = value; }
    public string Name { get => gameObject.name; set => gameObject.name = value; }

    private void Awake()
    {
        modules = new List<Module>();
        SceneStateManager.Instance.Planets.Add(this);
    }
    public PlanetData GetPlanetData()
    {
        List<ModuleData> modulesData = new List<ModuleData>();
        foreach(Module module in modules)
        {
            modulesData.Add(module.GetModuleData());
        }
        PlanetData data = new PlanetData(modulesData,gameObject.name);
        return data;
    }
    public void DisablePhysicsModules()
    {
        foreach(Module module in Modules)
        {
            module.UpdatePhysicsState(false);
        }
    }
    public void EnablePhysicsModules()
    {
        foreach (Module module in Modules)
        {
            module.UpdatePhysicsState(true);
        }
    }
}
