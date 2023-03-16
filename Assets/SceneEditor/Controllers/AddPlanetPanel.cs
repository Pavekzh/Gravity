using Assets.SceneEditor.Models;
using Assets.Services;
using UIExtended;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace Assets.SceneEditor.Controllers
{
    public class AddPlanetPanel : PanelController
    {
        [SerializeField] SaveSystemFactory saveSystemFactory;
        [SerializeField] BasicTools.StateChanger visibleManager;
        [Header("Presets panel")]        
        [SerializeField] string presetsDirectory = "Presets/Planets/";
        [SerializeField] CollectionPresenter presetsPresenter;
        [SerializeField] SelectableFilePresenter presetFilePresenter;
        [SerializeField] FileNamesCollectionScriptableObject presetsFileNames;
        [Header("User panel")]        
        [SerializeField] string userDirectory = "Planets/";
        [SerializeField] DirectoryPresenter userPlanetsPresenter;
        [SerializeField] SelectableFilePresenter usersFilePresenter;

        private ISaveSystem saveSystem;
        private string selectedFile;
        private PlanetPanelTab panelTab = PlanetPanelTab.PresetsTab;
        
        public enum PlanetPanelTab
        {            
            PresetsTab,
            UserTab
        }
        public BasicTools.Binding<string> userFileBinding { get; private set; } = new BasicTools.Binding<string>();
        public BasicTools.Binding<string> presetBinding { get; private set; } = new BasicTools.Binding<string>();
        public string Directory { get => Services.SceneStateManager.BaseDirectory + userDirectory; }

        private void Awake()
        {

            if(!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }
            saveSystem = saveSystemFactory.GetSaveSystem(); 
            
            //user files settings
            userFileBinding.ValueChanged += LoadPlanet;
            usersFilePresenter.PathBinding = userFileBinding;
            userPlanetsPresenter.FilePresenter = usersFilePresenter;
            userPlanetsPresenter.Directory = Directory;
            userPlanetsPresenter.FileExtension = saveSystem.Extension;

            //loading presets
            presetBinding.ValueChanged += LoadPlanetFromResources;

            List<SelectableFilePresenter> presetPresenterCollection = new List<SelectableFilePresenter>();
            foreach (string name in presetsFileNames.Collection)
            {
                presetPresenterCollection.Add(new SelectableFilePresenter(presetFilePresenter) { Path = name, PathBinding = presetBinding });
            }
            presetsPresenter.Collection = presetPresenterCollection;
        }

        protected override void DoClose()
        {
            visibleManager.State = BasicTools.State.Default;
            userPlanetsPresenter.ClosePanel();
            presetsPresenter.ClosePanel();
        }
        protected override void DoOpen()
        {
            visibleManager.State = BasicTools.State.Changed;
            OpenTab(panelTab);
        }

        public void ChangeVisibleState()
        {
            if (visibleManager.State == BasicTools.State.Default)
                Open();
            else
                Close();
        }


        public void OpenTab(PlanetPanelTab tab)
        {
            this.panelTab = tab;
            switch(tab)
            {
                case PlanetPanelTab.PresetsTab:
                    userPlanetsPresenter.ClosePanel();
                    presetsPresenter.OpenPanel();
                    break;
                case PlanetPanelTab.UserTab:
                    presetsPresenter.ClosePanel();
                    userPlanetsPresenter.OpenPanel();
                    break;
            }
        }

        public void OpenTab(int tab)
        {
            OpenTab((PlanetPanelTab)tab);
        }


        private void BuildPlanet(PlanetData pData)
        {
            if(pData != null)
            {
                this.RestorablePanel = null;
                PlanetBuildTool buildTool = EditorController.Instance.ToolsController.EnableTool(PlanetBuildTool.StaticKey) as PlanetBuildTool;
                buildTool.Build(pData);                
                Close();

            }
        }

        private void LoadPlanetFromResources(string name,object sender)
        {
            string path = presetsDirectory + name;
            try
            {
                TextAsset textAsset = Resources.Load<TextAsset>(path);
                using (Stream stream = new MemoryStream(textAsset.bytes))
                {
                    PlanetData pData = (PlanetData)saveSystem.Load(stream, typeof(PlanetData));
                    BuildPlanet(pData);
                }
            }
            catch(Exception ex)
            {
                BasicTools.MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }

        }

        private void LoadPlanet(string path, object sender)
        {
            selectedFile = path;
            PlanetData planetData = saveSystem.Load(Directory + selectedFile + saveSystem.Extension, typeof(PlanetData)) as PlanetData;
            if (planetData != null)
            {
                BuildPlanet(planetData);
            }
            else
                CommonMessagingSystem.Instance.ShowErrorMessage("PlanetData didn't load properly", this);
        }

        public void SavePlanet()
        {
            saveSystem.Save(Services.PlanetSelectSystem.Instance.SelectedPlanet.PlanetData,Directory + Services.PlanetSelectSystem.Instance.SelectedPlanet.PlanetData.Name + saveSystem.Extension);
            this.Close();
            this.Open();
        }
    }
}