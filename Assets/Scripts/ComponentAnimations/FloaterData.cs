using UnityEngine;
namespace My.ScriptableAnimation
{
    [CreateAssetMenu(
        fileName = "Floater_X",
        menuName = AssetMenuConstants.GENERAL + "Floater data"
    )]
    public class FloaterData : ScriptableObject
    {
        [SerializeField] float _floatSpeed = 1f;
        [SerializeField] float _floatVariance = 0.4f;
        [Range(.1f, 2)] [SerializeField] float _floatAccelerationVariance = 1f;

        [SerializeField] float _enabledDelayTime = 0;

        public float FloatSpeed { get => _floatSpeed; }
        public float FloatVariance { get => _floatVariance; }
        public float FloatAccelerationVariance { get => _floatAccelerationVariance; }
        public float EnabledDelayTime { get => _enabledDelayTime; }
    }
}