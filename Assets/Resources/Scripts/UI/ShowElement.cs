using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;

public class ShowElement : MonoBehaviour
{
    [SerializeField] GameObject displayObject;
    [SerializeField] bool isDisplayed;

    void Start()
    {
        if(displayObject == null)
        {
            ErrorManager.Instance.ShowErrorMessage("Displayable object not set");
        }
    }
    public void ChangeState()
    {
        if(isDisplayed == true)
        {
            this.Hide();
        }
        else
        {
            this.Show();
        }

    }    
    public void Show()
    {
        displayObject.SetActive(true);
        isDisplayed = true;
    }
    public void Hide()
    {
        displayObject.SetActive(false);
        isDisplayed = false;
    }
}
