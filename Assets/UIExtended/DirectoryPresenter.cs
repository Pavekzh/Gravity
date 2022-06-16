using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using TMPro;
using UnityEngine.UI;
using BasicTools;

namespace UIExtended
{
    [RequireComponent(typeof(CollectionPresenter))]
    public class DirectoryPresenter : MonoBehaviour
    {
        [SerializeField] private CollectionPresenter collectionPresenter;    

        public FileItemPresenter FilePresenter { get; set; }
        public string Directory { get; set; }
        public string FileExtension { get; set; }

        bool isOpened = false;

        private void Awake()
        {
            if (collectionPresenter == null)
            {
                Debug.Log("CollectionPresenter not set, searching instance");
                collectionPresenter = GetComponent<CollectionPresenter>();
            }
        }

        public void ChangeDisplayState()
        {
            if (isOpened)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }

        public void ClosePanel()
        {
            if (isOpened)
            {
                collectionPresenter.ClosePanel();
                isOpened = false;
            }
        }

        public void OpenPanel()
        {
            if (isOpened)
                ClosePanel();
            isOpened = true;

            string[] files = null;
            try
            {
                files = System.IO.Directory.GetFiles(Directory, "*" + FileExtension);
            }
            catch (IOException ex)
            {
                MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }

            List<FileItemPresenter> itemsList = new List<FileItemPresenter>();
            foreach(string path in files)
            {
                FileItemPresenter fPresenter = FilePresenter.Clone() as FileItemPresenter;
                fPresenter.Path = Path.GetFileNameWithoutExtension(path);
                itemsList.Add(fPresenter);
            }

            collectionPresenter.Collection = itemsList;
            collectionPresenter.OpenPanel();
        }
    }

}


