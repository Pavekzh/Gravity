using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;

namespace UIExtended
{
    public class CollectionPresenter : MonoBehaviour
    {
        [SerializeField] RectTransform contentContainer;
        [SerializeField] RectTransform visibleArea;
        [SerializeField] [Min(0)] float interval;
        [SerializeField] [Min(0)] float startInterval;

        public virtual IEnumerable<IItemPresenter> Collection { get; set; }

        bool isOpened = false;

        protected virtual float AddItemView(IItemPresenter item,float offset)
        {
            RectTransform view = item.GetItemView(contentContainer);
            view.anchoredPosition = new Vector2(view.anchoredPosition.x + offset, view.anchoredPosition.y);
            return offset + view.rect.width;
        }

        protected virtual void Start()
        {
            if (visibleArea == null)
                visibleArea = this.GetComponent<RectTransform>();
            if (contentContainer == null)
                MessagingSystem.Instance.ShowErrorMessage("Container has not set", this);
        }

        public virtual void ChangeDisplayState()
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

        public virtual void ClosePanel()
        {
            if (isOpened)
            {
                foreach (Transform child in contentContainer.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                isOpened = false;
            }
        }

        public virtual void OpenPanel()
        {
            if (isOpened)
                ClosePanel();
            isOpened = true;

            if(Collection != null)
            {
                float offset = startInterval;
                foreach(IItemPresenter item in Collection)
                {
                    offset = AddItemView(item, offset + interval);
                }


                if (offset < visibleArea.rect.width)
                {
                    contentContainer.anchoredPosition = new Vector2(0, contentContainer.anchoredPosition.y);
                    contentContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, visibleArea.rect.width);
                }
                else
                {
                    contentContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, offset);
                    contentContainer.anchoredPosition = new Vector2(offset / 2, contentContainer.anchoredPosition.y);
                }
            }
        }
    }
}