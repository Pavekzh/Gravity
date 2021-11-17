using System.Collections;
using UnityEngine;
using UIExtended;
using BasicTools;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class PlanetKickTool : ObjectTool
    {
        [SerializeField] private InputManipulator inputManipulator;
        [SerializeField] private OutputManipulator outputManipulator;

        private Binding<Vector3> inputBinding;
        private InputSystem inputSystem;
        private PlanetController selectedPlanet { get => EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet; }
        private GravityModuleData selectedGravityInteractor { get => EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key); }

        public override string DefaultKey { get => "PlanetKickTool"; }

        protected override void Awake()
        {
            base.Awake();
            EditorController.Instance.Camera.ZoomChanged += SceneZoomChanged;
            inputManipulator.ScaleFactor = EditorController.Instance.Camera.ScaleFactor;
            EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanetChanged += SelectedPlanetChanged;
        }

        private void SceneZoomChanged(float value, object sender)
        {
            inputManipulator.ScaleFactor = (sender as CameraModel).ScaleFactor;
        }

        private void SelectedPlanetChanged(object sender, PlanetController planet)
        {               
            if(planet != null)
            {
                GravityModuleData gravityModuleData = planet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key);
                gravityModuleData.VelocityProperty.Binding.ValueChanged += VelocityChanged;
                if (this.ToolSelectedAndWorking)
                {
                    inputManipulator.EnableTool(planet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).PositionProperty.Binding);
                    gravityModuleData.VelocityProperty.Binding.ForceUpdate();
                }
            }
        }

        private void VelocityChanged(Vector2 value, object source)
        {
            if(ToolSelectedAndWorking && source != (object)this)
                inputBinding.ChangeValue(ComputeInputVector(value.GetVector3()),source);
        }

        public override void DisableTool()
        {
            if(inputSystem != null)
            {
                inputManipulator.InputReadingStarted -= ManipulatorActivates;
                inputManipulator.InputReadingStoped -= ManipulatorDeactivates;
                inputManipulator.InputBinding.ValueChanged -= InputChanged;

                if (Services.TimeManager.Instance != null)
                    Services.TimeManager.Instance.ResumePhysics();
            }
            inputManipulator.DisableTool();
            inputBinding = null;
            outputManipulator.IsVisible = false;
            ToolSelectedAndWorking = false;
        }

        public override void EnableTool(InputSystem inputSystem)
        {            
            ToolSelectedAndWorking = true;
            this.inputBinding = inputManipulator.InputBinding;
            if(selectedPlanet != null)
            {            
                inputManipulator.EnableTool(selectedGravityInteractor.PositionProperty.Binding);
                inputManipulator.InputReadingStarted += ManipulatorActivates;
                inputManipulator.InputReadingStoped += ManipulatorDeactivates;
                inputManipulator.InputBinding.ValueChanged += InputChanged;

                outputManipulator.IsVisible = true;
                selectedGravityInteractor.VelocityProperty.Binding.ForceUpdate();
            }
            Services.TimeManager.Instance.StopPhysics();
            this.inputSystem = inputSystem;

        }

        private void InputChanged(Vector3 value, object source)
        {
            Vector3 outputVector = ComputeOutputVector(value);
            selectedGravityInteractor.VelocityProperty.Binding.ChangeValue(outputVector.GetVectorXZ(),this);
            outputManipulator.UpdateManipulatorView(selectedGravityInteractor.Position.GetVector3(), outputVector, EditorController.Instance.Camera.ScaleFactor);
        }



        public Vector3 ComputeOutputVector(Vector3 inputVector)
        {
            return inputVector;
        }

        public Vector3 ComputeInputVector(Vector3 outputVector)
        {
            return outputVector;
        }

        private void ManipulatorActivates()
        {
            EditorController.Instance.ToolsController.DisableSceneControl();
        }

        private void ManipulatorDeactivates()
        {
            EditorController.Instance.ToolsController.EnableSceneControl();
        }
    }

}