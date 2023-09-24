using DG.Tweening;
using UnityEngine;

namespace Utilities
{
    public static class GExtensions 
    {
        public static Tween DOShakePosition(this Transform transform, DOShakeArgs args)
        {
            return transform.DOShakePosition(args.Duration, args.Strength, args.Vibrato, args.Randomness, false, args.FadeOut, args.RandomnessMode);
        }
        
        public static Tween DOPunchPosition(this Transform transform, Vector2 punch, DOPunchArgs args)
        {
            return transform.DOPunchPosition(punch, args.Duration, args.Vibrato, args.Elasticity);
        }
    }
}