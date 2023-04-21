using System.Collections;
using UnityEngine;

namespace Assets.UIComponents
{
    [CreateAssetMenu(fileName ="ScaleAnimationConfig", menuName ="ScriptableObjects/ScaleAnimation" )]
    public class ScaleAnimationConfig : ScriptableObject
    {
        public float ScaleFactor = 1.1f;
        public float Duration = 0.5f;
        public DG.Tweening.Ease EaseFunction = DG.Tweening.Ease.OutElastic;
    }
}