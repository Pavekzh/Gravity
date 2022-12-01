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
        [SerializeField] bool defaultInactive = true;

        public override State State
        {
            get { return state; }
            set
            {
                state = value;
                if (value == State.Default)
                {
                    displayObject.SetActive(false == defaultInactive);
                }
                else if (value == State.Changed)
                {
                    displayObject.SetActive(true == defaultInactive);
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            Button button = this.GetComponent<Button>();
            if (button != null && button.onClick.GetPersistentEventCount() == 0)
                button.onClick.AddListener(ChangeState);
        }

        public virtual void Show()
        {
            State = State.Changed;
        }

        public virtual void Hide()
        {
            State = State.Default;
        }
    }

}
