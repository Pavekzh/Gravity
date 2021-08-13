using UnityEngine;
using System.Collections;
using Assets.Library;
public class MergePlanetModule : Module
{
    [SerializeField]GeneralModulePresenter modulePresenter;

    public override ModuleData GetModuleData()
    {
        throw new System.NotImplementedException();
    }

    public override void SetModule(ModuleData module)
    {
        throw new System.NotImplementedException();
    }

    void Awake()
    {
        Planet.ModulePresenters.Add(modulePresenter);
        base.Awake();
    }
}
