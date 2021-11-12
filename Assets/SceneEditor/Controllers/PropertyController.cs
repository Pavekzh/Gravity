using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;
using TMPro;
using UnityEngine.Events;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class PropertyController
    {
        private PropertyViewData propertyData;
        private RectTransform parentContainer;

        private TMP_InputField[] inputFields;
        private string[] inputTexts
        {
            get
            {
                string[] inputTexts = new string[inputFields.Length];
                for (int i = 0; i < inputFields.Length; i++)
                {
                    inputTexts[i] = inputFields[i].text;
                }
                return inputTexts;
            }
            set
            {
                if(inputFields != null)
                {
                    if (value.Length != inputFields.Length)
                        throw new System.Exception("inputTexts array must have same length with inputFields");
                    else
                    {
                        for (int i = 0; i < value.Length; i++)
                        {
                            if (inputFields[i] != null)
                                inputFields[i].text = value[i];
                        }
                    }
                }
                savedData = value;
            }
        }
        //used to store property value when ui destroyed (property view closed)
        private string[] savedData;
        private bool inputEntering = false;

        public float propertyOffset { get; private set; }

        public PropertyController(PropertyViewData propertyData)
        {            
            this.propertyData = propertyData;
            this.propertyData.ValueChanged += ValueChanged;

            if (propertyData.Components != null)
                this.inputFields = new TMP_InputField[propertyData.Components.Length];
            else
                this.inputFields = new TMP_InputField[1];            
        }

        public void CreateView(RectTransform parentContainer, float viewOffset)
        {

            if (propertyData.Components != null)
                this.inputFields = new TMP_InputField[propertyData.Components.Length];
            else
                this.inputFields = new TMP_InputField[1];

            this.propertyOffset = viewOffset;
            this.parentContainer = parentContainer;

            RectTransform propertyElement = GameObject.Instantiate(ValuesPanelTemplate.Instance.EmptyPrefab, parentContainer.transform);
            propertyElement.name = "Property";

            TMP_Text propertyLabel = GameObject.Instantiate(ValuesPanelTemplate.Instance.PropertyLabelPrefab, propertyElement);
            propertyLabel.text = propertyData.Name;
            propertyLabel.name = "PropertyLabel";

            if (propertyData.Components != null && ((propertyData.Components.Length == 1 && propertyData.Components[0] != "") || (propertyData.Components.Length > 1)))
            {
                uint i = 0;
                float offset = propertyLabel.rectTransform.rect.height + ValuesPanelTemplate.Instance.LineMargin;
                foreach (string valueLine in propertyData.Components)
                {
                    TMP_Text lineText = GameObject.Instantiate(ValuesPanelTemplate.Instance.LineLabelPrefab, propertyElement.transform);
                    lineText.text = valueLine;
                    lineText.rectTransform.anchoredPosition = new Vector2(lineText.rectTransform.anchoredPosition.x, -(offset + (lineText.rectTransform.rect.height / 2)));

                    TMP_InputField lineInput = GameObject.Instantiate(ValuesPanelTemplate.Instance.InputFieldPrefab, propertyElement.transform);
                    lineInput.text = savedData[i];
                    RectTransform inputRect = lineInput.GetComponent<RectTransform>();
                    inputRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineText.rectTransform.rect.height);
                    inputRect.anchoredPosition = new Vector2(inputRect.anchoredPosition.x, -(offset + (lineText.rectTransform.rect.height / 2)));

                    offset += inputRect.rect.height + ValuesPanelTemplate.Instance.LineMargin;
                    inputFields[i] = lineInput;
                    AddListeners(lineInput);
                    i++;
                }
                propertyElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offset);
                propertyElement.anchoredPosition -= new Vector2(0, (propertyElement.rect.height / 2) + propertyOffset);
                propertyOffset += offset + ValuesPanelTemplate.Instance.PropertiesMargin;
            }
            else
            {
                TMP_InputField valueInput = GameObject.Instantiate(ValuesPanelTemplate.Instance.InputFieldPrefab, propertyElement);
                valueInput.text = savedData[0];
                RectTransform valueInputRect = valueInput.gameObject.GetComponent<RectTransform>();
                valueInputRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, propertyLabel.rectTransform.rect.height);

                AddListeners(valueInput);
                inputFields[0] = valueInput;

                propertyElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, propertyLabel.rectTransform.rect.height);
                propertyElement.anchoredPosition -= new Vector2(0, (propertyElement.rect.height / 2) + propertyOffset);
                propertyOffset += propertyLabel.rectTransform.rect.height + ValuesPanelTemplate.Instance.PropertiesMargin;
            }
        }

        private void AddListeners(TMP_InputField inputField)
        {
            UnityAction<string> action = new UnityAction<string>(InputChanged);
            inputField.onValueChanged.AddListener(action);
            UnityAction<string> selectAction = new UnityAction<string>(InputFieldSelected);
            inputField.onSelect.AddListener(selectAction);
            UnityAction<string> deselectAction = new UnityAction<string>(InputFieldDeselected);
            inputField.onDeselect.AddListener(deselectAction);
        }

        private void InputChanged(string changedText)
        {
            if(inputEntering)
                propertyData.ChangePresenter(inputTexts, this);
        }

        private void ValueChanged(string[] presentation,object sender)
        {
            if (sender != this && !inputEntering)
                inputTexts = presentation;

        }

        private void InputFieldSelected(string value)
        {
            inputEntering = true;
        }

        private void InputFieldDeselected(string value)
        {
            inputEntering = false;
        }
    }
}