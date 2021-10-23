using System.Collections;
using UnityEngine;
using UIExtended;
using BasicTools;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class PlanetKickTool : ObjectTool
    {
        [SerializeField] private UIExtended.InputManipulator inputManipulator;
        [SerializeField] private UIExtended.OutputManipulator outputManipulator;


        private bool isWorking;
        private InputSystem inputSystem;
        private GravityModuleData selectedPlanet;

        public override string DefaultKey { get => "PlanetKickTool"; }

        protected override void Awake()
        {
            base.Awake();
            this.toolsController.ObjectSelectionTool.SelectedPlanetChanged += SelectedPlanetChanged;
            this.inputManipulator.OnManipulatorActivates += ManipulatorActivatesHandler;
        }

        private void SelectedPlanetChanged(PlanetController planet, object sender)
        {
            selectedPlanet = planet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key);
            inputManipulator.Origin = EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet.transform;
        }

        public override void DisableTool()
        {
            if(inputSystem != null)
            {
                inputSystem.OnTouchDown -= SingleTouch;
                inputSystem.OnTouchContinues -= SingleTouch;
                inputSystem.OnTouchRelease -= TouchRelease;
            }
            inputManipulator.IsVisible = false;
            outputManipulator.IsVisible = false;
            isWorking = false;

            if(Services.TimeManager.Instance != null)
                Services.TimeManager.Instance.ResumePhysics();
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            Services.TimeManager.Instance.StopPhysics();

            this.inputSystem = inputSystem;
            inputManipulator.Origin = EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet.transform;

            inputSystem.OnTouchDown += SingleTouch;
            inputSystem.OnTouchContinues += SingleTouch;
            inputSystem.OnTouchRelease += TouchRelease;
            
            inputManipulator.IsVisible = true;
            outputManipulator.IsVisible = true;

            isWorking = true;
        }

        private void FixedUpdate()
        {
            if(isWorking)
            {
                inputManipulator.UpdateView(selectedPlanet.Position.GetVector3(), (selectedPlanet.Position.GetVector3() + ComputeInputVector(selectedPlanet.Velocity.GetVector3())),EditorController.Instance.Camera.ScaleFactor);
                outputManipulator.UpdateManipulatorView(selectedPlanet.Position.GetVector3(), selectedPlanet.Velocity.GetVector3(),EditorController.Instance.Camera.ScaleFactor);
            }
        }

        private void SingleTouch(Touch touch)
        {
            Vector3 inputVector = inputManipulator.GetInputByTouch(touch, EditorController.Instance.Camera.ScaleFactor);
            if (inputManipulator.IsManipulatorActive)
            {
                Vector3 outputVector = ComputeOutputVector(inputVector);

                outputManipulator.UpdateManipulatorView(selectedPlanet.Position.GetVector3(), outputVector, EditorController.Instance.Camera.ScaleFactor);
                selectedPlanet.Velocity = outputVector.GetVectorXZ();
            }
        }

        public Vector3 ComputeOutputVector(Vector3 inputVector)
        {
            return inputVector;
        }

        public Vector3 ComputeInputVector(Vector3 outputVector)
        {
            return -outputVector;
        }

        private void ManipulatorActivatesHandler(object sender, System.EventArgs e)
        {
            EditorController.Instance.ToolsController.DisableSceneControl();
        }

        private void TouchRelease(Touch touch)
        {
            EditorController.Instance.ToolsController.EnableSceneControl();
            inputManipulator.Deactivate(touch);
        }
    }

}