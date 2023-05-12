using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

namespace Assets.UIAnimate
{
    public class ClickScaleAnimation : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] bool useScriptableConfig = true;
        [SerializeField] ScaleAnimationConfig animationConfig;
        [SerializeField] float scaleFactor = 1.1f;
        [SerializeField] float duration = 0.5f;
        [SerializeField] Ease easeFunction = Ease.OutElastic;

        private Vector3 scale;

        private void Awake()
        {
            scale = transform.localScale;
            if(!useScriptableConfig)
            {
                animationConfig = new ScaleAnimationConfig();
                animationConfig.ScaleFactor = scaleFactor;
                animationConfig.Duration = duration;
                animationConfig.EaseFunction = easeFunction;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Animate();
        }

        private void Animate()
        {
            transform.localScale = scale * animationConfig.ScaleFactor;
            transform.DOScaleX(scale.x, animationConfig.Duration).SetEase(animationConfig.EaseFunction).SetUpdate(true);
            transform.DOScaleY(scale.y, animationConfig.Duration).SetEase(animationConfig.EaseFunction).SetUpdate(true);
            transform.DOScaleZ(scale.z, animationConfig.Duration).SetEase(animationConfig.EaseFunction).SetUpdate(true);
        }
    }
}