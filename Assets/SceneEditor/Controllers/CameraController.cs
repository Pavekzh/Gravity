using System;
using UnityEngine;
using Assets.SceneEditor.Models;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class CameraController:MonoBehaviour
    {
        private new Camera camera;
        private CameraStartupData startupData;

        private CameraModel model;
        private SceneInstance sceneInstance;

        public CameraModel Model 
        {
            get => model;
            set
            {
                if(value != null)
                {
                    model = value;
                    model.Camera = camera;
                    model.StartupData = startupData;
                    model.UpdateCamera();
                }

            }
        }

        [Zenject.Inject]
        private void Construct(Services.SceneInstance sceneInstance,CameraModel cameraModel)
        {
            model = cameraModel;
            camera = cameraModel.Camera;
            startupData = cameraModel.StartupData;
            this.sceneInstance = sceneInstance;  
            
            if (model.IsSceneCamera)
                sceneInstance.SceneSet += OnSceneChanged;
        }


        private void OnSceneChanged(SceneState state,bool isChangedLocal)
        {
            CameraModel model = state.Camera;
            if (model != null)
                this.Model = model;
            else
                state.Camera = Model;
        }

        public void ResetRotation()
        {
            Model.ResetRotation();
        }

        public void ResetTransform()
        {
            Model.ResetTransform();
        }
    }
}
