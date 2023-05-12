using BasicTools;
using DG.Tweening;
using UnityEngine;

namespace Assets.UIAnimate
{
    public class OpenExpandAnimation : InitializableStateChanger
    {
        [SerializeField] ScaleAnimationConfig openScaleAnimation;
        [SerializeField] ScaleAnimationConfig closeScaleAnimation;

        private Vector3 scale;


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
            if (state == State.Default)
                gameObject.SetActive(false);
            else if (state == State.Changed)
                gameObject.SetActive(true);
        }

        private void Open()
        {
            InitAnimation();
            StartAnimation();

            AnimateOpen();
        }

        private void Close()
        {
            InitAnimation();

            AniamteClose();
        }

        private void AnimateOpen()
        {
            transform.localScale = scale * openScaleAnimation.ScaleFactor;
            transform.DOScaleX(scale.x, openScaleAnimation.Duration).SetEase(openScaleAnimation.EaseFunction).SetUpdate(true);
            transform.DOScaleY(scale.y, openScaleAnimation.Duration).SetEase(openScaleAnimation.EaseFunction).SetUpdate(true);
            transform.DOScaleZ(scale.z, openScaleAnimation.Duration).SetEase(openScaleAnimation.EaseFunction).SetUpdate(true);
        }

        private void AniamteClose()
        {
            Vector3 resultScale = scale * closeScaleAnimation.ScaleFactor;
            transform.DOScaleX(resultScale.x, closeScaleAnimation.Duration).SetEase(closeScaleAnimation.EaseFunction).SetUpdate(true);
            transform.DOScaleY(resultScale.y, closeScaleAnimation.Duration).SetEase(closeScaleAnimation.EaseFunction).SetUpdate(true);
            transform.DOScaleZ(resultScale.z, closeScaleAnimation.Duration).SetEase(closeScaleAnimation.EaseFunction).SetUpdate(true).OnComplete(EndAnimation);
        }



        private void InitAnimation()
        {
            scale = transform.localScale;
        }

        private void StartAnimation()
        {
            gameObject.SetActive(true);
        }

        private void EndAnimation()
        {
            gameObject.SetActive(false);

            transform.localScale = scale;
        }
    }
}