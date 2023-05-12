using UnityEngine;

namespace Assets.UIAnimate
{
    [CreateAssetMenu(fileName ="MoveAnimationConfig", menuName ="ScriptableObjects/MoveAnimation" )]
    public class MoveAnimationConfig : AnimationConfig
    {
        public Vector3 Point = Vector3.zero;
    }
}