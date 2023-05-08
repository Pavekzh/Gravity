using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Services
{
    public class SandboxInstaller : MonoInstaller
    {
        [SerializeField] SceneEditor.Models.CameraStartupData cameraData;
        [SerializeField] GravityComputer gravityComputer;
        [SerializeField] SceneStateLoader sceneLoader;
        [SerializeField] TimeFlow timeFlow;
        [SerializeField] PlanetSelector planetSelector;
        [SerializeField] SceneEditor.Controllers.EditorController editorController;
        [SerializeField] ValuesPanelConfig valuesPanelConfig;
        [SerializeField] Transform SceneParent;
        [SerializeField] GameObject CommonPlanetPrefab;


        public override void InstallBindings()
        {
            InstallCamera();
            InstallSceneLoader();
            InstallTimeFlow();
            InstallPlanetSelector();
            InstallSceneInstantiation();
            InstallGravity();
            InstallEditor();
            InstallPlanetsArrangementTools();
            InstallValuesPanel();
        }

 
        private void InstallCamera()
        {
            SceneEditor.Models.CameraModel cameraModel = new SceneEditor.Models.CameraModel(cameraData);
            Container.Bind<SceneEditor.Models.CameraModel>().FromInstance(cameraModel).AsSingle();
        }

        private void InstallSceneLoader()
        {
            Container.Bind<SceneStateLoader>().FromInstance(sceneLoader).AsSingle();
        }

        private void InstallTimeFlow()
        {
            timeFlow.Initialize();
            Container.Bind<TimeFlow>().FromInstance(timeFlow).AsSingle();
        }

        private void InstallPlanetSelector()
        {
            Container.Bind<PlanetSelector>().FromInstance(planetSelector).AsSingle();
        }

        private void InstallSceneInstantiation()
        {
            Container.Bind<SceneInstance>().FromNew().AsSingle().WithArguments(SceneParent);
            Container.Bind<IModuleFactory>().To<CommonModuleFactory>().FromNew().AsTransient();
            Container.Bind<IPlanetFactory>().To<CommonPlanetFactory>().FromNew().AsTransient().WithArguments(CommonPlanetPrefab);
        } 
        
        private void InstallGravity()
        {
            Container.Bind<GravityComputer>().FromInstance(gravityComputer).AsSingle();
        }

        private void InstallEditor()
        {
            Container.Bind<SceneEditor.Controllers.EditorController>().FromInstance(editorController).AsSingle();
        }
        
        private void InstallPlanetsArrangementTools()
        {
            ColorInterval[] intervals = new ColorInterval[5]
            {
                new ColorInterval(ColoredSpheresPlanetsArrangement.defaultColors[0],10),
                new ColorInterval(ColoredSpheresPlanetsArrangement.defaultColors[1],100),
                new ColorInterval(ColoredSpheresPlanetsArrangement.defaultColors[2],1000),
                new ColorInterval(ColoredSpheresPlanetsArrangement.defaultColors[3],10000),
                new ColorInterval(ColoredSpheresPlanetsArrangement.defaultColors[4],100000)
            };
            Container.Bind<IPlanetsArrangementTool<float>>().To<ColoredSpheresPlanetsArrangement>().AsTransient().WithArguments(intervals);
        }

        private void InstallValuesPanel()
        {
            Container.Bind<ValuesPanelConfig>().FromInstance(valuesPanelConfig).AsSingle();
            Container.Bind<ModuleControllerFactory>().FromNew().AsTransient();
            Container.Bind<PropertyControllerFactory>().FromNew().AsTransient();
        }


    }
}