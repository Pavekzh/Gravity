using Assets.SceneEditor.Models;
using System;
using UnityEngine;
using UnityEditor;

namespace Assets.SceneSimulation
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ViewDefinitionModule : Module
    {
        [SerializeField]
        private ViewModuleScriptableObject settingsObject;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private IViewModuleData moduleData;

        public ViewModuleScriptableObject SettingsObject { get => settingsObject; set => settingsObject = value; }
        public bool SettingsFoldout { get; set; }
        public IViewModuleData ModuleData 
        { 
            get => moduleData;
            set
            {
                this.moduleData = value;
                value.MeshBinding.ValueChanged += setMesh;
                value.MaterialBinding.ValueChanged += setMaterial;
                value.MeshBinding.ForceUpdate();
                value.MaterialBinding.ForceUpdate();
            }
        }

        public void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public override ModuleData InstatiateModuleData()
        {
            return settingsObject.CreateModuleData().GetModuleData();
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
