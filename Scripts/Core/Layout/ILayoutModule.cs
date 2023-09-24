using System;

namespace Core.Layout
{
    internal interface ILayoutModule
    {
        Controller Controller { get; }
        void Draw();
    }
    
    internal interface IDelayedLoader : IDisposable
    {
        event EventHandler OnLoaded;
        void StartLoad();
    }
}