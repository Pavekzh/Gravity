using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine.UI;
using BasicTools;

namespace UIExtended
{
    public class DirectoryPresenter : MonoBehaviour
    {
        [SerializeField] FilePresenter filePresenter;
        [SerializeField] RectTransform container;
        [SerializeField] RectTransform panelRect;
        [SerializeField] Vector2 fileViewSize;
        [SerializeField] float margin;
        [SerializeField] [Min(0)] float startMargin;

        public string Directory { get; set; }
        public string FileExtension { get; set; }
        bool isOpened = false;

        private void Start()
        {
            if (panelRect == null)
                panelRect = this.GetComponent<RectTransform>();
            if (filePresenter == null)
                ErrorManager.Instance.ShowErrorMessage("FilePresenter has not set", this);
            if (container == null)
                ErrorManager.Instance.ShowErrorMessage("Container has not set", this);
        }

        private void AddFileView(string filePath, float offset)
        {
            RectTransform fileView = this.filePresenter.GetFileView(filePath, fileViewSize);
            fileView.parent = container;
            fileView.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fileViewSize.x);
            fileView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fileViewSize.y);
            fileView.localScale = Vector3.one;
            fileView.anchoredPosition = new Vector2((fileViewSize.x / 2) + offset, -(container.rect.height / 2));
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
            foreach (Transform child in container.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            isOpened = false;
        }

        public void OpenPanel()
        {
            if (isOpened)
            {
                ClosePanel();
            }
            isOpened = true;
            string[] files = null;
            try
            {
                files = System.IO.Directory.GetFiles(Directory, "*" + FileExtension);
            }
            catch (IOException ex)
            {
                ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
            }
            if (files != null && files.Length != 0)
            {
                float width = (files.Length * fileViewSize.x) + ((files.Length - 1) * margin);
                container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                if (width < panelRect.rect.width)
                {
                    container.anchoredPosition = new Vector2(0, container.anchoredPosition.y);
                    container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelRect.rect.width);
                }
                else
                {
                    container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                    container.anchoredPosition = new Vector2(width / 2, container.anchoredPosition.y);
                }

                float offset = startMargin;
                for (int i = 0; i < files.Length; i++)
                {
                    AddFileView(files[i], offset);
                    offset += fileViewSize.x + margin;
                }
            }

        }
    }

}


