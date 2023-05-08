using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Services
{
    public interface IPlanetFactory
    {
        GameObject CreatePlanet(SceneEditor.Models.PlanetData data);
    }
}
