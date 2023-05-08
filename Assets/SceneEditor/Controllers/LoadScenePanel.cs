using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIExtended;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class LoadScenePanel : PanelController
    {
        [SerializeField] private ShowElement visibleManager;

        [Header("Users scenes")]
        [SerializeField] private DirectoryPresenter userTabPresenter;
        [SerializeField] private SelectableFilePresenter usersFilePresenter;
        [Header("Presets scenes")]
        [SerializeField] private CollectionPresenter presetsTabPresenter;
        [SerializeField] private SelectableFilePresenter presetsFilePresenter;
        [SerializeField] private FileNamesCollectionScriptableObject fileNames;


        public enum LoadSceneTab
        {
            Presets,
            User
        }

        public BasicTools.Binding<string> userFileNameBinding { get; private set; } = new BasicTools.Binding<string>();
        public BasicTools.Binding<string> presetsFileNameBinding { get; private set; } = new BasicTools.Binding<string>();

        private LoadSceneTab tab = LoadSceneTab.Presets;
        private string fileName;
        private bool loadFromResources;
        private SceneStateLoader sceneLoader;

        [Zenject.Inject]
        private void Construct(SceneStateLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            userFileNameBinding.ValueChanged += SelectUsersPath;
            userTabPresenter.Directory = sceneLoader.Directory;
            userTabPresenter.FileExtension = sceneLoader.Extension;
            userTabPresenter.FilePresenter = this.usersFilePresenter;
            usersFilePresenter.PathBinding = userFileNameBinding;

            List<SelectableFilePresenter> presetsPresenterCollection = new List<SelectableFilePresenter>();
            presetsFileNameBinding.ValueChanged += SelectPresetsPath;
            foreach(string name in fileNames.Collection)
            {
                presetsPresenterCollection.Add(new SelectableFilePresenter(presetsFilePresenter) { Path = name,PathBinding = presetsFileNameBinding });
            }
            presetsTabPresenter.Collection = presetsPresenterCollection;
        }


        protected override void DoOpen()
        {
            visibleManager.Show();
            OpenTab(tab);
        }

        protected override void DoClose()
        {
            userTabPresenter.ClosePanel();
            presetsTabPresenter.ClosePanel();
            visibleManager.Hide();
        }

        public void OpenTab(LoadSceneTab tab)
        {
            this.tab = tab;
            switch (tab)
            {
                case LoadSceneTab.Presets:
                    userTabPresenter.ClosePanel();
                    presetsTabPresenter.OpenPanel();
                    break;
                case LoadSceneTab.User:
                    userTabPresenter.OpenPanel();
                    presetsTabPresenter.ClosePanel();
                    break;
            }
        }

        public void OpenTab(int tab)
        {
            OpenTab((LoadSceneTab)tab);
        }

        public void SelectPresetsPath(string path, object sender)
        {
            fileName = path;
            loadFromResources = true;
        }

        public void SelectUsersPath(string path,object sender)
        {
            fileName = path;
            loadFromResources = false;
        }

        public void Load()
        {
            if(fileName != "")
            {
                if (loadFromResources)
                    sceneLoader.LoadPreset(fileName);
                else
                    sceneLoader.Load(fileName);

                PanelController panelBeforeMenu = RestorablePanel.RestorablePanel;                
                this.RestorablePanel.RestorablePanel = null;
                this.RestorablePanel = panelBeforeMenu;
                
                Close();
            }
        }

        public void Delete() 
        {
            sceneLoader.Delete(fileName);
            userTabPresenter.OpenPanel();
        }
    }
}