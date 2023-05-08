using System;
using Assets.SceneEditor.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BasicTools;

namespace Assets.Services
{
    public class ColoredSpheresPlanetsArrangement:IPlanetsArrangementTool<float>
    {
        public static float MaxRadius { get; set; } = 5;
        public static float MinRadius { get; set; } = 1;
        public static Color[] defaultColors = new Color[]
        {
            new Color32(0x34,0x98,0xdb,0xff),            
            new Color32(0x7F,0xB7,0x7E,0xff),
            new Color32(0xEA,0x92,0x15,0xff),
            new Color32(0x87,0x23,0x41,0xff),
            new Color32(0x2F,0x1B,0x41,0xff)
        };

        private List<ColorInterval> intervals;
        private List<Renderer> spheres = new List<Renderer>();
        private Material colorMaterial;
        private SceneInstance sceneInstance;
        private IPlanetEstimator<float> lastEstimator;

        public bool IsShowing { get; private set; }

        public delegate float StepOperation(float startPosition);


        public ColoredSpheresPlanetsArrangement(IEnumerable<ColorInterval> intervals, SceneInstance sceneInstance)
        {                   
            this.intervals = intervals.ToList();
            colorMaterial = Resources.Load<Material>("Materials/BaseColor");
            SetSceneInstance(sceneInstance);

        }        
        
        public ColoredSpheresPlanetsArrangement(float startPosition,UInt32 intervalsCount, StepOperation op, SceneInstance sceneInstance)
        {
            SetIntervals(startPosition, intervalsCount, op);
            colorMaterial = Resources.Load<Material>("Materials/BaseColor");
            SetSceneInstance(sceneInstance);
        }


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


        private void SetSceneInstance(SceneInstance sceneInstance)
        {
            this.sceneInstance = sceneInstance;
            this.sceneInstance.SceneChanges += SceneChanged;
            this.sceneInstance.SceneSet += SceneChanged;
        }

        private void SceneChanged(SceneState scene,bool isValueLocal)
        {
            UpdateArrangement();
        }

        private void SceneChanged(SceneState scene)
        {
            UpdateArrangement();
        }


        public void HideArrangement()
        {
            if (IsShowing)
            {
                ClearSpheres();
                foreach (PlanetData planet in sceneInstance.CurrentScene.Planets)
                {
                    ViewModuleData viewModule = planet.GetModule<ViewModuleData>(ViewModuleData.Key);
                    if (viewModule != null)
                        viewModule.IsViewEnabled = true;
                }
                IsShowing = false;
            }
        }

        public void ShowArrangement(IPlanetEstimator<float> estimator)
        {
            IsShowing = true;
            lastEstimator = estimator;
            UpdateArrangement();
        }

        public void UpdateArrangement()
        {
            if (!IsShowing)
                return;

            if (intervals == null || !intervals.Any())
                throw new Exception("Intervals not set");


            if(sceneInstance.CurrentScene.Planets.Count > spheres.Count)
            {
                int delta = sceneInstance.CurrentScene.Planets.Count - spheres.Count;
                for(int s = 0; s < delta; s++)
                {
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    obj.name = "EstimatorSphere";
                    spheres.Add(obj.GetComponent<Renderer>());
                }
            }

            int i = 0;
            foreach(Renderer sphere in spheres)
            {
                if(i < sceneInstance.CurrentScene.Planets.Count)
                {
                    PlanetData planet = sceneInstance.CurrentScene.Planets[i];
                    float estimate = lastEstimator.Estimate(planet);
                    float scale = 1;

                    Color estimatedColor = intervals.Last().Color;

                    if (estimate <= intervals[0].Limit)
                    {
                        estimatedColor = intervals[0].Color;
                        scale = Mathf.InverseLerp(0, intervals[0].Limit, estimate);
                    }
                    else
                    {
                        for (int j = 1; j < intervals.Count; j++)
                        {
                            if (estimate <= intervals[j].Limit)
                            {
                                estimatedColor = intervals[j].Color;
                                scale = Mathf.InverseLerp(intervals[j - 1].Limit, intervals[j].Limit, estimate);
                                break;
                            }
                        }
                    }

                    scale = Mathf.Lerp(MinRadius, MaxRadius, scale);

                    ViewModuleData viewModule = planet.GetModule<ViewModuleData>(ViewModuleData.Key);
                    if (viewModule != null)
                        viewModule.IsViewEnabled = false;


                    sphere.gameObject.layer = LayerMask.NameToLayer("PlanetNonInteract");
                    sphere.material = colorMaterial;
                    sphere.material.color = estimatedColor;
                    sphere.transform.position = planet.GetModule<GravityModuleData>(GravityModuleData.Key).Position.GetVector3();
                    sphere.transform.localScale = new Vector3(scale, scale, scale);
                    sphere.gameObject.SetActive(true);
                }
                else
                {
                    sphere.gameObject.SetActive(false);
                }
                i++;
            }

            
        }

        private void ClearSpheres()
        {
            foreach(Renderer sphere in spheres)
            {
                sphere.gameObject.SetActive(false);
            }
        }
    }
}
