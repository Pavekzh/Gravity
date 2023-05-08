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
        [SerializeField] int frameRate = 50;
        [SerializeField] bool useTimeScale = true;

        private float deltaTime = 0;
        private int predictIterationsNumber;
        private bool isVisible;
        private GravityComputer gravityComputer;
        private PlanetSelector selector;

        [Zenject.Inject]
        private void Construct(GravityComputer gravityComputer,PlanetSelector selector)
        {
            this.gravityComputer = gravityComputer;
            this.selector = selector;
        }

        private void Awake()
        {
            if (useTimeScale)
                deltaTime = Time.fixedDeltaTime;
            else
                deltaTime = (float)1 / frameRate;

            predictIterationsNumber = (int)(predictLimit / deltaTime);
        }

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
            if(selector.SelectedPlanet != null)
            {
                Guid selectedGravityInteractor = (selector.SelectedPlanet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key) as GravityModuleData).Planet.Guid;
                StateCurve<StateCurvePoint3D> curve;

                if (limitType == LimitType.Length)
                    curve = gravityComputer.PredictPositions((float)predictLimit, selectedGravityInteractor,deltaTime);
                else
                    curve = gravityComputer.PredictPositions(predictIterationsNumber, selectedGravityInteractor,deltaTime);

                curveDrawer.Draw(curve);
            }
        }
    }
}