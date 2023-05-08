using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.SceneEditor.Controllers;
using Assets.SceneEditor.Models;

namespace Assets.Services
{
    public class ModuleControllerFactory
    {
        private Zenject.IInstantiator instantiator;

        public ModuleControllerFactory(Zenject.IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public ModuleController Create(ModuleData moduleData)
        {
            ModuleController controller = instantiator.Instantiate<ModuleController>(new object[] { moduleData });
            return controller;
        }
    }
}
