using System.Collections;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneSimulation
{ 
    public abstract class Module : MonoBehaviour
    {
        public abstract ModuleData InstatiateModuleData();
    }
}