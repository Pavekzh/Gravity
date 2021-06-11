using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;


public class GravityManager : Singleton<GravityManager>
{
    public float GravityRatio;

    public GameObject PlanetsObject;
    public List<IGravityObject> Objects = new List<IGravityObject>();

    void Start()
    {
        if (PlanetsObject == null)
            ErrorManager.Instance.ShowErrorMessage("There is no Planets Storage-Object in scene");
    }

}

