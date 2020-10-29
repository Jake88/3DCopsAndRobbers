using UnityEngine;

public class Rotater : MonoBehaviour
{
    [Range(0,2f)] [SerializeField] float _rotateSpeed = .3f;
    [SerializeField] float _enabledDelayTime = 0;

    float _timeUntilRotate;

    void OnEnable()
    {
        _timeUntilRotate = _enabledDelayTime;    
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _timeUntilRotate -= Time.fixedDeltaTime;
        if (_timeUntilRotate > 0) return;

        transform.Rotate(Vector3.up, Time.fixedDeltaTime * (360 * _rotateSpeed));
    }
}
