﻿using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.SceneEditor.Models;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    class ModuleController
    {
        private RectTransform moduleRoot;
        private RectTransform label;
        private RectTransform propertiesContainer;
        private TMP_Text moduleLabel;

        private List<PropertyController> propertyControllers = new List<PropertyController>();
        private float propertiesOffset;

        public float ModuleOffset { get; private set; }
        public ModuleData ModuleData { get; private set; }

        public ModuleController(ModuleData data)
        {
            this.ModuleData = data;
            foreach(PropertyViewData property in data.Properties)
            {
                PropertyController controller = new PropertyController(property);
                propertyControllers.Add(controller);
            }
        }
    
        public void CreateView(RectTransform parent,float modulesOffset)
        {
            Init(parent,modulesOffset);
            foreach(PropertyController controller in propertyControllers)
            {
                controller.CreateView(propertiesContainer, propertiesOffset);
                propertiesOffset = controller.propertyOffset;

            }
            ModuleOffset += propertiesOffset;
        } 

        private void Init(RectTransform parent,float offset)
        {
            moduleRoot = GameObject.Instantiate(ValuesPanelTemplate.Instance.EmptyPrefab,parent);
            moduleRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width);
            moduleRoot.anchoredPosition = new Vector2(0, (-moduleRoot.rect.height / 2) - offset);
            moduleRoot.gameObject.name = "ModulePresenter";
            ModuleOffset = offset + moduleRoot.rect.height;

            label = GameObject.Instantiate(ValuesPanelTemplate.Instance.ShortViewPrefab, moduleRoot);
            label.gameObject.name = "ShortView";
            TMP_Text modLabel = GameObject.Instantiate(ValuesPanelTemplate.Instance.ModuleLabelPrefab, label);
            modLabel.text = ModuleData.Name;
            modLabel.name = "ModuleLabel";

            propertiesContainer = GameObject.Instantiate(ValuesPanelTemplate.Instance.EmptyPrefab, moduleRoot);
            propertiesContainer.gameObject.name = "PropertiesContainer";
            propertiesContainer.anchoredPosition += new Vector2(0, -label.rect.height - (propertiesContainer.rect.height / 2));

            moduleLabel = modLabel;
            propertiesOffset = 0;

        }
    }
}
