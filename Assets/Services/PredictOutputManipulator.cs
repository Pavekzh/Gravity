using System;
using UnityEngine;
using UIExtended;
using BasicTools;
using Assets.SceneEditor.Models;
using Assets.SceneEditor.Controllers;

namespace Assets.Services
{
    public class PredictOutputManipulator : OutputManipulator
    {
        public enum LimitType
        {
            Time,
            Length
        }
        [SerializeField] CurveDrawer curveDrawer;        
        [SerializeField] float predictLimit = 2;
        [SerializeField] LimitType limitType;

        int predictIterationsNumber { get => (int)(predictLimit / Time.fixedDeltaTime); }
        private bool isVisible;

        public override bool IsVisible 
        {
            get => isVisible;
            set
            {
                curveDrawer.gameObject.SetActive(value);
                isVisible = value;
            }
        }

        public override void UpdateManipulatorView(Vector3 origin, Vector3 directPoint, float scaleFactor)
        {
            if(EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet != null)
            {
                Guid selectedGravityInteractor = (EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key) as GravityModuleData).Guid;
                StateCurve<StateCurvePoint3D> curve;

                if (limitType == LimitType.Length)
                    curve = GravityManager.Instance.PredictPositions((float)predictLimit, selectedGravityInteractor);
                else
                    curve = GravityManager.Instance.PredictPositions(predictIterationsNumber, selectedGravityInteractor);

                curveDrawer.Draw(curve);
            }
        }
    }
}