using Kyzlyk.Helpers;
using System;

namespace Utilities
{
    [Serializable]
    public struct DOPunchArgs
    {
        public DOPunchArgs(OverloadAction @default)
        {
            Duration = 1f;
            Vibrato = 10;
            Elasticity = 1f;
        }

        public float Duration;
        public int Vibrato;
        public float Elasticity;
    }
}