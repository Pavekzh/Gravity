using System;
using BasicTools;
using Assets.SceneSimulation;
using UnityEngine;

namespace Assets.SceneEditor.Models
{
    public interface IViewModuleData
    {
        Binding<Mesh> MeshBinding { get; }
        Binding<Material> MaterialBinding { get; }
        Binding<float> ScaleBinding { get; }

        void UpdateView();
        ModuleData GetModuleData();
    }
}
