using UnityEngine;
using System.Collections;

namespace UIExtended
{
    public class CustomizableObjectPresenter : MonoBehaviour
    {
        [SerializeField] RectTransform container;
        [SerializeField] float margin;

        UICustomizableObject customizableObject;

        public void Open(UICustomizableObject customizableObject)
        {
            Close();
            if (customizableObject != null)
            {
                float offset = 0;
                foreach (IModulePresenter presenter in customizableObject.ModulePresenters)
                {
                    offset -= AddModulePresenter(presenter, offset) + margin;
                }
                this.customizableObject = customizableObject;
            }

        }

        public void Close()
        {
            if (customizableObject != null)
            {
                foreach (IModulePresenter presenter in customizableObject.ModulePresenters)
                {
                    presenter.Close();
                }
            }
        }

        private float AddModulePresenter(IModulePresenter presenter, float offset)
        {
            RectTransform view = presenter.OpenFullView();
            view.transform.SetParent(container.transform);
            view.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.rect.width);
            view.localScale = Vector2.one;
            view.anchoredPosition = new Vector2(0, (-view.rect.height / 2) + offset);
            return offset + view.rect.height;
        }


    }
}