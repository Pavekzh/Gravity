using System;
using System.Collections.Generic;
using BasicTools;
using UnityEngine;
using Assets.SceneSimulation;
using Assets.SceneEditor.Models;
using System.Linq;


namespace Assets.Services
{
    public class GravityComputer:MonoBehaviour
    {
        [SerializeField] private float gravityRatio;
        [SerializeField] private Dictionary<Guid, GravityModule> interactors = new Dictionary<Guid, GravityModule>();

        private SceneInstance sceneInstance;

        public Dictionary<Guid, GravityModule> GravityInteractors { get => interactors; }
        public float GravityRatio 
        { 
            get => gravityRatio; 
            private set
            {               
                gravityRatio = value;
                GravityBinding.ChangeValue(value, this);

            }
        }

        public Binding<float> GravityBinding { get; private set; } = new Binding<float>();
        
        [Zenject.Inject]
        private void Construct(SceneInstance sceneInstance)
        {
            this.sceneInstance = sceneInstance;
            sceneInstance.SceneSet += SceneChanged;            
            
            GravityBinding.ValueChanged += GravityRatioChanged;           
            GravityBinding.ChangeValue(gravityRatio,this);
        }

        private void GravityRatioChanged(float value, object source)
        {
            if(source != (System.Object)this)
            {
                gravityRatio = value;
                sceneInstance.CurrentScene.Gravity = value;
                if(value <= 0)
                {
                    if(PlayerPrefs.GetInt("EasterEgg1",0) == 0)
                    {
                        MessagingSystem.Instance.ShowMessage("With negative or zero gravity also works :)", this);
                        PlayerPrefs.SetInt("EasterEgg1", 1);
                    }          
                }
                    
            }
        }

        private void SceneChanged(SceneState scene, bool isChangedLocal)
        {
            if(scene.Planets.Count == 0)
                interactors = new Dictionary<Guid, GravityModule>();

            if(!isChangedLocal)
                this.GravityRatio = scene.Gravity;
        }
        
        public StateCurve<StateCurvePoint3D> PredictPositions(float curveLength, Guid element,float deltaTime)
        {
            StateCurve<StateCurvePoint3D> Curve = new StateCurve<StateCurvePoint3D>();
            List<GravityInteractor> lastAllInteractorsState = new List<GravityInteractor>();
            int allInteractors = 0;
            int predictableElement = -1;

            foreach (KeyValuePair<Guid, GravityModule> interactor in GravityInteractors)
            {
                if (element == interactor.Key)
                    predictableElement = allInteractors;

                lastAllInteractorsState.Add(interactor.Value.Data);
                allInteractors++;
            }
            if (predictableElement == -1)
                throw new Exception("Element was not found (GravityManager.Predict:86)");

            if (allInteractors != 0)
            {
                Curve.AddPoint(new GravityStateCurvePoint(lastAllInteractorsState[predictableElement], 0));


                while (Curve.Length < curveLength)
                {
                    List<GravityInteractor> allInteractorsState = new List<GravityInteractor>();
                    for (int j = 0; j < allInteractors; j++)
                    {
                        GravityInteractor newState = new GravityInteractor(lastAllInteractorsState[j]);
                        Vector3 velocityChange = ComputeForce(newState, lastAllInteractorsState, gravityRatio) * deltaTime / newState.Mass;
                        newState.Velocity += velocityChange.GetVectorXZ();
                        newState.Position += newState.Velocity * deltaTime;

                        allInteractorsState.Add(newState);
                        if (j == predictableElement)
                        {
                            float newDistanceFromStart = Curve.Length + newState.Velocity.magnitude * deltaTime;
                            if (newDistanceFromStart < curveLength)
                            {
                                Curve.AddPoint(new GravityStateCurvePoint(newState, newDistanceFromStart));
                            }
                            else
                            {
                                Curve.AddPoint((GravityStateCurvePoint)Curve.Points[Curve.PointsAmount - 1].GetPointBetween(new GravityStateCurvePoint(newState, newDistanceFromStart), curveLength - Curve.Length));                
                            }
                        }

                    }
                    lastAllInteractorsState = allInteractorsState;
                }

            }
            return Curve;
        }

        public StateCurve<StateCurvePoint3D> PredictPositions(int iterations, Guid element, float deltaTime)
        {
            StateCurve<StateCurvePoint3D> Curve = new StateCurve<StateCurvePoint3D>();
            List<GravityInteractor> lastAllInteractorsState = new List<GravityInteractor>();
            int allInteractors = 0;
            int predictableElement = -1;

            foreach(KeyValuePair<Guid,GravityModule> interactor in GravityInteractors)
            {
                if (element == interactor.Key)
                    predictableElement = allInteractors;

                lastAllInteractorsState.Add(interactor.Value.Data);
                allInteractors++;
            }
            if (predictableElement == -1)
                throw new Exception("Element was not found (GravityManager.Predict:86)");

            if (allInteractors != 0)
            {
                Curve.AddPoint(new GravityStateCurvePoint(lastAllInteractorsState[predictableElement], 0));

                for (int i = 1; i < iterations; i++)
                {
                    List<GravityInteractor> allInteractorsState = new List<GravityInteractor>();

                    for (int j = 0; j < allInteractors; j++)
                    {
                        GravityInteractor newState = new GravityInteractor(lastAllInteractorsState[j]);
                        Vector3 velocityChange = ComputeForce(newState, lastAllInteractorsState, gravityRatio) * deltaTime / newState.Mass;
                        newState.Velocity += velocityChange.GetVectorXZ();
                        newState.Position += newState.Velocity * deltaTime;

                        allInteractorsState.Add(newState);
                        if (j == predictableElement)
                            Curve.AddPoint(new GravityStateCurvePoint(newState, Curve.Length + newState.Velocity.magnitude * deltaTime));
                    }
                    lastAllInteractorsState = allInteractorsState;
                }
            }
            return Curve;
        }

