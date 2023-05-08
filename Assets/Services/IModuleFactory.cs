using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Services
{
    public interface IModuleFactory
    {
        public SceneSimulation.Module CreateModule(GameObject parent, SceneEditor.Models.ModuleData data);
    }
}
