using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using BasicTools;

namespace Assets.UIExtended
{
    public class SequentialStateChangerList : InitializableStateChanger
    {
        [SerializeField] float sequentialDelay;        
        [SerializeField] bool reverseOnClose = false;
        [SerializeField] InitializableStateChanger[] changers;


        public override State State
        {
            get => state;
            set
            {
                if (!IsInitialized)
                    Initialize(state);

                state = value;
                ChangeState(state);
            }
        }

        private IEnumerator UpdateChangers(State state,bool reverse)
        {
            int start = 0;
            int end = changers.Length;
            int op = 1;
            if (reverse)
            {
                start = changers.Length - 1;
                end = -1;
                op = -1;
            }

            for(int i = start; i != end; i += op)
            {
                changers[i].State = state;
                yield return new WaitForSeconds(sequentialDelay);
            }
        }

        private void ChangeState(State state)
        {
            if (state == State.Default)
                StartCoroutine(UpdateChangers(state, reverseOnClose));
            else if (state == State.Changed)
                StartCoroutine(UpdateChangers(state, false));
        }

        public override void Initialize(State state)
        {
            base.Initialize(state);
            foreach (InitializableStateChanger sChanger in changers)
            {
                sChanger.Initialize(state);
            }
        }
    }
}