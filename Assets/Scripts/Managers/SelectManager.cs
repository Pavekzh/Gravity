using UnityEngine;
using System.Collections;
using Assets.Library;

public class SelectManager : Singleton<SelectManager>
{
    public delegate void SelectedPlanetChangedHandler(Planet planet,object sender);
    public event SelectedPlanetChangedHandler SelectedPlanetChanged;

    public Planet SelectedObject { get; private set; }

    [SerializeField] Planet defaultSelectedObject;
    [SerializeField] bool isSelectionEnabled;
    [SerializeField] string planetsLayer;
    [SerializeField] float selectSphereRadius;

    int layerMask;
    private void Start()
    {
        SelectObject(defaultSelectedObject, this);
        layerMask = 1 << LayerMask.NameToLayer(planetsLayer);
    }
    public void SelectObject(Planet planet,object sender)
    {
        SelectedObject = planet;
        SelectedPlanetChanged?.Invoke(planet, sender);
    }
    private void Update()
    {
        if (isSelectionEnabled)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hitinfo;
                if (Physics.SphereCast(ray, selectSphereRadius, out hitinfo, Mathf.Infinity, layerMask))
                {
                    SelectObject(hitinfo.collider.gameObject.GetComponent<Planet>(), this);

                    if (SelectedObject == null)
                    {
                        ErrorManager.Instance.ShowErrorMessage("SelectedObject must have Planet component",this);
                    }
                }

            }
        }
    }
}
