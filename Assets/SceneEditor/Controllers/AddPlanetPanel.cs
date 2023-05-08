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

        private ISaveSystem SaveSystem { get => saveSystemFactory.GetChachedSaveSystem(); }
        private string selectedFile;
        private PlanetPanelTab panelTab = PlanetPanelTab.PresetsTab;
        private PlanetSelector selector;

        public enum PlanetPanelTab
        {            
            PresetsTab,
            UserTab
        }
        public BasicTools.Binding<string> userFileBinding { get; private set; } = new BasicTools.Binding<string>();
        public BasicTools.Binding<string> presetBinding { get; private set; } = new BasicTools.Binding<string>();
        public string UserDirectory { get => Services.SceneStateLoader.BaseDirectory + userDirectory; }

        [Zenject.Inject]
        private void Construct(PlanetSelector selector)
        {
            this.selector = selector;
        }

        private void Awake()
        {

            if(!System.IO.Directory.Exists(UserDirectory))
            {
                System.IO.Directory.CreateDirectory(UserDirectory);
            }
            
            //user files settings
            userFileBinding.ValueChanged += LoadPlanet;
            usersFilePresenter.PathBinding = userFileBinding;
            userPlanetsPresenter.FilePresenter = usersFilePresenter;
            userPlanetsPresenter.Directory = UserDirectory;
            userPlanetsPresenter.FileExtension = SaveSystem.Extension;

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
                PlanetBuildTool buildTool = editor.ToolsController.EnableTool(PlanetBuildTool.StaticKey) as PlanetBuildTool;
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
                    PlanetData pData = (PlanetData)SaveSystem.Load(stream, typeof(PlanetData));
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
            PlanetData planetData = SaveSystem.Load(UserDirectory + selectedFile + SaveSystem.Extension, typeof(PlanetData)) as PlanetData;
            if (planetData != null)
            {
                BuildPlanet(planetData);
            }
            else
                CommonMessagingSystem.Instance.ShowErrorMessage("PlanetData didn't load properly", this);
        }

        public void SavePlanet()
        {
            SaveSystem.Save(selector.SelectedPlanet.PlanetData,UserDirectory + selector.SelectedPlanet.PlanetData.Name + SaveSystem.Extension);
            this.Close();
            this.Open();
        }
    }
}