using BasicTools;
using UnityEngine;
using Assets.SceneEditor.Models;

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
        protected Vector2 joystickInput;

        protected PlanetController SelectedObject
        {
            get
            {
                if (selectedObject != null) return selectedObject;
                else
                {
                    SelectedObject = Services.PlanetSelectSystem.Instance.SelectedPlanet;
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

        protected override void Awake()
        {
            base.Awake();
            Services.PlanetSelectSystem.Instance.SelectedPlanetChanged += selectedObjectChanged;
        }

        protected void FixedUpdate()
        {
            if (IsToolEnabled && joystickInput != Vector2.zero)
            {
                selectedGravity.PositionProperty.Binding.ChangeValue(selectedGravity.Data.Position + (joystickInput * moveSpeed), this);
            }
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

                Services.PlanetSelectSystem.Instance.UnlockSelection();
                EditorController.Instance.ToolsController.EnableSceneControl();
            }else
            {
                joystickSystem.InputBinding.ValueChanged -= JoystickInput;
                joystickSystem.JoystickInputReadingEnded -= JoystickUp;
            }


            joystickSystem.DisableManipulator();
            joystickSystem.TouchInputReadingStarted -= TouchInputEnabled;
            joystickSystem.TouchInputReadingEnded -= TouchInputDisabled;


            base.DoDisable();
        }

        protected override void DoEnable(InputSystem inputSystem)
        {
            joystickSystem = EditorController.Instance.ManipulatorsController.EnableManipulator<PositioningJoystickSystem>(PositioningJoystickSystem.DefaultKey);
            joystickSystem.InputBinding.ValueChanged += JoystickInput;
            joystickSystem.JoystickInputReadingEnded += JoystickUp;
            joystickSystem.TouchInputReadingStarted += TouchInputEnabled;
            joystickSystem.TouchInputReadingEnded += TouchInputDisabled;
            base.DoEnable(inputSystem);
        }

        private void JoystickUp()
        {
            joystickInput = Vector2.zero;
        }

        private void TouchInputDisabled()
        {
            joystickSystem.InputBinding.ValueChanged += JoystickInput;            
            joystickSystem.JoystickInputReadingEnded += JoystickUp;
  
            joystickSystem.InputBinding.ValueChanged -= TouchInput;
            touchReading = false;

            Services.PlanetSelectSystem.Instance.UnlockSelection();
            EditorController.Instance.ToolsController.EnableSceneControl();
        }

        private void TouchInputEnabled()
        {
            joystickSystem.InputBinding.ValueChanged -= JoystickInput;
            joystickSystem.JoystickInputReadingEnded -= JoystickUp;

            joystickSystem.InputBinding.ValueChanged += TouchInput;
            touchReading = true;

            Services.PlanetSelectSystem.Instance.LockSelection();
            EditorController.Instance.ToolsController.DisableSceneControl();
        }

        private void TouchInput(Vector3 value, object source)
        {
            if (SelectedObject != null)
                selectedGravity.PositionProperty.Binding.ChangeValue(value.GetVectorXZ(),this);
        }

        private void JoystickInput(Vector3 value, object source)
        {
            if(selectedObject != null)
                joystickInput = -value.GetVectorXZ();
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