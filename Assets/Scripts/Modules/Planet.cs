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
}
