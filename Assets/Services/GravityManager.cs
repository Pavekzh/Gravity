using System;
using System.Collections.Generic;
using BasicTools;
using UnityEngine;
using Assets.SceneSimulation;
using Assets.SceneEditor.Models;
using System.Linq;

namespace Assets.Services
{
    public class GravityManager:Singleton<GravityManager>
    {
        [SerializeField] private float gravityRatio;
        [SerializeField] private List<SceneSimulation.GravityModule> interactors;
        public List<SceneSimulation.GravityModule> GravityInteractors { get => interactors; }
        public float GravityRatio { get => gravityRatio; }

        void Start()
        {
            SceneRefreshed();
            SceneStateManager.Instance.SceneRefreshed += SceneRefreshed;
        }

        void SceneRefreshed()
        {
            interactors = new List<SceneSimulation.GravityModule>();
        }

        //test
        public GameObject checkpoint;            
        List<GameObject> checkPoints;
        public void FixedUpdate()
        {
            //test
            if(checkPoints == null)
            {
                checkPoints = new List<GameObject>();
                for (int i = 1; i < 201; i++)
                {
                    checkPoints.Add(GameObject.Instantiate(checkpoint));
                }
            }

            if (interactors.Count != 0)
            {
                //List<List<GravityInteractor>> interactorsPrediction = Predict(200);
                List<GravityInteractor> prediction = Predict(200, GravityInteractors[0]);

                Vector3 lastPos = prediction[0].Position.GetVector3();
                for (int i = 1; i < 200; i++)
                {
                    Vector3 pos = prediction[i].Position.GetVector3();
                    checkPoints[i].transform.position = prediction[i].Position.GetVector3();
                    Debug.DrawLine(lastPos, pos, Color.red);
                    lastPos = pos;
                }

                //Vector3 lastPos = interactorsPrediction[0][0].Position.GetVector3();
                //for (int i = 1; i < 200; i++)
                //{
                //    Vector3 pos = interactorsPrediction[i][0].Position.GetVector3();
                //    Debug.DrawLine(lastPos, pos, Color.blue);
                //    lastPos = pos;
                //}

                //for (int i = 0; i < interactors.Count(); i++)
                //{
                //    Vector3 lastPos = interactorsPrediction[0][i].Position.GetVector3();
                //    for (int j = 1; j < 100; j++)
                //    {
                //        Vector3 pos = interactorsPrediction[j][i].Position.GetVector3();
                //        Debug.DrawLine(lastPos, pos, Color.blue);
                //        lastPos = pos;

                //    }
                //}
            }

        }
        //end test

        public List<GravityInteractor> Predict(int iterations,GravityModule predictObject)
        {
            int indexOfElem = GravityInteractors.IndexOf(predictObject);
            List<GravityInteractor> interactorPrediction = new List<GravityInteractor>();
            List<GravityInteractor> lastAllInteractorsState = GravityInteractors.Select(x => x.data).ToList();
            int allInteractors = lastAllInteractorsState.Count();


            if (allInteractors != 0)
            {
                interactorPrediction.Add(lastAllInteractorsState[indexOfElem]);

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
                        if (j == indexOfElem)
                            interactorPrediction.Add(newState);
                    }
                    lastAllInteractorsState = allInteractorsState;
                }

            }
            return interactorPrediction;


        }

        public List<List<GravityInteractor>> Predict(int iterations)
        {
            List<GravityInteractor> startInteractorsData = interactors.Select(x => x.data).ToList();                
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
            foreach (GravityModule obj in GravityInteractors)
            {
                if (obj.IsSimulationEnabled)
                {
                    force += GravityRatio * ComputeForce(data, obj.data);
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
