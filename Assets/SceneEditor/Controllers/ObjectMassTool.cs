using BasicTools;
using UIExtended;
using Assets.SceneEditor.Models;
using System.Collections.Generic;
using UnityEngine;
using Assets.Services;
using BasicTools.Validation;

namespace Assets.SceneEditor.Controllers
{
    public class ObjectMassTool : ObjectScaleTool
    {
        public override string DefaultKey => "ObjectMassTool";
        public override string ToolName => "Mass set tool";

        protected SphericalViewPlanetsArrangementManager arrangementManager;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override IValidationRule<float>[] ScaleValidationRule => null;

        protected override Binding<float> ScalePropertyBinding
        {
            get 
            {
                if (SelectedObject != null)
                    return SelectedObject.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).MassProperty.Binding;
                else return null;
            }
        }

        protected override float ScalePropertyInitialValue
        {
            get { return SelectedObject.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).Mass; }
        }

        protected override void selectedObjectChanged(object sender, PlanetController planet)
        {
            if(SelectedObject != null)
                ScalePropertyBinding.ValueChanged -= InputChanged;

            base.selectedObjectChanged(sender, planet);
            
            if(SelectedObject != null)
                ScalePropertyBinding.ValueChanged += InputChanged;
        }

        protected override void DoDisable()
        {
            base.DoDisable();

            ScalePropertyBinding.ValueChanged -= InputChanged;
            arrangementManager.ClearArrangement();
        }

        protected override void DoEnable(InputSystem inputSystem)
        {                      
            base.DoEnable(inputSystem); 
            ScalePropertyBinding.ValueChanged += InputChanged;
            arrangementManager = new SphericalViewPlanetsArrangementManager(new MassEstimator(), 1, 5, x => x * 10);
            arrangementManager.ShowArrangement();
        }

        protected override void GetJoystick()
        {
            joystick = EditorController.Instance.ManipulatorsController.EnableManipulator<RelativeScaleJoystickSystem>(RelativeScaleJoystickSystem.DefaultKey);
        }

        private void InputChanged(float value, object source)
        {
            if (IsToolEnabled)
            {
                if (arrangementManager.IsShowing)
                    arrangementManager.ShowArrangement();
            }

        }

        public void ChangePropertyArrangementState()
        {
            if (arrangementManager.IsShowing)
                arrangementManager.ClearArrangement();
            else
                arrangementManager.ShowArrangement();
        }
    }
}