using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BasicTools;

namespace UIExtended
{
    public class ShowElement : StateChanger
    {
        [SerializeField] GameObject displayObject;
        [SerializeField] State state = State.Default;

        public override State State
        {
            get { return state; }
            set
            {
                state = value;
                if (value == State.Default)
                {
                    displayObject.SetActive(false);
                }
                else if (value == State.Changed)
                {
                    displayObject.SetActive(true);
                }
            }
        }

        private void Start()
        {
            State = state;
        }

        public void Show()
        {
            State = State.Changed;
        }

        public void Hide()
        {
            State = State.Default;
        }
    }

}
