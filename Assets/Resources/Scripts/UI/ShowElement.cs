using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;
using UnityEngine.UI;

public class ShowElement : MonoBehaviour
{
    [SerializeField] GameObject displayObject;
    [SerializeField] bool isDisplayed;
    [SerializeField] Image changeImage;
    [SerializeField] Sprite showSprite;
    [SerializeField] Sprite hideSprite;

    void Start()
    {
        if(displayObject == null)
        {
            ErrorManager.Instance.ShowErrorMessage("Displayable object not set");
        }
        if(changeImage != null)
        {
            if (isDisplayed == true)
            {
                Show();
            }
            else
            {
                Hide();
            }
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
        if(changeImage != null)
        {
            changeImage.sprite = hideSprite;
        }
        displayObject.SetActive(true);
        isDisplayed = true;
    }
    public void Hide()
    {
        if (changeImage != null)
        {
            changeImage.sprite = showSprite;
        }
        displayObject.SetActive(false);
        isDisplayed = false;
    }
}
