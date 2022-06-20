using System.Collections;
using UnityEngine;
using UIExtended;
using BasicTools;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class PlanetKickTool : ObjectTool
    {
        [SerializeField] private OutputManipulator outputManipulator;

        private VariableMagnitudeVectorJoystickSystem joystickSystem;
        private InputSystem inputSystem;
        private PlanetController selectedPlanet;
        private PlanetController SelectedPlanet 
        { 
            get
            {
                if (selectedPlanet == null)
                    return SelectedPlanet = Services.PlanetSelectSystem.Instance.SelectedPlanet;
                else
                    return selectedPlanet;
            }
            set
            {
                selectedPlanet = value;
                if (selectedPlanet != null)
                    selectedGravityInteractor = selectedPlanet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key);
                else
                    selectedGravityInteractor = null;
            }
        }
        private GravityModuleData selectedGravityInteractor;
        private Binding<Vector2> velocityBinding;

        public override string DefaultKey { get => "PlanetKickTool"; }

        public override string ToolName => "Velocity set tool";

        protected override void Awake()
        {
            base.Awake();
            Services.PlanetSelectSystem.Instance.SelectedPlanetChanged += SelectedPlanetChanged;
        }


        private void SelectedPlanetChanged(object sender, PlanetController planet)
        {
            if(planet != null)
            {
                if (this.IsToolEnabled)
                {
                    if (selectedPlanet == null)
                    {
                        joystickSystem.EnableManipulator(inputSystem);
                    }
                    SelectedPlanet = planet;

                    outputManipulator.IsVisible = true;
                    outputManipulator.UpdateManipulatorView(selectedGravityInteractor.Data.Position, selectedGravityInteractor.Data.Velocity, EditorController.Instance.Camera.ScaleFactor);
                }
                else
                    SelectedPlanet = planet;
            }
            else
            {
                SelectedPlanet = null;
                if (IsToolEnabled)
                {
                    outputManipulator.IsVisible = false;
                }
            }
        }

        protected override void DoDisable()
        {
            if(inputSystem != null)
            {
                joystickSystem.InputReadingStarted -= ManipulatorActivates;
                joystickSystem.InputReadingStoped -= ManipulatorDeactivates;
                joystickSystem.InputBinding.ValueChanged -= InputChanged;

            }
            joystickSystem.DisableManipulator();
            outputManipulator.IsVisible = false;
            IsToolEnabled = false;            
            base.DoDisable();
        }

        protected override void DoEnable(InputSystem inputSystem)
        {
            IsToolEnabled = true;
            this.joystickSystem = EditorController.Instance.ManipulatorsController.EnableManipulator<VariableMagnitudeVectorJoystickSystem>(VariableMagnitudeVectorJoystickSystem.DefaultKey);
            joystickSystem.InputReadingStarted += ManipulatorActivates;
            joystickSystem.InputReadingStoped += ManipulatorDeactivates;
            joystickSystem.InputBinding.ValueChanged += InputChanged;

            if (SelectedPlanet != null)
            {            
                outputManipulator.IsVisible = true;
                outputManipulator.UpdateManipulatorView(selectedGravityInteractor.Data.Position, selectedGravityInteractor.Data.Velocity, EditorController.Instance.Camera.ScaleFactor);
            }
            this.inputSystem = inputSystem;
            base.DoEnable(inputSystem);
        }

        private void InputChanged(Vector3 value, object source)
        {
            Vector3 outputVector = ComputeOutputVector(value);
            if(selectedGravityInteractor != null)
            {
                selectedGravityInteractor.VelocityProperty.Binding.ChangeValue(outputVector.GetVectorXZ(), this);
                outputManipulator.UpdateManipulatorView(selectedGravityInteractor.Position.GetVector3(), outputVector, EditorController.Instance.Camera.ScaleFactor);
            }
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