namespace Core.PlayerControl.Core
{
    public interface IStationStateSwitcher
    {
        void SwitchState<T>() where T : BaseState;
    }
}
