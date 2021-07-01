using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.Library;
using TMPro;

public class PlanetFilePresenter : FilePresenter
{
    [SerializeField] ElementStateChanger UIChanger;
    [SerializeField] MoveSelectedPlanet moveSystem;
    [SerializeField] SaveSystemXML saveSystem;
    [SerializeField] LoadSavedPlanet loader;
    [SerializeField] RectTransform containerElement;
    [SerializeField] RectTransform topElement;
    [SerializeField] RectTransform bottomElement;
    [SerializeField] TMP_Text label;
    [SerializeField] [Range(0, 1)] float topElementFill;


    public override RectTransform GetFileView(string path, Vector2 cellSize)
    {
        loader.UIChanger = UIChanger;
        loader.FilePath = path;
        loader.SaveSystem = saveSystem;
        loader.MoveSystem = moveSystem;
        label.text = System.IO.Path.GetFileNameWithoutExtension(path);

        topElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
        topElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * topElementFill);
        topElement.anchoredPosition = new Vector2(cellSize.x / 2, -(cellSize.y * topElementFill) / 2);

        bottomElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
        bottomElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * (1 - topElementFill));
        bottomElement.anchoredPosition = new Vector2(0, bottomElement.rect.height / 2);

        GameObject container = GameObject.Instantiate(containerElement.gameObject);
        GameObject top = GameObject.Instantiate(topElement.gameObject, container.transform);
        GameObject bottom = GameObject.Instantiate(bottomElement.gameObject, container.transform);

        return container.GetComponent<RectTransform>();
    }
    public void PointerDown(PointerEventData eventData)
    {

    }
}
