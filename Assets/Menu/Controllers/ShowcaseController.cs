﻿using Assets.Services;
using Assets.SceneEditor.Models;
using UnityEngine;
using System;
using System.IO;


namespace Assets.Menu.Controllers
{
    public class ShowcaseController : MonoBehaviour
    {        
        [SerializeField] TMPro.TMP_Text planetLabel;
        [SerializeField] FileNamesCollectionScriptableObject presetsFileNames;
        [SerializeField] string presetDirectory = "Presets/Planets/";
        [SerializeField] string userPlanetsDirectory = "Planets/";
        [SerializeField] SaveSystemFactory saveSystemFactory;
        [Header("Planet")]
        [SerializeField] float scale = 1;
        [SerializeField] float pointerRotationSpeed = 1;
        [SerializeField] Vector3 axisOfRotation = Vector3.up;
        [SerializeField] float angularSpeed = 1;
        [SerializeField] float smoothness = 0.3f;

        public ISaveSystem SaveSystem { get => saveSystemFactory.GetChachedSaveSystem(); }

        protected string UserPlanetsDirectory { get => SceneStateManager.BaseDirectory + userPlanetsDirectory; }

        protected int lastShowedPlanet = -1;
        protected GameObject planet;

        protected void Start()
        {
            if (!System.IO.Directory.Exists(UserPlanetsDirectory))
            {
                System.IO.Directory.CreateDirectory(UserPlanetsDirectory);
            }
            ShowRandomPlanet(); 
        }

        public void ShowRandomPlanet()
        {
            if (planet != null)
                Destroy(planet);

            string[] presetsNames = presetsFileNames.Collection.ToArray();
            string[] userNames = System.IO.Directory.GetFiles(UserPlanetsDirectory, "*" + SaveSystem.Extension);
            System.Random rand = new System.Random();
            int index;
            do
            {
                index = rand.Next(0, (presetsNames.Length + userNames.Length) - 1);
            }
            while (index == lastShowedPlanet);

            lastShowedPlanet = index;

            if (index < presetsNames.Length)
            {
                try
                {
                    TextAsset textFile = Resources.Load<TextAsset>(presetDirectory + presetsNames[index]);
                    using (Stream stream = new MemoryStream(textFile.bytes))
                    {
                        PlanetData pData = (PlanetData)SaveSystem.Load(stream, typeof(PlanetData));
                        BuildPlanet(pData);
                    }
                }
                catch (Exception ex)
                {
                    BasicTools.MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
                }

            }
            else
            {
                index = index - presetsNames.Length;
                PlanetData pData = SaveSystem.Load(userNames[index], typeof(PlanetData)) as PlanetData;
                if (pData != null)
                {
                    BuildPlanet(pData);
                }
                else
                    CommonMessagingSystem.Instance.ShowErrorMessage("PlanetData didn't load properly", this);
            }
        }

        protected void BuildPlanet(PlanetData pData)
        {       
            pData.GetModule<PlanetViewModuleData>(PlanetViewModuleData.Key).Scale = scale;
            planet = pData.CreateSceneObject().gameObject;
            planet.transform.position = Vector3.zero;
            planetLabel.text = pData.Name;

            ShowcaseRotation rotation =  planet.AddComponent<ShowcaseRotation>();
            rotation.AngularSpeed = this.angularSpeed;
            rotation.Axis = this.axisOfRotation;
            rotation.PointerRotationSpeed = this.pointerRotationSpeed;
            rotation.Smoothness = this.smoothness;

        }
    }
}