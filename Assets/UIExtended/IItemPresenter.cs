using UnityEngine;
using System.Collections;

namespace UIExtended
{
    public interface IItemPresenter
    {
        RectTransform GetItemView(RectTransform parent);
    }
}