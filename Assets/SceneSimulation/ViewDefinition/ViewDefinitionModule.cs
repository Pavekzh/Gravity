using Assets.SceneEditor.Models;
using System;
using UnityEngine;

namespace Assets.SceneSimulation
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ViewDefinitionModule : Module
    {
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        public IViewModuleData ModuleData { get; private set; }

        public void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public override ModuleData InstatiateModuleData()
        {
            PlanetViewModuleData viewModuleData = new PlanetViewModuleData();
            return viewModuleData;
        }
        
        public void SetModuleData(IViewModuleData data)
        {
            this.ModuleData = data;
            data.MeshBinding.ValueChanged += setMesh;
            data.MaterialBinding.ValueChanged += setMaterial;            
            data.MeshBinding.ForceUpdate();
            data.MaterialBinding.ForceUpdate();
        }

        private void setMesh(Mesh mesh,object sender)
        {
            this.meshFilter.mesh = mesh;
        }

        private void setMaterial(Material material,object value)
        {
            this.meshRenderer.material = material;
        }
    }
}
