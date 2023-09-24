using Core.PlayerControl;

namespace Core.Layout.PlayerControl
{
    internal interface IPlayerConstructor : ILayoutPartConstructor
    {
        void Spawn(out Player main, out Player[][] playerGroups);
    }
}