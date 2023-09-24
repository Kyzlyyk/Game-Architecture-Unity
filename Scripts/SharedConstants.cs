using UnityEngine;

public readonly struct SharedConstants
{
    #region camera settings
    public static readonly float CameraWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
    public static readonly float CameraHeight = Camera.main.orthographicSize * 2f;
    public static readonly float CameraDepth = Camera.main.depth;

    public static Vector2 CameraSize => new(CameraWidth, CameraHeight);
    #endregion
    #region layers
    public const string PlayerLayer = "Player";
    public const string DecorLayer = "Decor";
    public const string BackgroundLayer = "Background";
    public const string GMaterialLayer = "GMaterial";
    public const string UndestroyableGMLayer = "UndestroyableGM";
    public const string ShockWaveLayer = "ShockWave";
    #endregion
    
    #region layers int
    public static readonly int PlayerLayerInt = LayerMask.NameToLayer(PlayerLayer);
    public static readonly int DecorLayerInt = LayerMask.NameToLayer(DecorLayer);
    public static readonly int BackgroundLayerInt = LayerMask.NameToLayer(BackgroundLayer);
    public static readonly int GMaterialLayerInt = LayerMask.NameToLayer(GMaterialLayer);
    public static readonly int UndestroyableGMLayerInt = LayerMask.NameToLayer(UndestroyableGMLayer);
    public static int ShockWaveLayerInt = LayerMask.NameToLayer(ShockWaveLayer);
    #endregion

#if UNITY_EDITOR
    #region scriptables path
    public const string Menu_PlayerControl = "PlayerControl";
    public const string Menu_PlayerControllers = "PlayerControl/Controllers";
    public const string Menu_Details = "PlayerControl/Details";
    public const string Menu_ShockWaves = "PlayerControl/ShockWaves";
    public const string Menu_Layout = "Layout";
    public const string Menu_LayoutControllers = "Layout/Controllers";
    public const string Menu_LayoutConstructors = "Layout/Constructors";
    #endregion
#endif
}