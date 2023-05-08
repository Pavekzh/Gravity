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

        protected MassEstimator planetEstimator;
        protected IPlanetsArrangementTool<float> planetsArrangementTool;

        [Zenject.Inject]
        protected void Construct(IPlanetsArrangementTool<float> planetsArrangementTool)
        {
            this.planetsArrangementTool = planetsArrangementTool;            
            planetEstimator = new MassEstimator();
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

            if(ScalePropertyBinding != null)
                ScalePropertyBinding.ValueChanged -= InputChanged;
            planetsArrangementTool.HideArrangement();
        }

        protected override void DoEnable(InputSystem inputSystem)
        {                      
            base.DoEnable(inputSystem); 
            if(ScalePropertyBinding != null)
                ScalePropertyBinding.ValueChanged += InputChanged;
            planetsArrangementTool.ShowArrangement(planetEstimator);
        }

        protected override void GetJoystick()
        {
            joystick = editor.ManipulatorsController.EnableManipulator<RelativeScaleJoystickSystem>(RelativeScaleJoystickSystem.DefaultKey);
        }

        private void InputChanged(float value, object source)
        {
            if (IsToolEnabled)
            {
                if (planetsArrangementTool.IsShowing)
                    planetsArrangementTool.ShowArrangement(planetEstimator);
            }

        }

        public void ChangePropertyArrangementState()
        {
            if (planetsArrangementTool.IsShowing)
                planetsArrangementTool.HideArrangement();
            else
                planetsArrangementTool.ShowArrangement(planetEstimator);
        }
    }
}