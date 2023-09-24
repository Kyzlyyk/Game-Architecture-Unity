using System;

namespace Core.PlayerControl
{
    [Serializable]
    internal struct PlayerProperties
    {
        public byte Difficulty;
        public byte Count;
        public byte RareRangeStart;
        public byte RareRangeEnd;
    }
}