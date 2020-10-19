using UnityEngine;

public class Floater : MonoBehaviour
{
    const float RADIANS_IN_FULL_CIRCLE = Mathf.PI * 2;
    [SerializeField] FloaterData _floaterData;
    
    [SerializeField] float _maxHeight = 0;
    [SerializeField] float _minHeight = 0;

    float _timeUntilStart;

    float _sumOfRadiansPerFrame = 0;
    Vector3 _previousFloaterHeight;

    void OnEnable()
    {
        _timeUntilStart = _floaterData.EnabledDelayTime;
    }

    void FixedUpdate()
    {
        _timeUntilStart -= Time.fixedDeltaTime;
        if (_timeUntilStart > 0) return;

        var radiansToAddThisFrame = RADIANS_IN_FULL_CIRCLE * Time.fixedDeltaTime;
        //var radiansManipulatedForExageratedEffect = (radiansToAddThisFrame * Mathf.Abs(Mathf.Sin(_sumOfRadiansPerFrame + radiansToAddThisFrame)) + 1) / 2;
        _sumOfRadiansPerFrame += radiansToAddThisFrame * _floaterData.FloatSpeed;

        var nextFloaterHeight = Mathf.Sin(_sumOfRadiansPerFrame) * _floaterData.FloatVariance;

        var vectorToAlterPositonBy = (Vector3.up * nextFloaterHeight) - _previousFloaterHeight;
        transform.position += vectorToAlterPositonBy;
        _previousFloaterHeight = Vector3.up * nextFloaterHeight;


        if (transform.position.y > _maxHeight) _maxHeight = transform.position.y;
        if (transform.position.y < _minHeight) _minHeight = transform.position.y;

        Debug.DrawLine(new Vector3(0, _maxHeight, 0), new Vector3(0, _minHeight, 0), Color.red);
    }
}
