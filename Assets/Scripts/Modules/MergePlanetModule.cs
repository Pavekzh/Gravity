using UnityEngine;
using System.Collections;
using Assets.Library;
public class MergePlanetModule : Module
{
    [SerializeField]GenericModulePresenter modulePresenter;

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
        Planet.Presenters.Add(modulePresenter);
        base.Awake();
    }
}
