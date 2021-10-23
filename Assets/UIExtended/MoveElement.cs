using BasicTools;
using System;
using UnityEngine;

namespace Assets.UIExtended
{
    public class MoveElement:BasicTools.StateChanger
    {
        [SerializeField] protected RectTransform element;
        [SerializeField] protected Vector2 standardPosition;
        [SerializeField] protected Vector2 changedPosition;

        public override State State 
        { 
            get => this.state;
            set
            {
                state = value;
                if (value == State.Default)
                    element.anchoredPosition = standardPosition;
                else if(value == State.Changed)
                    element.anchoredPosition = changedPosition;
            }
        }
    }
}
