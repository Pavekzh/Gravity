using Assets.SceneEditor.Models;
using System;
using UnityEngine;
using UnityEditor;

namespace Assets.SceneSimulation
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ViewDefinitionModule : Module
    {
        [SerializeField]
        private ViewModuleScriptableObject settingsObject;
        [SerializeField]
        private bool autoUpdate = true;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private IViewModuleData moduleData;

        public bool AutoUpdate { get => autoUpdate; }
        public bool SettingsFoldout { get; set; } = true;
        public ViewModuleScriptableObject SettingsObject { get => settingsObject; set => settingsObject = value; }
        public IViewModuleData ModuleData 
        { 
            get
            {
                if (moduleData != null)
                    return moduleData;
                else
                {
                    this.moduleData = SettingsObject.CreateModuleData();
                    this.moduleData.MeshBinding.ValueChanged += setMesh;
                    this.moduleData.MaterialBinding.ValueChanged += setMaterial;
                    this.moduleData.MeshBinding.ForceUpdate();
                    this.moduleData.MaterialBinding.ForceUpdate();
                    return moduleData;
                }
                    
            }
            set
            {
                if(this.moduleData != null)
                {
                    this.moduleData.MeshBinding.ValueChanged -= setMesh;
                    this.moduleData.MaterialBinding.ValueChanged -= setMaterial;
                }

                this.moduleData = value;
                value.MeshBinding.ValueChanged += setMesh;
                value.MaterialBinding.ValueChanged += setMaterial;
                value.MeshBinding.ForceUpdate();
                value.MaterialBinding.ForceUpdate();
            }
        }

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnDestroy()
        {
            if(moduleData != null)
            {
                moduleData.MeshBinding.ValueChanged -= setMesh;
                moduleData.MaterialBinding.ValueChanged -= setMaterial;
            }
        }

        public void UpdateView()
        {
            SettingsObject.UpdateModule(ModuleData);
        }

        public override ModuleData InstatiateModuleData()
        {
            return ModuleData.GetModuleData();
        }

        private void setMesh(Mesh mesh,object sender)
        {
            if(this.meshFilter != null)
                this.meshFilter.mesh = mesh;
        }

        private void setMaterial(Material material,object value)
        {
            if(this.meshRenderer != null)
                this.meshRenderer.material = material;
        }
    }
}
