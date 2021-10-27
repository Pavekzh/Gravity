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
    }
}
