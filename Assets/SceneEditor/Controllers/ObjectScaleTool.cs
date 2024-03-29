﻿using Assets.SceneEditor.Models;
using BasicTools;
using Assets.Services;
using BasicTools.Validation;

namespace Assets.SceneEditor.Controllers
{
    public class ObjectScaleTool : ObjectTool
    {
        public override string DefaultKey => "ScaleTool";
        public override string ToolName => "Set scale tool";

        protected ScalarJoystickSystem joystick;
        protected PlanetController selectedObject;
        protected bool dragging;
        
        protected virtual IValidationRule<float>[] ScaleValidationRule 
        { 
            get => ViewModuleData.ScaleValidationRules;
        }

        protected virtual Binding<float> ScalePropertyBinding
        {
            get
            {
                if (SelectedObject != null)
                    return SelectedObject.PlanetData.GetModule<ViewModuleData>(ViewModuleData.Key).ScaleBinding;
                else return null;
            }
        }

        protected virtual float ScalePropertyInitialValue
        {
            get
            {
                return SelectedObject.PlanetData.GetModule<ViewModuleData>(ViewModuleData.Key).Scale;
            }
            
        }

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
            }

        }

        protected override void Construct(TimeFlow timeFlow, PlanetSelector selector, EditorController editor)
        {
            base.Construct(timeFlow, selector, editor);
            selector.SelectedPlanetChanged += selectedObjectChanged;
        }

        protected virtual void selectedObjectChanged(object sender, PlanetController planet)
        {
            if (IsToolEnabled)
            {
                if(ScalePropertyBinding != null)
                    ScalePropertyBinding.ValueChanged -= ExternalValueChanged;
            }

            SelectedObject = planet;

            if(IsToolEnabled)
            {
                if(ScalePropertyBinding != null)
                    ScalePropertyBinding.ValueChanged += ExternalValueChanged;

                if (SelectedObject != null)
                {
                    joystick.OriginBinding = selectedObject.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).PositionProperty.Binding;
                    joystick.InputBinding.ChangeValue(ScalePropertyInitialValue, this);
                }
                else
                    joystick.OriginBinding = null;
            }
        }

        protected override void DoDisable()
        {
            if(ScalePropertyBinding != null)
                ScalePropertyBinding.ValueChanged -= ExternalValueChanged;
            joystick.InputBinding.ValueChanged -= input;
            joystick.DragInputStarted -= dragStarted;
            joystick.DragInputEnded -= dragEnded;
            if(this.ScaleValidationRule != null)
                joystick.InputBinding.ValidationRules.RemoveRange(this.ScaleValidationRule);
            joystick.DisableManipulator();
            base.DoDisable();
        }
        protected override void DoEnable(InputSystem inputSystem)
        {
            GetJoystick();
            if(ScalePropertyBinding != null)
                ScalePropertyBinding.ValueChanged += ExternalValueChanged;
            joystick.OriginBinding = selectedObject.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).PositionProperty.Binding;
            joystick.InputBinding.ChangeValue(ScalePropertyInitialValue, this);
            joystick.DragInputStarted += dragStarted;
            joystick.DragInputEnded += dragEnded;
            joystick.InputBinding.ValueChanged += input;
            if (this.ScaleValidationRule != null)
                joystick.InputBinding.ValidationRules.AddRange(this.ScaleValidationRule);
            base.DoEnable(inputSystem);        
        }

        private void ExternalValueChanged(float value, object source)
        {
            if(source != (System.Object)this)
            {
                joystick.InputBinding.ChangeValue(value, source);
            }
        }

        protected virtual void GetJoystick()
        {
            joystick = editor.ManipulatorsController.EnableManipulator<ScaleJoystickSystem>(ScaleJoystickSystem.DefaultKey);
        }


        private void dragEnded()
        {
            editor.ToolsController.EnableSceneControl();
            selector.UnlockSelection();
        }

        private void dragStarted()
        {
            editor.ToolsController.DisableSceneControl();
            selector.LockSelection();
        }

        private void input(float value, object source)
        {
            if (IsToolEnabled && source != (System.Object)this && SelectedObject != null)
            {
                ScalePropertyBinding.ChangeValue(value,this);
            }
        }
    }
}