using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.SceneEditor.Controllers;
using Assets.SceneEditor.Models;

namespace Assets.Services
{
    public class PropertyControllerFactory
    {
        private Zenject.IInstantiator instantiator;

        public PropertyControllerFactory(Zenject.IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public PropertyController Create(PropertyViewData propertyData)
        {
            PropertyController controller = instantiator.Instantiate<PropertyController>(new object[] { propertyData });
            return controller;
        }
    }
}
