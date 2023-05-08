using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Services
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] GameObject PlanetPrefab;

        public override void InstallBindings()
        {
            InstallPlanetFactory();
        }
        private void InstallPlanetFactory()
        {
            Container.Bind<IPlanetFactory>().To<FacadeOnlyPlanetFactory>().FromNew().AsTransient().WithArguments(PlanetPrefab);
            Container.Bind<IModuleFactory>().To<CommonModuleFactory>().FromNew().AsTransient();
        }
    }
}