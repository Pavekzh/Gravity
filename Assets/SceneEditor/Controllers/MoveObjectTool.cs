using BasicTools;
using UnityEngine;
using Assets.SceneEditor.Models;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class MoveObjectTool : ObjectTool
    {
        [SerializeField] float moveSpeed = 1;

        public override string DefaultKey => "MoveTool";
        public override string ToolName => "Position set tool";

        protected PositioningJoystickSystem joystickSystem;
        protected bool touchReading = false;
        protected GravityModuleData selectedGravity;
        protected PlanetController selectedObject;

        protected PlanetController SelectedObject
        {
            get
            {
                if (selectedObject != null) return selectedObject;
                else
                {
                    SelectedObject = selector.SelectedPlanet;
                    return selectedObject;
                }
            }
            set
            {                    
                selectedObject = value;
                if (selectedObject != null)
                {
                    selectedGravity = selectedObject.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key);
                }
                else
                    selectedGravity = null;
            }

        }

        protected override void Construct(TimeFlow timeFlow, PlanetSelector selector, EditorController editor)
        {
            base.Construct(timeFlow, selector, editor);
            selector.SelectedPlanetChanged += selectedObjectChanged;
        }

        private void selectedObjectChanged(object sender, PlanetController planet)
        {
            SelectedObject = planet;
        }

        protected override void DoDisable()
        {
            if (touchReading)
            {
                joystickSystem.InputBinding.ValueChanged -= TouchInput;
                touchReading = false;

                selector.UnlockSelection();
                editor.ToolsController.EnableSceneControl();
            }else
            {
                joystickSystem.InputBinding.ValueChanged -= JoystickInput;
            }


            joystickSystem.DisableManipulator();
            joystickSystem.TouchInputReadingStarted -= TouchInputEnabled;
            joystickSystem.TouchInputReadingEnded -= TouchInputDisabled;

            base.DoDisable();
        }

        protected override void DoEnable(InputSystem inputSystem)
        {
            joystickSystem = editor.ManipulatorsController.EnableManipulator<PositioningJoystickSystem>(PositioningJoystickSystem.DefaultKey);
            joystickSystem.InputBinding.ValueChanged += JoystickInput;
            joystickSystem.TouchInputReadingStarted += TouchInputEnabled;
            joystickSystem.TouchInputReadingEnded += TouchInputDisabled;
            base.DoEnable(inputSystem);
        }

        private void TouchInputDisabled()
        {
            joystickSystem.InputBinding.ValueChanged += JoystickInput;            
  
            joystickSystem.InputBinding.ValueChanged -= TouchInput;
            touchReading = false;

            selector.UnlockSelection();
            editor.ToolsController.EnableSceneControl();
        }

        private void TouchInputEnabled()
        {
            joystickSystem.InputBinding.ValueChanged -= JoystickInput;

            joystickSystem.InputBinding.ValueChanged += TouchInput;
            touchReading = true;

            selector.LockSelection();
            editor.ToolsController.DisableSceneControl();
        }

        private void TouchInput(Vector3 value, object source)
        {
            if (SelectedObject != null)
                selectedGravity.PositionProperty.Binding.ChangeValue(value.GetVectorXZ(),this);
        }

        private void JoystickInput(Vector3 value, object source)
        {
            if(selectedObject != null)
                selectedGravity.PositionProperty.Binding.ChangeValue( selectedGravity.Data.Position - (value.GetVectorXZ() * moveSpeed), this);
        }


        public void EnableImmediatelyInputReading()
        {
            if (IsToolEnabled)
                joystickSystem.EnableTouchPositioning();
        }

        public void DisableToolUI()
        {
            joystickSystem.DisableUI();
        }
    }
}