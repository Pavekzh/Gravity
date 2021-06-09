using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Resources.Library;


public class GravityManager : Singleton<GravityManager>
{
    public float GravityRatio;

    public GameObject PlanetsObject;
    public List<GravityObject> Objects = new List<GravityObject>();


    void Start()
    {
        if (PlanetsObject == null)
            ErrorManager.Instance.ShowErrorMessage("There is no Planets Storage-Object in scene");
    }

}

