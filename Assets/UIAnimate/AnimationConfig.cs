using UnityEditor;
using UnityEngine;

namespace Assets.UIAnimate
{
    [CreateAssetMenu(fileName = "AnimationConfig", menuName = "ScriptableObjects/Animation")]
    public class AnimationConfig : ScriptableObject
    {
        public float Duration = 1;
        public DG.Tweening.Ease EaseFunction = DG.Tweening.Ease.InOutQuad;
    }
}