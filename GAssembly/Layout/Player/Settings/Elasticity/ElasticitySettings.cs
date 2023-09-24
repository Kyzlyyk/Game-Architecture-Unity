using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Elasticity")]
public class ElasticitySettings : ScriptableObject
{
    [Header("Reflection")]
    [SerializeField] private float _reflectionForce;
    public float ReflectionForce => _reflectionForce;

    [Header("Throw")]
    [SerializeField] private float _throwForce;
    public float ThrowForce => _throwForce;

    [Header("Global")]
    [SerializeField] private float _decelerationTime;
    public float DecelerationTime => _decelerationTime;

    [SerializeField] private ForceMode2D _forceMode;
    public ForceMode2D ForceMode => _forceMode;

    [SerializeField] private LayerMask _reflectiveLayers;
    public LayerMask ReflectiveLayers => _reflectiveLayers;
    
}