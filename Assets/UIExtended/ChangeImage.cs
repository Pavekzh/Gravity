using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BasicTools;

namespace UIExtended
{
    public class ChangeImage : StateChanger
    {
        [SerializeField] protected Image imageField;
        [SerializeField] protected Sprite defaultState;
        [SerializeField] protected Sprite changedState;

        public override State State
        {
            get { return state; }
            set
            {
                if (value == State.Default)
                {
                    state = State.Default;
                    imageField.sprite = defaultState;
                }
                else
                {
                    state = State.Changed;
                    imageField.sprite = changedState;
                }
            }
        }
    }
}