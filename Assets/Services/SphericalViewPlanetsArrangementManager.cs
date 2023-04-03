using System;
using Assets.SceneEditor.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BasicTools;

namespace Assets.Services
{
    public class SphericalViewPlanetsArrangementManager
    {
        public static float MaxRadius { get; set; } = 5;
        public static float MinRadius { get; set; } = 1;

        Color[] defaultColors = new Color[]
        {
            new Color32(0x34,0x98,0xdb,0xff),            
            new Color32(0x7F,0xB7,0x7E,0xff),
            new Color32(0xEA,0x92,0x15,0xff),
            new Color32(0x87,0x23,0x41,0xff),
            new Color32(0x2F,0x1B,0x41,0xff)
        };
        List<ColorInterval> intervals;
        IPlanetEstimator<float> estimator;
        List<GameObject> createdSpheres;
        Material colorMaterial;

        public bool IsShowing;

        public SphericalViewPlanetsArrangementManager(IPlanetEstimator<float> estimator, List<ColorInterval> intervals)
        {
            this.estimator = estimator;
            this.intervals = intervals;
            colorMaterial = Resources.Load<Material>("Materials/MassEstimate");
        }        
        
        public SphericalViewPlanetsArrangementManager(IPlanetEstimator<float> estimator, float startPosition,UInt32 intervalsCount, StepOperation op)
        {
            this.estimator = estimator;
            SetIntervals(startPosition, intervalsCount, op);
            colorMaterial = Resources.Load<Material>("Materials/MassEstimate");
        }

        public delegate float StepOperation(float startPosition);


        public void SetIntervals(List<ColorInterval> intervals)
        {
            this.intervals = intervals;
        }

        public void SetIntervals(float startPosition, UInt32 intervalsCount, StepOperation op)
        {
            this.intervals = new List<ColorInterval>();
            for(int i = 0; i< intervalsCount; i++)
            {
                startPosition = op(startPosition);

                if (defaultColors.Length > i)
                    intervals.Add(new ColorInterval(defaultColors[i], startPosition));
                else
                    intervals.Add(new ColorInterval(defaultColors[defaultColors.Length - 1],startPosition));
            }
        }


        public void ClearArrangement()
        {
            if (IsShowing)
            {
                foreach (GameObject sphere in createdSpheres)
                {
                    GameObject.Destroy(sphere);
                }
                foreach (PlanetData planet in SceneStateManager.Instance.CurrentScene.Planets)
                {
                    ViewModuleData viewModule = planet.GetModule<ViewModuleData>(ViewModuleData.Key);
                    if (viewModule != null)
                        viewModule.EnableView();
                }
                createdSpheres = null;
                IsShowing = false;
            }
        }

        public void ShowArrangement()
        {
            if (intervals == null || !intervals.Any())
                throw new Exception("Intervals not set");

            if (!IsShowing || createdSpheres == null)
                createdSpheres = new List<GameObject>();


            for (int p = 0; p < SceneStateManager.Instance.CurrentScene.Planets.Count; p++)
            {
                PlanetData planet = SceneStateManager.Instance.CurrentScene.Planets[p];
                float estimate = estimator.Estimate(planet);
                float scale = 1;

                Color estimatedColor = intervals.Last().Color;

                if(estimate <= intervals[0].Limit)
                {
                    estimatedColor = intervals[0].Color;
                    scale = Mathf.InverseLerp(0, intervals[0].Limit, estimate);
                }
                else
                {
                    for (int i = 1; i < intervals.Count; i++)
                    {
                        if (estimate <= intervals[i].Limit)
                        {
                            estimatedColor = intervals[i].Color;
                            scale = Mathf.InverseLerp(intervals[i - 1].Limit, intervals[i].Limit, estimate);
                            break;
                        }
                    }
                }

                scale = Mathf.Lerp(MinRadius, MaxRadius, scale);

                ViewModuleData viewModule = planet.GetModule<ViewModuleData>(ViewModuleData.Key);
                if(viewModule != null)
                    viewModule.DisableView();


                GameObject sphere;
                if (createdSpheres.Count > p) sphere = createdSpheres[p];
                else
                {                 
                    sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    createdSpheres.Add(sphere);
                }

                sphere.layer = LayerMask.NameToLayer("PlanetNonInteract");
                sphere.GetComponent<Renderer>().material = colorMaterial;
                sphere.GetComponent<Renderer>().material.color = estimatedColor;
                sphere.transform.position = planet.GetModule<GravityModuleData>(GravityModuleData.Key).Position.GetVector3();
                sphere.transform.localScale = new Vector3(scale, scale, scale);
            }                
            IsShowing = true;
        }
    }
}
