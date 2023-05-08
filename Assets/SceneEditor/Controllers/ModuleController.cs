using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.SceneEditor.Models;
using Assets.Services;
using Zenject;

namespace Assets.SceneEditor.Controllers
{
    public class ModuleController
    {
        private RectTransform moduleRoot;
        private RectTransform label;
        private RectTransform propertiesContainer;
        private TMP_Text moduleLabel;

        private List<PropertyController> propertyControllers = new List<PropertyController>();
        private float propertiesOffset;
        private IInstantiator instantiator;
        private ValuesPanelConfig config;

        public ModuleData ModuleData { get; private set; }

        public ModuleController(ModuleData data,ValuesPanelConfig config,IInstantiator instantiator,PropertyControllerFactory propertyFactory)
        {
            this.ModuleData = data;
            this.config = config;
            this.instantiator = instantiator;
            foreach(PropertyViewData pData in data.Properties)
            {
                PropertyController controller = propertyFactory.Create(pData);
                propertyControllers.Add(controller);
            }
        }
    
        public void CreateView(RectTransform parent,ref float modulesOffset)
        {
            if (ModuleData.DisplayOnValuesPanel)
            {
                Init(parent, ref modulesOffset);
                foreach (PropertyController controller in propertyControllers)
                {
                    controller.CreateView(propertiesContainer, ref propertiesOffset);
                }
                modulesOffset += propertiesOffset + config.ModuleMargin;
            }
        } 

        private void Init(RectTransform parent,ref float offset)
        {
            moduleRoot = instantiator.InstantiatePrefabForComponent<RectTransform>(config.EmptyPrefab,parent);
            moduleRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width);
            moduleRoot.anchoredPosition = new Vector2(0, (-moduleRoot.rect.height / 2) - offset);
            moduleRoot.gameObject.name = "ModulePresenter";
            offset = offset + moduleRoot.rect.height;

            label = instantiator.InstantiatePrefabForComponent<RectTransform>(config.ShortViewPrefab, moduleRoot);
            label.gameObject.name = "ShortView";
            TMP_Text modLabel = instantiator.InstantiatePrefabForComponent<TMP_Text>(config.ModuleLabelPrefab, label);
            modLabel.text = ModuleData.Name;
            modLabel.name = "ModuleLabel";

            propertiesContainer = instantiator.InstantiatePrefabForComponent<RectTransform>(config.EmptyPrefab, moduleRoot);
            propertiesContainer.gameObject.name = "PropertiesContainer";
            propertiesContainer.anchoredPosition += new Vector2(0, -label.rect.height - (propertiesContainer.rect.height / 2));

            moduleLabel = modLabel;
            propertiesOffset = 0;

        }
    }
}
