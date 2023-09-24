using DG.Tweening;
using Kyzlyk.Helpers;
using System;

namespace Utilities
{
    [Serializable]
    public struct DOShakeArgs
    {
        public DOShakeArgs(OverloadAction @default)
        {
            Duration = 1f;
            Strength = 1f;
            Vibrato = 10;
            Randomness = 90f;
            FadeOut = true;
            RandomnessMode = ShakeRandomnessMode.Full;
        }

        public float Duration;
        public float Strength;
        public int Vibrato;
        public float Randomness;
        public bool FadeOut;
        public ShakeRandomnessMode RandomnessMode;
    }
}