using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    [SerializeField] protected Image imageField;
    [SerializeField] protected Sprite defaultState;
    [SerializeField] protected Sprite changedState;
    [SerializeField] protected bool isDefaultState = true;

    void Start()
    {
        if(imageField != null)
        {
            if (isDefaultState)
            {
                imageField.sprite = defaultState;
            }
            else
            {
                imageField.sprite = changedState;
            }
        }

    }
    public virtual void ChangeState()
    {
        if(imageField != null)
        {
            if (isDefaultState)
            {
                imageField.sprite = changedState;
                isDefaultState = false;
            }
            else
            {
                imageField.sprite = defaultState;
                isDefaultState = true;
            }
        }

    }
    public virtual void ChangeState(bool isDefaultState)
    {
        this.isDefaultState = isDefaultState;
        if(imageField != null)
        {
            if (isDefaultState)
            {
                imageField.sprite = defaultState;
            }
            else
            {
                imageField.sprite = changedState;
            }
        }
    }
}
