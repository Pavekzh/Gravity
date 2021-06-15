using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;
using UnityEngine.UI;

public class ShowElement : MonoBehaviour
{
    [SerializeField] GameObject displayObject;
    [SerializeField] bool isDisplayed;
    private void Start()
    {
        if (isDisplayed)
        {
            displayObject.SetActive(true);
        }
        else
        {
            displayObject.SetActive(false);
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
