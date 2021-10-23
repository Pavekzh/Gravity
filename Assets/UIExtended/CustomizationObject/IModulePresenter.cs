using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UIExtended
{
    public interface IModulePresenter
    {
        RectTransform OpenFullView();
        RectTransform OpenFoldedView();
        void Close();
    }
}