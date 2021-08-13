using UnityEngine;
using System.Collections;

public class GeneralModulePresenter : IModulePresenter
{
    private RectTransform root;
    private RectTransform label;
    private RectTransform propertiesContainer;

    public GeneralModulePresenter(RectTransform Root,RectTransform Label, RectTransform PropertiesContainer)
    {
        this.root = Root;
        this.label = Label;
        this.propertiesContainer = PropertiesContainer;
    }

    public void Close()
    {
        root.gameObject.SetActive(false);
    }

    public RectTransform OpenFoldedView()
    {
        label.gameObject.SetActive(true);
        root.gameObject.SetActive(true);
        propertiesContainer.gameObject.SetActive(false);
        return label;
    }

    public RectTransform OpenFullView()
    {
        root.gameObject.SetActive(true);
        label.gameObject.SetActive(true);
        propertiesContainer.gameObject.SetActive(true);
        return root;
    }

}
