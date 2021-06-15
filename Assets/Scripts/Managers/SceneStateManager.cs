using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;


public class SceneStateManager : Singleton<SceneStateManager>
{
    public List<Planet> Planets { get; private set; }

    public delegate void SceneRefreshHandler();
    public event SceneRefreshHandler SceneRefreshed;

    public override void Awake()
    {
        base.Awake();
        Planets = new List<Planet>();
        this.SceneRefreshed += RefreshSettings;
    }
    public SceneState GetState()
    {
        List<PlanetData> planetsData = new List<PlanetData>();
        foreach (Planet planet in Planets)
        {
            planetsData.Add(planet.GetPlanetData());
        }
        SceneState state = new SceneState(planetsData);
        return state;
    }
    public void RefreshScene(SceneState state)
    {
        ClearScene();
        foreach(PlanetData planet in state.PlanetsData)
        {
            PlanetBuilder builder = new PlanetBuilder(planet);
            builder.CreatePlanet(GravityManager.Instance.PlanetsObject.transform);
        }
    }
    private void RefreshSettings()
    {
        Planets = new List<Planet>();
    }
    public void ClearScene()
    {
        SceneRefreshed?.Invoke();
        foreach (Transform child in GravityManager.Instance.PlanetsObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
