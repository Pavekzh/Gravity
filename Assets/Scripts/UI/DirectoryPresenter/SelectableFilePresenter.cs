using UnityEngine;
using System.Collections;
using Assets.Library;
using TMPro;
using UnityEngine.UI;

public class SelectableFilePresenter : FilePresenter
{

    [SerializeField] SelectFilePath selectSystem;
    [SerializeField] FileSelector selector;
    [SerializeField] RectTransform selectorRect;
    [SerializeField] RectTransform fileViewPrefab;
    [SerializeField] RectTransform labelRect;
    [SerializeField] TMP_Text label;
    [SerializeField][Range(0,1)] float selectorFill;

    public SelectFilePath SelectSystem { get => selectSystem; set => selectSystem = value; }

    private void Start()
    {
        if (labelRect == null)
        {
            ErrorManager.Instance.ShowErrorMessage("LabelRect has not set", this);
        }
        if (selectorRect == null)
        {
            ErrorManager.Instance.ShowErrorMessage("SelectorRect has not set", this);
        }
        if (selector == null)
        {
            ErrorManager.Instance.ShowErrorMessage("Selector has not set",this);
        }

        if (fileViewPrefab == null)
        {
            ErrorManager.Instance.ShowErrorMessage("FileViewPrefab has not set", this);
        }

        if (label == null)
        {
            ErrorManager.Instance.ShowErrorMessage("LabelPrefab has not set", this);
        }
        if(SelectSystem == null)
        {
            ErrorManager.Instance.ShowErrorMessage("SelectSystem has not set",this);
        }
    }
    public override RectTransform GetFileView(string path,Vector2 cellSize)
    {
        selector.FilePath = path;
        selector.SelectSystem = SelectSystem;
        label.text = System.IO.Path.GetFileNameWithoutExtension(path);

        selectorRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
        selectorRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * selectorFill);
        selectorRect.anchoredPosition = new Vector2(cellSize.x / 2,-(cellSize.y * selectorFill)/2);

        labelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
        labelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * (1 - selectorFill));
        labelRect.anchoredPosition = new Vector2(0,labelRect.rect.height / 2);

        GameObject fileView = GameObject.Instantiate(fileViewPrefab.gameObject);
        GameObject selectObj = GameObject.Instantiate(selector.gameObject,fileView.transform);
        GameObject labelObj = GameObject.Instantiate(label.gameObject,fileView.transform);

        return fileView.GetComponent<RectTransform>();
    }
}
