using UnityEngine;
using System.Collections;
using Assets.Library;
public class LoadSavedPlanet : MonoBehaviour
{
    [SerializeField] StateChanger uiChanger;
    [SerializeField] MoveSelectedPlanet moveSystem;
    [SerializeField] SaveSystemXML saveSystem;
    [SerializeField] string filePath;

    public StateChanger UIChanger { get => uiChanger; set => uiChanger = value; }
    public MoveSelectedPlanet MoveSystem { get => moveSystem; set => moveSystem = value; }
    public SaveSystemXML SaveSystem { get => saveSystem; set => saveSystem = value; }
    public string FilePath { get => filePath; set => filePath = value; }

    private bool isCreating;
    private Planet lastPlanet;
    private void Update()
    {
        if (isCreating)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                TouchReleased();
            }
        }
    }
    public void ViewPressed()
    {
        uiChanger.State = State.Default;
        SaveSystem.FilePath = FilePath;
        PlanetData planetData = SaveSystem.LoadFromFile(typeof(PlanetData)) as PlanetData;
        if (planetData != null)
        {
            PlanetBuilder planetBuilder = new PlanetBuilder(planetData);
            Planet planet = planetBuilder.CreatePlanet(GravityManager.Instance.PlanetsObject.transform);
            SelectManager.Instance.SelectObject(planet,this);

            isCreating = true;
            lastPlanet = planet;
            planet.DisablePhysicsModules();
            MoveSystem.EnableMoving();
        }
        else
            ErrorManager.Instance.ShowErrorMessage("Trying to load not PlanetData file as PlanetData",this);
    }
    public void TouchReleased()
    {
        if (isCreating && lastPlanet != null)
        {
            isCreating = false;
            if(TimeManager.Instance.IsPhysicsEnabled)
                lastPlanet.EnablePhysicsModules();
            MoveSystem.DisableMoving();
        }
    }
}
