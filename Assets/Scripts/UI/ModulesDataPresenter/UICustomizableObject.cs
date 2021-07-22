using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface UICustomizableObject
{
    IList<IModulePresenter> Presenters { get; set; }
}
