using UnityEngine;
using System.Collections;
using Assets.Library;

public class MoveElement : ElementStateChanger
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Vector2 movedPosition;
    [SerializeField] Vector2 defaultPosition;
    [SerializeField] State state = State.Default;

    public override State State { get => State; set => state = value; }
    public override void ChangeState()
    {
        if (state == State.Default)
        {
            rectTransform.anchoredPosition = movedPosition;
            state = State.Changed;
        }
        else if(state == State.Changed)
        {
            rectTransform.anchoredPosition = defaultPosition;
            state = State.Default;
        }
    }
    public override void ChangeState(State state)
    {
        this.state = state;
        if (state == State.Default)
        {
            rectTransform.anchoredPosition = defaultPosition;
        }
        else if(state == State.Changed)
        {
            rectTransform.anchoredPosition = movedPosition;         
        }
        
    }
}
