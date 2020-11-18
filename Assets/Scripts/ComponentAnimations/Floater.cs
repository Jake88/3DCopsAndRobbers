using UnityEngine;
namespace My.ScriptableAnimation
{
    public class Floater : MonoBehaviour
    {
        const float RADIANS_IN_FULL_CIRCLE = Mathf.PI * 2;

        [SerializeField] FloaterData _floaterData;

        float _timeUntilStart;
        float _sumOfRadiansPerFrame = 0;
        Vector3 _previousFloaterHeight;

        bool _paused;
        float _currentSpeed = 0;

        public void Pause() => _paused = true;
        public void Resume() => _paused = false;

        void OnEnable()
        {
            _timeUntilStart = _floaterData.EnabledDelayTime;
            _currentSpeed = _floaterData.FloatSpeed;
        }

        void FixedUpdate()
        {
            _timeUntilStart -= Time.fixedDeltaTime;
            if (_timeUntilStart > 0) return;

            if (_paused)
            {
                _currentSpeed -= Time.fixedDeltaTime;
                if (_currentSpeed < 0) _currentSpeed = 0;
            }
            else
            {
                if (_currentSpeed != _floaterData.FloatSpeed)
                {
                    var sign = Mathf.Sign(_floaterData.FloatSpeed - _currentSpeed);
                    _currentSpeed += Time.fixedDeltaTime * sign;
                    if (Mathf.Sign(_floaterData.FloatSpeed - _currentSpeed) != sign)
                        _currentSpeed = _floaterData.FloatSpeed;
                }
            }

            var radiansToAddThisFrame = RADIANS_IN_FULL_CIRCLE * Time.fixedDeltaTime;
            _sumOfRadiansPerFrame += radiansToAddThisFrame * _currentSpeed;

            var nextFloaterHeight = Mathf.Sin(_sumOfRadiansPerFrame) * _floaterData.FloatVariance;

            // apply acceleration variance
            /*if (nextFloaterHeight < 0)
                nextFloaterHeight = Mathf.Pow(Mathf.Abs(nextFloaterHeight), _floaterData.FloatAccelerationVariance) * -1;
            else
                nextFloaterHeight = Mathf.Pow(nextFloaterHeight, _floaterData.FloatAccelerationVariance);*/

            var vectorToAlterPositonBy = (Vector3.up * nextFloaterHeight) - _previousFloaterHeight;
            transform.position += vectorToAlterPositonBy;
            _previousFloaterHeight = Vector3.up * nextFloaterHeight;
        }
    }
}