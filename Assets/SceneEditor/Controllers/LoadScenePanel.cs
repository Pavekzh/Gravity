using System.Collections;
using UnityEngine;
using UIExtended;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class LoadScenePanel : PanelController
    {

        [SerializeField] private SelectableFilePresenter filePresenter;
        [SerializeField] private DirectoryPresenter directoryPresenter;
        [SerializeField] private ShowElement visibleManager;

        public BasicTools.Binding<string> FileNameBinding { get; private set; } = new BasicTools.Binding<string>();

        private string fileName;

        private void Awake()
        {
            FileNameBinding.ValueChanged += SelectPath;
            directoryPresenter.Directory = Services.SceneStateManager.Instance.Directory;
            directoryPresenter.FileExtension = Services.SceneStateManager.Instance.Extension;
            directoryPresenter.FilePresenter = this.filePresenter;
            filePresenter.PathBinding = FileNameBinding;
        }

        public void SelectPath(string path,object sender)
        {
            fileName = path;
        }

        public void Load()
        {
            if(fileName != "")
            {
                Assets.Services.SceneStateManager.Instance.Load(fileName);
                this.RestorablePanel = null;
                Close();
            }
        }

        protected override void DoOpen()
        {
            visibleManager.Show();
            directoryPresenter.OpenPanel();
        }

        protected override void DoClose()
        {
            directoryPresenter.ClosePanel();
            visibleManager.Hide();
        }

        public void Delete() 
        {
            Services.SceneStateManager.Instance.Delete(fileName);
            directoryPresenter.OpenPanel();
        }
    }
}