using System;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class CameraController:MonoBehaviour
    {
        [SerializeField] private new CameraModel camera;

        public void ResetRotation()
        {
            camera.ResetRotation();
        }

        public void ResetTransform()
        {
            camera.ResetTransform();
        }
    }
}
