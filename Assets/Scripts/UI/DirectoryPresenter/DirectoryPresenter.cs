using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine.UI;
using Assets.Library;

public class DirectoryPresenter : MonoBehaviour
{
    [SerializeField] FilePresenter filePresenter;
    [SerializeField] RectTransform container;
    [SerializeField] RectTransform panelRect; 
    [SerializeField] Vector2 fileViewSize;
    [SerializeField] float margin;

    bool isOpened = false;
    private void Start()
    {
        if(panelRect == null)
            panelRect = this.GetComponent<RectTransform>();
        if (filePresenter == null)
            ErrorManager.Instance.ShowErrorMessage("FilePresenter has not set",this);
        if (container == null)
            ErrorManager.Instance.ShowErrorMessage("Container has not set",this);
    }
    private void AddFileView(string filePath,RectTransform container,float offset)
    {
        RectTransform fileView = this.filePresenter.GetFileView(filePath,fileViewSize);
        fileView.parent = container;
        fileView.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,fileViewSize.x);
        fileView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fileViewSize.y);
        fileView.anchoredPosition = new Vector2((fileViewSize.x / 2) + offset, -(container.rect.height / 2));
    }

    public void ClosePanel()
    {
        foreach(Transform child in container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        isOpened = false;
    }
    public void OpenPanel(string directory,string filesExtension)
    {
        if (isOpened)
        {
            ClosePanel();
        }
        isOpened = true;
        string[] files = null;
        try
        {
            files = Directory.GetFiles(directory, "*" + filesExtension);
        }
        catch(IOException ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message,this);
        }
        if(files != null && files.Length != 0)
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

            float offset = 0;
            for (int i = 0; i < files.Length; i++)
            {
                AddFileView(files[i], container, offset);
                offset += fileViewSize.x + margin;
            }         
        }

    }
}