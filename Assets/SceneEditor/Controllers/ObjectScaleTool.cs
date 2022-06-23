using Assets.SceneEditor.Models;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public class ObjectScaleTool : ObjectTool
    {
        [SerializeField] float scaleSpeed;

        public override string DefaultKey => "ScaleTool";
        public override string ToolName => "Set scale tool";

        protected ScaleManipulator joystick;
        protected PlanetController selectedObject;
        protected ViewModuleData selectedView;
        protected float joystickInput;
        protected bool dragging;

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
                    selectedView = selectedObject.PlanetData.GetModule<ViewModuleData>(ViewModuleData.Key);
                }
                else
                    selectedView = null;
            }

        }

        protected override void Awake()
        {
            base.Awake();
            Services.PlanetSelectSystem.Instance.SelectedPlanetChanged += selectedObjectChanged;
        }

        private void selectedObjectChanged(object sender, PlanetController planet)
        {
            SelectedObject = planet;
            if(IsToolEnabled)
            {
                if (SelectedObject != null)
                {
                    joystick.OriginBiding = selectedObject.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).PositionProperty.Binding;
                    joystick.InputBinding.ChangeValue(selectedView.ObjectScale, this);
                }
                else
                    joystick.OriginBiding = null;
            }
        }


        protected override void DoDisable()
        {            
            joystick.InputBinding.ValueChanged -= input;
            joystick.JoystickInputReadingStoped -= joystickInputStoped;
            joystick.DragInputStarted -= dragStarted;
            joystick.DragInputEnded -= dragEnded;
            joystick.DisableManipulator();
            base.DoDisable();
        }
        protected override void DoEnable(InputSystem inputSystem)
        {
            joystick = EditorController.Instance.ManipulatorsController.EnableManipulator<ScaleManipulator>(ScaleManipulator.DefaultKey);
            joystick.OriginBiding = selectedObject.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).PositionProperty.Binding;
            joystick.InputBinding.ChangeValue(selectedView.ObjectScale,this);
            joystick.DragInputStarted += dragStarted;
            joystick.DragInputEnded += dragEnded;
            joystick.InputBinding.ValueChanged += input;
            joystick.JoystickInputReadingStoped += joystickInputStoped;
            base.DoEnable(inputSystem);
        }

        private void dragEnded()
        {
            EditorController.Instance.ToolsController.EnableSceneControl();
            Services.PlanetSelectSystem.Instance.UnlockSelection();
            dragging = false;
        }

        private void dragStarted()
        {
            EditorController.Instance.ToolsController.DisableSceneControl();
            Services.PlanetSelectSystem.Instance.LockSelection();
            joystickInputStoped();
            dragging = true;
        }

        private void joystickInputStoped()
        {
            joystickInput = 0;
        }

        private void input(float value, object source)
        {

            if (IsToolEnabled && source != (System.Object)this && SelectedObject != null)
            {
                if (dragging)
                {
                    selectedView.ScaleBinding.ChangeValue(value,this);
                }
                else
                {
                    selectedView.ScaleBinding.ChangeValue(selectedView.ObjectScale + selectedView.ObjectScale * value * scaleSpeed, this);
                    joystick.InputBinding.ChangeValue(selectedView.ObjectScale, this);
                }

            }

        }
    }
}