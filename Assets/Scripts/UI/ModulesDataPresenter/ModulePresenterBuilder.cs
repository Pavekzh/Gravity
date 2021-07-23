using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ModulePresenterBuilder : MonoBehaviour
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

            if(moduleLabel != null)
                moduleLabel.text = value;
        }
    }

    private string moduleName;
    private RectTransform moduleRoot;
    private RectTransform label;
    private RectTransform propertiesContainer;
    private TMP_Text moduleLabel;

    private Vector2 propertiesOffset = Vector2.zero;

    public List<IModuleProperty> properties = new List<IModuleProperty>();

    private void Awake()
    {
        propertiesOffset = new Vector2(0, startMargin);
    }

    public void ResetSettings()
    {
        properties.Clear();
        moduleName = "";

        moduleRoot = null;
        label = null;
        propertiesContainer = null;
        moduleLabel = null;
    }

    public IModulePresenter GetPresenter()
    {
        Init();
        foreach(IModuleProperty property in properties)
        {
            AddProperty(property);
        }
        GenericModulePresenter modulePresenter = new GenericModulePresenter(moduleRoot,label,propertiesContainer);
        return modulePresenter;
    }

    private void Init()
    {
        moduleRoot = GameObject.Instantiate(emptyPrefab);
        moduleRoot.gameObject.name = "ModulePresenter";
        moduleRoot.gameObject.SetActive(false);

        label = GameObject.Instantiate(shortViewPrefab, moduleRoot);
        label.gameObject.name = "ShortView";
        label.gameObject.SetActive(false);

        propertiesContainer = GameObject.Instantiate(emptyPrefab, moduleRoot);
        propertiesContainer.gameObject.name = "PropertiesContainer";
        propertiesContainer.gameObject.SetActive(false);
        propertiesContainer.anchoredPosition += new Vector2(0, -label.rect.height - (propertiesContainer.rect.height / 2));

        TMP_Text modLabel = GameObject.Instantiate(moduleLabelPrefab, label);
        modLabel.text = this.ModuleName;
        modLabel.name = "ModuleLabel";

        moduleLabel = modLabel;
    }

    public void AddProperty(IModuleProperty valueBinding)
    {
        RectTransform propertyElement = GameObject.Instantiate(emptyPrefab, propertiesContainer.transform);
        propertyElement.name = "Property";

        TMP_Text propertyLabel = GameObject.Instantiate(propertyLabelPrefab, propertyElement);
        propertyLabel.text = valueBinding.ValueLabel;
        propertyLabel.name = "PropertyLabel";

        if (valueBinding.Labels.Length == 1 && valueBinding.Labels[0] == "")
        {
            TMP_InputField valueInput = GameObject.Instantiate(inputFieldPrefab, propertyElement);
            RectTransform valueInputRect = valueInput.gameObject.GetComponent<RectTransform>();
            valueInputRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, propertyLabel.rectTransform.rect.height);

            valueBinding.AddInputField(valueInput, 0);

            propertyElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, propertyLabel.rectTransform.rect.height);
            propertyElement.anchoredPosition -= new Vector2(0, propertyElement.rect.height / 2) + propertiesOffset;
            propertiesOffset += new Vector2(0, propertyLabel.rectTransform.rect.height + propertiesMargin);
        }
        else
        {
            uint i = 0;
            float offset = propertyLabel.rectTransform.rect.height + lineMargin;
            foreach (string valueLine in valueBinding.Labels)
            {
                TMP_Text lineText = GameObject.Instantiate(lineLabelPrefab, propertyElement.transform);
                lineText.text = valueLine;
                lineText.rectTransform.anchoredPosition = new Vector2(lineText.rectTransform.anchoredPosition.x, -(offset + (lineText.rectTransform.rect.height / 2)));

                TMP_InputField lineInput = GameObject.Instantiate(inputFieldPrefab, propertyElement.transform);
                RectTransform inputRect = lineInput.GetComponent<RectTransform>();
                inputRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineText.rectTransform.rect.height);
                inputRect.anchoredPosition = new Vector2(inputRect.rect.x, -(offset + (lineText.rectTransform.rect.height / 2)));

                offset += inputRect.rect.height + lineMargin;
                valueBinding.AddInputField(lineInput, i);
                i++;
            }
            propertyElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offset);
            propertyElement.anchoredPosition -= new Vector2(0, (propertyElement.rect.height / 2)) + propertiesOffset;
            propertiesOffset += new Vector2(0, offset + propertiesMargin);
        }

    }

}
