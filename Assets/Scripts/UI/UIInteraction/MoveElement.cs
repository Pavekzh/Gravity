using UnityEngine;
using System.Collections;
using Assets.Library;

public class MoveElement : StateChanger
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Vector2 movedPosition;
    [SerializeField] Vector2 defaultPosition;
    [SerializeField] State state = State.Default;

    public override State State
    {
        get { return state; }
        set
        {
            state = value;
            if (value == State.Default)
            {
                rectTransform.anchoredPosition = defaultPosition;
            }
            else if (value == State.Changed)
            {
                rectTransform.anchoredPosition = movedPosition;
            }
        }
    }
}
