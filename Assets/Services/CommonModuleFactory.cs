using Assets.SceneEditor.Models;
using Assets.SceneSimulation;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Services
{
    public class CommonModuleFactory:IModuleFactory
    {
        private IInstantiator instantiator;

        public CommonModuleFactory(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public Module CreateModule(GameObject planet,ModuleData data)
        {
            Module module = instantiator.InstantiateComponent(data.ModuleType, planet) as Module;

            if (module == null)
                throw new Exception("Module Type must inherited Module");
            else
                module.SetModuleData(data);

            return module;

        }
    }
}
