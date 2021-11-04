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

        private InputSystem inputSystem;
        private PlanetController selectedPlanet { get => EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet; }
        private GravityModuleData selectedGravityInteractor { get => EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key); }

        public override string DefaultKey { get => "PlanetKickTool"; }

        protected override void Awake()
        {
            base.Awake();
            this.inputManipulator.OnManipulatorActivates += ManipulatorActivatesHandler;
            EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanetChanged += SelectedPlanetChanged;
        }

        private void SelectedPlanetChanged(object sender, PlanetController planet)
        {
            if (isSelectedAndWorking)
            {
                if(planet != null)
                {                        
                    inputManipulator.Origin = selectedPlanet.transform;
                    if (inputManipulator.IsVisible == false)
                    {
                        inputManipulator.IsVisible = true;
                    }
                    if (outputManipulator.IsVisible == false)
                    {
                        outputManipulator.IsVisible = true;
                    }
                }
                else
                {
                    inputManipulator.IsVisible = false;
                    outputManipulator.IsVisible = false;
                }
            }
        }

        public override void DisableTool()
        {
            if(inputSystem != null)
            {
                inputSystem.OnTouchDown -= SingleTouch;
                inputSystem.OnTouchContinues -= SingleTouch;
                inputSystem.OnTouchRelease -= TouchRelease;            

                if(Services.TimeManager.Instance != null)
                    Services.TimeManager.Instance.ResumePhysics();
            }
            inputManipulator.IsVisible = false;
            outputManipulator.IsVisible = false;
            isSelectedAndWorking = false;
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            if(selectedPlanet != null)
            {
                inputManipulator.Origin = selectedPlanet.transform;            
                inputManipulator.IsVisible = true;
                outputManipulator.IsVisible = true;
            }
            Services.TimeManager.Instance.StopPhysics();
            this.inputSystem = inputSystem;

            inputSystem.OnTouchDown += SingleTouch;
            inputSystem.OnTouchContinues += SingleTouch;
            inputSystem.OnTouchRelease += TouchRelease;

            isSelectedAndWorking = true;
        }

        private void FixedUpdate()
        {
            if(isSelectedAndWorking && selectedPlanet != null)
            {
                GravityModuleData gravityObject = selectedGravityInteractor;
                inputManipulator.UpdateView(gravityObject.Position.GetVector3(), (gravityObject.Position.GetVector3() + ComputeInputVector(gravityObject.Velocity.GetVector3())),EditorController.Instance.Camera.ScaleFactor);
                outputManipulator.UpdateManipulatorView(gravityObject.Position.GetVector3(), gravityObject.Velocity.GetVector3(),EditorController.Instance.Camera.ScaleFactor);
            }
        }

        private void SingleTouch(Touch touch)
        {
            if(selectedPlanet != null)
            {
                GravityModuleData gravityObject = selectedGravityInteractor;
                Vector3 inputVector = inputManipulator.GetInputByTouch(touch, EditorController.Instance.Camera.ScaleFactor);
                if (inputManipulator.IsManipulatorActive)
                {
                    Vector3 outputVector = ComputeOutputVector(inputVector);

                    outputManipulator.UpdateManipulatorView(gravityObject.Position.GetVector3(), outputVector, EditorController.Instance.Camera.ScaleFactor);
                    gravityObject.Velocity = outputVector.GetVectorXZ();
                }
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