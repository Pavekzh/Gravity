using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;

public class SaveScene:MonoBehaviour
{
    private SceneState quickSave;

    public void QuickSave()
    {
        quickSave = SceneStateManager.Instance.GetState();
    }
    public void LoadQuickSave()
    {
        if(quickSave != null)
            SceneStateManager.Instance.RefreshScene(quickSave);
    }
}
