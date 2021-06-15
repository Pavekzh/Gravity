using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine.UI;


public class SelectFilePanel : MonoBehaviour
{
    public delegate void SelectedFileChangedHandler(object sender);
    public event SelectedFileChangedHandler SelectedFileChanged;
    [SerializeField] SaveSystem saveSystem;
    [SerializeField] RectTransform container;
    [SerializeField] string savesDirectory;
    [SerializeField] string extension;
    [SerializeField] float imageBoxSize;
    [SerializeField] float margin;
    [SerializeField] Sprite fileImage;


    [SerializeField] GameObject fileViewPrefab;
    [SerializeField] GameObject imagePrefab;
    [SerializeField] GameObject labelPrefab;

    private string selectedFile;

    public string SavesDirectory
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath + savesDirectory;
#elif UNITY_ANDROID || UNITY_IOS
            return Application.persistentDataPath + savesDirectory;
#endif
        }
    }
    private void Start()
    {
        if(fileViewPrefab == null)
        {
            ErrorManager.Instance.ShowErrorMessage("FileViewPrefab not set",this);
        }
        if(fileViewPrefab.GetComponent<RectTransform>() == null)
        {
            ErrorManager.Instance.ShowErrorMessage("FileViewPrefab must contain RectTransform component",this);
        }
        if(imagePrefab == null)
        {
            ErrorManager.Instance.ShowErrorMessage("ImagePrefab not set",this);
        }
        if(imagePrefab.GetComponent<Image>() == null)
        {
            ErrorManager.Instance.ShowErrorMessage("ImagePrefab must contain Image component",this);
        }
        if(imagePrefab.GetComponent<FileSelect>() == null)
        {
            ErrorManager.Instance.ShowErrorMessage("ImagePrefab must contain FileSelect component",this);
        }
        if (labelPrefab == null)
        {
            ErrorManager.Instance.ShowErrorMessage("LabelPrefab not set",this);
        }
        if (labelPrefab.GetComponent<TMP_Text>() == null)
        {
            ErrorManager.Instance.ShowErrorMessage("LabelPrefab must contain TMP_Text component",this);
        }
    }
    public void SelectFile(string filePath,object sender)
    {
        this.selectedFile = filePath;
        SelectedFileChanged?.Invoke(sender);
    }
    public void OpenPanel()
    {
        string[] files = null;
        try
        {
            files = Directory.GetFiles(SavesDirectory, "*" + extension);
        }
        catch(IOException ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message,this);
        }
        if(files != null && files.Length != 0)
        {
            float width = (files.Length * imageBoxSize) + ((files.Length - 1) * margin);
            container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            RectTransform panelRect = this.GetComponent<RectTransform>();
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
                AddFileView(files[i], panelRect, offset);
                offset += imageBoxSize + margin;
            }         
            selectedFile = files[0];
        }

    }
    private void AddFileView(string filePath,RectTransform panelRectTransform,float offset)
    {
        GameObject fileView = GameObject.Instantiate(fileViewPrefab, container);
        GameObject imageObj = GameObject.Instantiate(imagePrefab, fileView.transform);
        GameObject labelObj = GameObject.Instantiate(labelPrefab, fileView.transform);

        Image image = imageObj.GetComponent<Image>();
        TMP_Text label = labelObj.GetComponent<TMP_Text>();
        RectTransform rectTransfrom = fileView.GetComponent<RectTransform>();
        FileSelect fileSelect = imageObj.GetComponent<FileSelect>();

        fileSelect.FilePath = filePath;
        fileSelect.Panel = this;
        image.sprite = fileImage;
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imageBoxSize);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imageBoxSize);
        label.text = filePath.Replace($"{SavesDirectory}/", "").Replace($"{extension}", "");
        rectTransfrom.anchoredPosition = new Vector2((imageBoxSize / 2) + offset, -(panelRectTransform.rect.height / 2));
    }
    public void ClosePanel()
    {
        foreach(Transform child in container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void LoadFile()
    {
        if(selectedFile != null)
        {
            saveSystem.LoadFromFile(selectedFile);      
        }
    }
    public void Reload()
    {
        ClosePanel();
        OpenPanel();
    }
    public void DeleteFile()
    {
        try
        {
            File.Delete(selectedFile);
        }
        catch(System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message,this);
        }
        Reload();
    }
}