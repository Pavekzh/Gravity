using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;


public class GravityManager : Singleton<GravityManager>
{
    [SerializeField]private float gravityRatio;
    [SerializeField] private GameObject planetsObject;

    public float GravityRatio { get => gravityRatio; }
    public GameObject PlanetsObject { get => planetsObject; }

    private List<GravityModule> objects = new List<GravityModule>();
    void RefreshSettings()
    {
        objects = new List<GravityModule>();
    }
    void Start()
    {
        if (PlanetsObject == null)
            ErrorManager.Instance.ShowErrorMessage("There is no Planets Storage-Object in scene",this);
        SceneStateManager.Instance.SceneRefreshed += RefreshSettings;
    }
    public List<GravityModule> Objects { get => objects; set => objects = value; }
}

