using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;
using BasicTools;

namespace UIExtended
{
    [Serializable]
    public class SelectableFilePresenter : FileItemPresenter
    {
        [SerializeField] FileSelector selector;
        [SerializeField] RectTransform selectorRect;
        [SerializeField] RectTransform fileViewPrefab;
        [SerializeField] RectTransform labelRect;
        [SerializeField] TMP_Text label;
        [SerializeField] [Range(0, 1)] float selectorFill;
        [SerializeField] Vector2 cellSize;

        public Binding<string> PathBinding { get; set; }

        public SelectableFilePresenter() { }

        public SelectableFilePresenter(SelectableFilePresenter source)
        {
            this.selector = source.selector;
            this.selectorRect = source.selectorRect;
            this.fileViewPrefab = source.fileViewPrefab;
            this.labelRect = source.labelRect;
            this.label = source.label;
            this.selectorFill = source.selectorFill;
            this.cellSize = source.cellSize;
            this.PathBinding = source.PathBinding;
        }

        public void Check()
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
                ErrorManager.Instance.ShowErrorMessage("Selector has not set", this);
            }

            if (fileViewPrefab == null)
            {
                ErrorManager.Instance.ShowErrorMessage("FileViewPrefab has not set", this);
            }

            if (label == null)
            {
                ErrorManager.Instance.ShowErrorMessage("LabelPrefab has not set", this);
            }
            if (PathBinding == null)
            {
                ErrorManager.Instance.ShowErrorMessage("SelectSystem has not set", this);
            }
        }

        public override RectTransform GetItemView(RectTransform parent)
        {
            this.selector.FilePath = base.Path;
            label.text = Path;

            selectorRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
            selectorRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * selectorFill);
            selectorRect.anchoredPosition = new Vector2(cellSize.x / 2, -(cellSize.y * selectorFill) / 2);

            labelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
            labelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * (1 - selectorFill));
            labelRect.anchoredPosition = new Vector2(0, labelRect.rect.height / 2);

            RectTransform fileView = GameObject.Instantiate(fileViewPrefab.gameObject,parent).GetComponent<RectTransform>();
            fileView.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
            fileView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y);
            fileView.localScale = Vector3.one;
            fileView.anchoredPosition = new Vector2((cellSize.x / 2), -(parent.rect.height / 2));

            FileSelector selector = GameObject.Instantiate(this.selector.gameObject, fileView.transform).GetComponent<FileSelector>();            
            selector.PathBinding = PathBinding;

            GameObject.Instantiate(label.gameObject, fileView.transform);

            return fileView;
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
