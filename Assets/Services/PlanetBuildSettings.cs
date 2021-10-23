using System;
using UnityEngine;
using BasicTools;

namespace Assets.Services
{
    public class PlanetBuildSettings:Singleton<PlanetBuildSettings>
    {
        [SerializeField] private Transform planetsParent;
        [SerializeField] private GameObject basePlanetPrefab;

        public Transform PlanetsParent { get => planetsParent; }
        public GameObject BasePlanetPrefab { get => basePlanetPrefab; }
    }
}
