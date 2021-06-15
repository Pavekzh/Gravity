using UnityEngine;
using System.Collections;
using TMPro;

public class FileSelect : MonoBehaviour
{
    private SelectFilePanel panel;
    public SelectFilePanel Panel
    {
        get => panel;
        set
        {
            if(panel != null)
                panel.SelectedFileChanged -= SelectedFileChanged;

            panel = value;
            if(panel != null)
                panel.SelectedFileChanged += SelectedFileChanged;
        }
    }
    [SerializeField]private ChangeImage changeImage;
    public string FilePath { get; set; }

        
    public void Select()
    {
        Panel.SelectFile(FilePath,this);
    }
    public void SelectedFileChanged(object sender)
    {
        if(sender != (object)this)
        {
            changeImage.ChangeState(true);
        }
    }
}
