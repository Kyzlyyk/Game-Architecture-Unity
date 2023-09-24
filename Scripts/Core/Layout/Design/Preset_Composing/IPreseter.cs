namespace Core.Layout.Design.PresetComposing
{
    public interface IPreseter
    {
        int Layer { get; }
        void Apply(Preset preset);
    }
}