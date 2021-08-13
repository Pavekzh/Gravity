using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;
using BasicTools;
using UIExtended;

public class SceneStateManager : Singleton<SceneStateManager>
{
    [SerializeField] SaveSystemXML saveSystem;
    [SerializeField] SaveSystemXML loadSystem;
    public List<Planet> Planets { get; private set; }

    public delegate void SceneRefreshHandler();
    public event SceneRefreshHandler SceneRefreshed;
    private SceneState quickSave;

    public void QuickSave()
    {
        try
        {
            quickSave = this.GetState();
        }
        catch (System.Exception ex)
        {
            GenericErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }

    }

    public void LoadQuickSave()
    {
        try
        {
            if (quickSave != null)
                this.RefreshScene(quickSave);
        }
        catch (System.Exception ex)
        {
            GenericErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }
    }

    public void SaveToFile()
    {
        saveSystem.SaveToFile(GetState());
    }

    public void LoadFromFile()
    {
        SceneState state =  loadSystem.LoadFromFile(typeof(SceneState)) as SceneState;
        if (state == null)
        {
            GenericErrorManager.Instance.ShowErrorMessage("File have invalid format or doesn't exist", this);
        }
        else
        {
            this.RefreshScene(state);
        }
    }

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
