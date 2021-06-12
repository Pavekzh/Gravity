﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;


public class GravityManager : Singleton<GravityManager>
{
    public float GravityRatio;

    public GameObject PlanetsObject;
    private List<GravityModule> objects = new List<GravityModule>();
    void RefreshSettings()
    {
        objects = new List<GravityModule>();
    }
    void Start()
    {
        if (PlanetsObject == null)
            ErrorManager.Instance.ShowErrorMessage("There is no Planets Storage-Object in scene");
        SceneStateManager.Instance.OnSceneRefresh += RefreshSettings;
    }
    public List<GravityModule> Objects { get => objects; set => objects = value; }
}

