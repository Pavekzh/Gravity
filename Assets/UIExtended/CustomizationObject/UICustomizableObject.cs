using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIExtended 
{
    public interface UICustomizableObject
    {
        IList<IModulePresenter> ModulePresenters { get; set; }
    }

}

