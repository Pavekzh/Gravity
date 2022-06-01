﻿using System;
using UnityEngine;
using Assets.SceneEditor.Models;
using BasicTools;


[Serializable]
[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/PlanetViewSettings", order = 0)]
public class PlanetViewScriptableObject : ViewModuleScriptableObject
{
    [SerializeField]
    private float planetRadius = 1;
    [SerializeField]
    private NoiseSettings noiseSettings;
    [SerializeField]
    private Gradient landGradient;
    [SerializeField]
    private Gradient waterGradient;

    public float PlanetRadius { get => planetRadius; set => planetRadius = value; }
    public NoiseSettings NoiseSettings { get => noiseSettings; set => noiseSettings = value; }
    public Gradient LandGradient { get => landGradient; set => landGradient = value; }
    public Gradient WaterGradient { get => waterGradient; set => waterGradient = value; }

    public override IViewModuleData CreateModuleData()
    {
        PlanetViewModuleData modData = new PlanetViewModuleData();
        modData.PlanetRadius = PlanetRadius;
        modData.NoiseSettings = new NoiseSettings(NoiseSettings);
        modData.WaterGradient = WaterGradient;
        modData.LandGradient = LandGradient;

        return modData;
    }

    protected override void UpdateViewModule(IViewModuleData moduleData)
    {
        PlanetViewModuleData data = moduleData as PlanetViewModuleData;
        if(data == null)
        {
            throw new Exception("Invalid ModuleData type or ModuleData is null. ModuleData must be PlanetViewModuleData");
        }
        else
        {
            if (this.planetRadius != data.PlanetRadius) data.PlanetRadius = this.planetRadius;
            if (!this.noiseSettings.Equals(data.NoiseSettings)) data.NoiseSettings = new NoiseSettings(this.noiseSettings);
            if (!this.landGradient.Equals(data.LandGradient)) data.LandGradient = this.landGradient;
            if (!this.waterGradient.Equals(data.WaterGradient)) data.WaterGradient = this.waterGradient;
        }
    }
}


