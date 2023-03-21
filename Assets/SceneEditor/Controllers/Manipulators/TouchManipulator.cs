using System;
using UnityEngine;
using BasicTools;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    class TouchManipulator : MonoBehaviour,IManipulator
    {
        [SerializeField] protected string manipulatorName;

        protected InputSystem inputSystem;

        public bool IsEnabled { get; private set; }
        public string ManipulatorKey => manipulatorName;
        public Binding<Vector3> InputBinding = new Binding<Vector3>();

        public event Action InputReadingStarted;
        public event Action InputReadingEnded;

        private Camera mainCamera;

        protected void Start()
        {
            mainCamera = Camera.main;
            EditorController.Instance.ManipulatorsController.Manipulators.Add(ManipulatorKey, this);
        }

        public void DisableManipulator()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
                this.inputSystem.OnTouchContinues -= Touch;
                this.inputSystem.OnTouchDown -= OnTouchStarted;
                this.inputSystem.OnTouchRelease -= OnTouchEnded;
                this.inputSystem.OnUITouch -= OnTouchEnded;
                this.inputSystem = null;
            }
        }

        public void EnableManipulator(InputSystem inputSystem)
        {
            if (!IsEnabled)
            {
                this.inputSystem = inputSystem;
                inputSystem.OnTouchContinues += Touch;
                inputSystem.OnTouchDown += OnTouchStarted;
                inputSystem.OnTouchRelease += OnTouchEnded;
                inputSystem.OnUITouch += OnTouchEnded;
                IsEnabled = true;
            }
        }

        private void Touch(Touch touch)
        {
            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                InputBinding.ChangeValue(ray.GetPoint(distance), this);
            }

        }

        private void OnTouchEnded()
        {
            if(!inputSystem.IsInputEnabled)
                InputReadingEnded?.Invoke();
        }

        private void OnTouchEnded(Touch touch)
        {
            InputReadingEnded?.Invoke();
        }

        private void OnTouchStarted(Touch touch)
        {
            InputReadingStarted?.Invoke();
        }
    }
}
