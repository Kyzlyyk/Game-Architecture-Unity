using System;
using UnityEngine;

namespace Core.PlayerControl.Lab.ShockWaves
{
    public class EntityShell : Shell
    {
        public override event EventHandler<Vector2> OnTouched;
    }
}