using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IModulePresenter
{
    RectTransform OpenFullView();
    RectTransform OpenFoldedView();
    void Close();
}
