using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenericModulePresenter : MonoBehaviour, IModulePresenter
{
    [Header("Base")]
    [SerializeField] RectTransform emptyPrefab;
    [SerializeField] TMP_Text moduleLabelPrefab;
    [SerializeField] RectTransform shortViewPrefab;


    [Header("Properties")]
    [SerializeField] TMP_InputField inputFieldPrefab;
    [SerializeField] TMP_Text propertyLabelPrefab;
    [SerializeField] TMP_Text lineLabelPrefab;
    [SerializeField] float startMargin = 10;
    [SerializeField] float propertiesMargin = 10;
    [SerializeField] float lineMargin = 5;

    public string ModuleName
    {
        get => moduleName;
        set
        {
            moduleName = value;
            moduleLabel.text = value;
        }
    }

    private string moduleName;
    private RectTransform fullView;
    private RectTransform shortView;
    private RectTransform propertiesContainer;
    private TMP_Text moduleLabel;

    private Vector2 propertiesOffset = Vector2.zero;
    private List<IModuleProperty> properties = new List<IModuleProperty>();


    public void Awake()
    {
        Init();
        propertiesOffset = new Vector2(0,startMargin);
    }

    private void Init()
    {
        fullView = GameObject.Instantiate(emptyPrefab);
        fullView.gameObject.name = "ModulePresenter";
        fullView.gameObject.SetActive(false);

        shortView = GameObject.Instantiate(shortViewPrefab,fullView);
        shortView.gameObject.name = "ShortView";
        shortView.gameObject.SetActive(false);

        propertiesContainer = GameObject.Instantiate(emptyPrefab, fullView);
        propertiesContainer.gameObject.name = "PropertiesContainer";
        propertiesContainer.gameObject.SetActive(false);
        propertiesContainer.anchoredPosition += new Vector2(0, - shortView.rect.height - (propertiesContainer.rect.height / 2)); 

        TMP_Text modLabel = GameObject.Instantiate(moduleLabelPrefab, shortView);
        modLabel.text = this.ModuleName;
        modLabel.name = "ModuleLabel";

        moduleLabel = modLabel;

    }

    public void AddProperty<T>(ModuleProperty<T> valueBinding)
    {
        properties.Add(valueBinding);
        
        RectTransform propertyElement = GameObject.Instantiate(emptyPrefab, propertiesContainer.transform);
        propertyElement.name = "Property";

        TMP_Text propertyLabel = GameObject.Instantiate(propertyLabelPrefab, propertyElement);
        propertyLabel.text = valueBinding.ValueLabel;
        propertyLabel.name = "PropertyLabel";



        if(valueBinding.Labels.Length == 1 && valueBinding.Labels[0] == "")
        {
            TMP_InputField valueInput = GameObject.Instantiate(inputFieldPrefab,propertyElement);
            RectTransform valueInputRect =  valueInput.gameObject.GetComponent<RectTransform>();
            valueInputRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, propertyLabel.rectTransform.rect.height);

            valueBinding.AddInputField(valueInput, 0);

            propertyElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, propertyLabel.rectTransform.rect.height);
            propertyElement.anchoredPosition -= new Vector2(0, propertyElement.rect.height / 2) + propertiesOffset;
            propertiesOffset += new Vector2(0,propertyLabel.rectTransform.rect.height + propertiesMargin);
        }
        else
        {
            uint i = 0;
            float offset = propertyLabel.rectTransform.rect.height + lineMargin;
            foreach (string valueLine in valueBinding.Labels)
            {
                TMP_Text lineText = GameObject.Instantiate(lineLabelPrefab, propertyElement.transform);
                lineText.text = valueLine;
                lineText.rectTransform.anchoredPosition = new Vector2(lineText.rectTransform.anchoredPosition.x,-(offset + (lineText.rectTransform.rect.height / 2)));

                TMP_InputField lineInput = GameObject.Instantiate(inputFieldPrefab, propertyElement.transform);
                RectTransform inputRect = lineInput.GetComponent<RectTransform>();
                inputRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineText.rectTransform.rect.height);
                inputRect.anchoredPosition = new Vector2(inputRect.rect.x , -(offset +(lineText.rectTransform.rect.height/2)));

                offset += inputRect.rect.height + lineMargin;
                valueBinding.AddInputField(lineInput, i);
                i++;
            }
            propertyElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offset);
            propertyElement.anchoredPosition -= new Vector2(0, (propertyElement.rect.height / 2)) + propertiesOffset;
            propertiesOffset += new Vector2(0,offset + propertiesMargin);
        }

    }

    public void Close()
    {
        fullView.gameObject.SetActive(false);
    }

    public RectTransform OpenFoldedView()
    {
        shortView.gameObject.SetActive(true);
        fullView.gameObject.SetActive(true);
        propertiesContainer.gameObject.SetActive(false);
        return shortView;
    }

    public RectTransform OpenFullView()
    {
        fullView.gameObject.SetActive(true);
        shortView.gameObject.SetActive(true);
        propertiesContainer.gameObject.SetActive(true);
        return fullView;
    }
}