        public StateCurve<GravityStateCurvePoint> Predict(int iterations,Guid element)
        {
            int predictableElement = -1;
            StateCurve<GravityStateCurvePoint> Curve = new StateCurve<GravityStateCurvePoint>();
            List<GravityInteractor> lastAllInteractorsState = GravityInteractors.Select(x => x.Value.Data).ToList();
            int allInteractors = 0;

            foreach (KeyValuePair<Guid, GravityModule> interactor in GravityInteractors)
            {
                if (element == interactor.Key)
                    predictableElement = allInteractors;

                lastAllInteractorsState.Add(interactor.Value.Data);
                allInteractors++;
            }
            if (predictableElement == -1)
                throw new Exception("Element was not found (GravityManager.Predict:86)");

            if (allInteractors != 0)
            {
                Curve.AddPoint(new GravityStateCurvePoint(lastAllInteractorsState[predictableElement], 0));

                for (int i = 1; i < iterations; i++)
                {
                    List<GravityInteractor> allInteractorsState = new List<GravityInteractor>();

                    for(int j = 0; j < allInteractors; j++)
                    {
                        GravityInteractor newState = new GravityInteractor(lastAllInteractorsState[j]);
                        Vector3 velocityChange = ComputeForce(newState, lastAllInteractorsState, gravityRatio) * Time.deltaTime / newState.Mass;
                        newState.Velocity += velocityChange.GetVectorXZ();
                        newState.Position += newState.Velocity * Time.fixedDeltaTime;
                        
                        allInteractorsState.Add(newState);
                        if (j == predictableElement)
                            Curve.AddPoint(new GravityStateCurvePoint(newState,Curve.Length + newState.Velocity.magnitude * Time.fixedDeltaTime));
                    }
                    lastAllInteractorsState = allInteractorsState;
                }
            }
            return Curve;
        }

        public List<List<GravityInteractor>> Predict(int iterations)
        {
            List<GravityInteractor> startInteractorsData = interactors.Select(x => x.Value.Data).ToList();                
            List<List<GravityInteractor>> interactorsPrediction = new List<List<GravityInteractor>>();

            if(startInteractorsData.Count() != 0)
            {
                interactorsPrediction.Add(startInteractorsData);
                for (int i = 1; i < iterations; i++)
                {
                    List<GravityInteractor> interactorsData = new List<GravityInteractor>();
                    interactorsPrediction.Add(interactorsData);

                    foreach (GravityInteractor interactor in interactorsPrediction[i - 1])
                    {
                        GravityInteractor newState = new GravityInteractor(interactor);                       
                        Vector3 velocityChange = ComputeForce(newState, interactorsPrediction[i - 1], gravityRatio) * Time.deltaTime / newState.Mass;
                        newState.Velocity += velocityChange.GetVectorXZ();
                            
                        newState.Position += newState.Velocity * Time.fixedDeltaTime;
                        interactorsData.Add(newState);
                    }
                }

            }
            return interactorsPrediction;
        }

        public Vector3 ComputeForce(GravityInteractor data)
        {
            Vector3 force = Vector3.zero;
            foreach (KeyValuePair<Guid,GravityModule> obj in GravityInteractors)
            {
                if (obj.Value.IsSimulationEnabled)
                {
                    force += GravityRatio * ComputeForce(data, obj.Value.Data);
                }

            }
            return force;
        }

        public static Vector3 ComputeForce(GravityInteractor data, IEnumerable<GravityInteractor> Environment, float GravityRatio)
        {
            Vector3 force = Vector3.zero;
            foreach (GravityInteractor obj in Environment)
            {
                force += GravityRatio * ComputeForce(data, obj);
            }
            return force;
        }

        public static Vector3 ComputeForce(GravityInteractor originInteractor, GravityInteractor directionInteractor)
        {
            float distance = Vector2.Distance(originInteractor.Position, directionInteractor.Position);

            float forceValue = 0;
            Vector3 forceDirection = Vector3.zero;

            if (distance != 0)
            {
                forceValue = (originInteractor.Mass * directionInteractor.Mass) / (distance * distance);
                forceDirection = (directionInteractor.Position - originInteractor.Position).GetVector3().normalized;
            }

            return forceDirection * forceValue;
        }
    }
}
