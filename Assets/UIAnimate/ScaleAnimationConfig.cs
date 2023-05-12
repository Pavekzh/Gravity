using System.Collections;
using UnityEngine;

namespace Assets.UIAnimate
{
    [CreateAssetMenu(fileName ="ScaleAnimationConfig", menuName ="ScriptableObjects/ScaleAnimation" )]
    public class ScaleAnimationConfig : AnimationConfig
    {
        public float ScaleFactor = 1.1f;
    }
}