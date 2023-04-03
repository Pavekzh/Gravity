using System;
using Assets.SceneEditor.Models;

namespace Assets.Services
{
    public interface IPlanetEstimator<T>
    {
       T Estimate(PlanetData planet);
    }
}
