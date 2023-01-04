using System;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class CameraController:MonoBehaviour
    {
        [SerializeField] private CameraStartupData startupData;
        [SerializeField] private new Camera camera;
        [SerializeField] private bool isSceneCamera;

        private CameraModel model;

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

        private void Awake()
        {
            model = new CameraModel(camera, startupData);            
            if (isSceneCamera)
                Services.SceneStateManager.Instance.SceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged()
        {
            CameraModel model = Services.SceneStateManager.Instance.CurrentScene.Camera;
            if (model != null)
                this.Model = model;
            else
                Services.SceneStateManager.Instance.CurrentScene.Camera = Model;
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
