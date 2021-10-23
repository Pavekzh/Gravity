using System.Collections;
using UnityEngine;
using UIExtended;
using Assets.SceneEditor.Models;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class AddPlanetPanel : PanelController
    {
        [SerializeField] string directory = "Planets/";
        [SerializeField] SaveSystemFactory saveSystemFactory;
        [SerializeField] BasicTools.StateChanger visibleManager;
        [SerializeField] DirectoryPresenter directoryPresenter;
        [SerializeField] SelectableFilePresenter filePresenter;

        private ISaveSystem saveSystem;
        private string fileName;

        public BasicTools.Binding<string> FileNameBinding { get; private set; } = new BasicTools.Binding<string>();
        public string Directory { get => Services.SceneStateManager.BaseDirectory + directory; }

        private void Awake()
        {
            if(!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }

            saveSystem = saveSystemFactory.GetSaveSystem();
            FileNameBinding.ValueChanged += LoadPlanet;
            filePresenter.PathBinding = FileNameBinding;
            directoryPresenter.FilePresenter = filePresenter;
            directoryPresenter.Directory = Directory;
            directoryPresenter.FileExtension = saveSystem.Extension;
        }

        private void LoadPlanet(string value, object sender)
        {
            fileName = value;
            PlanetData planetData = saveSystem.Load(Directory + fileName + saveSystem.Extension, typeof(PlanetData)) as PlanetData;
            if (planetData != null)
            {
                PlanetBuildTool buildTool = EditorController.Instance.ToolsController.EnableTool(PlanetBuildTool.StaticKey) as PlanetBuildTool;
                buildTool.Build(planetData);
                Close();
            }
            else
                CommonErrorManager.Instance.ShowErrorMessage("PlanetData didn't load properly", this);
        }

        public override void Close()
        {
            visibleManager.State = BasicTools.State.Default;
            directoryPresenter.ClosePanel();
        }

        public override void Open()
        {
            EditorController.Instance.Panel = this;
            visibleManager.State = BasicTools.State.Changed;
            directoryPresenter.OpenPanel();
        }

        public void ChangeVisibleState()
        {
            if (visibleManager.State == BasicTools.State.Default)
                Open();
            else
                Close();
        }

        public void SavePlanet()
        {
            saveSystem.Save(EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet.PlanetData,Directory + EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet.PlanetData.Name + saveSystem.Extension);
        }
    }
}