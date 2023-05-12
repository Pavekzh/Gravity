using System.Collections;
using UnityEngine;
using BasicTools;
using DG.Tweening;

namespace Assets.UIAnimate
{
    public class OpenMoveAnimation : InitializableStateChanger
    {
        [SerializeField] bool changeActiveProperty = true;
        [SerializeField] bool useTargetAsDestination = false;
        [SerializeField] Transform targetPosition;
        [SerializeField] AnimationConfig openConfig;
        [SerializeField] AnimationConfig closeConfig;

        private Vector3 position;

        public override State State 
        {
            get => state;
            set
            {
                if (!IsInitialized)
                    Initialize(state);

                if(value != state)
                {
                    state = value;

                    if (value == State.Default)
                    {
                        Close();
                    }
                    else if (value == State.Changed)
                    {
                        Open();
                    }
                }

            }
        }

        public override void Initialize(State state)
        {
            base.Initialize(state);
            SavePosition();
            if (state == State.Default)
            {
                if(!useTargetAsDestination)
                    transform.position = targetPosition.position;

                AnimationEnd();
            }
            else if (state == State.Changed)
            {
                if (useTargetAsDestination)
                    transform.position = targetPosition.position;
                AnimationStart();
            }

        }

        private void Open()
        {
            if (useTargetAsDestination)
                SavePosition();

            AnimationStart();
            AnimateOpen();
        }

        private void Close()
        {
            if(!useTargetAsDestination)
                SavePosition();

            AnimateClose();
        }

        private void AnimationStart()
        {
            if(changeActiveProperty)
                gameObject.SetActive(true);
        }

        private void AnimationEnd()
        {
            if(changeActiveProperty)
                gameObject.SetActive(false);
        }

        private void AnimateOpen()
        {
            Vector3 destination = position;
            if (useTargetAsDestination)
                destination = targetPosition.position;

            transform.DOMove(destination, openConfig.Duration).SetEase(openConfig.EaseFunction).SetUpdate(true);
        }

        private void AnimateClose()
        {
            Vector3 destination = targetPosition.position;
            if (useTargetAsDestination)
                destination = position;

            transform.DOMove(destination, closeConfig.Duration).SetEase(closeConfig.EaseFunction).SetUpdate(true).OnComplete(AnimationEnd);
        }        
        
        private void SavePosition()
        {
            position = transform.position;
        }
    }
}