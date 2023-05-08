using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;
using Assets.Services;
using TMPro;

namespace Assets.SceneEditor.Controllers
{
    public class PlanetController:MonoBehaviour
    {
        public PlanetData PlanetData { get; private set; }
        private RectTransform planetView;
        private ModuleController[] modules;
        private SceneInstance sceneInstance;
        private PlanetSelector selector;
        private ValuesPanelConfig config;
        private Zenject.IInstantiator instantiator;
        private ModuleControllerFactory moduleControllerFactory;

        [Zenject.Inject]
        private void Construct(SceneInstance sceneInstance,PlanetSelector selector,ValuesPanelConfig config,Zenject.IInstantiator instantiator,ModuleControllerFactory moduleControllerFactory)
        {
            this.sceneInstance = sceneInstance;
            this.selector = selector;
            this.config = config;
            this.instantiator = instantiator;
            this.moduleControllerFactory = moduleControllerFactory;
            
        }

        private void OnDestroy()
        {
            selector.RemovePlanet(this.PlanetData.Guid);
            sceneInstance.RemovePlanet(PlanetData);
            CloseView();
        }

        public void SetPlanetData(PlanetData data)
        {
            this.PlanetData = data;
            this.IntitializeModules();
        }

        private void IntitializeModules()
        {
            int modulesCount = 0;
            foreach(var module in PlanetData.Modules)
            {
                if (module.Value.DisplayOnValuesPanel)
                    modulesCount++;
            }

            modules = new ModuleController[modulesCount];
        
            foreach(var module in PlanetData.Modules)
            {
                AddModule(module.Value);
            }
        }

        private void AddModule(ModuleData moduleData)
        {
            if(moduleData.DisplayOnValuesPanel)
                modules[moduleData.DisplayIndex] = moduleControllerFactory.Create(moduleData);
        }


        public void OpenView(RectTransform setValuesPanel)
        {
            CloseView();
            planetView = instantiator.InstantiatePrefabForComponent<RectTransform>(config.EmptyPrefab, setValuesPanel);
            planetView.name = PlanetData.Name + "View";
            planetView.anchorMin = new Vector2(0, 0);
            planetView.anchorMax = new Vector2(1, 1);
            planetView.pivot = new Vector2(0.5f, 0.5f);
            TMP_InputField nameField = instantiator.InstantiatePrefabForComponent<TMP_InputField>(config.PlanetNameFieldPrefab, planetView);
            nameField.text = PlanetData.Name;
            UnityEngine.Events.UnityAction<string> nameChangedAction = new UnityEngine.Events.UnityAction<string>(NameChanged);
            nameField.onValueChanged.AddListener(nameChangedAction);

            float offset = config.StartMargin + nameField.GetComponent<RectTransform>().rect.height;
            foreach(ModuleController controller in modules)
            {
                controller.CreateView(planetView,ref offset);
            }
        }

        private void NameChanged(string name)
        {
            this.PlanetData.Name = name;
        }

        public void CloseView()
        {
            if(planetView != null)
            {
                GameObject.Destroy(planetView.gameObject);
            }
        }

        public void DeletePlanet()
        {           
            GameObject.Destroy(this.gameObject);
        }
    }
}