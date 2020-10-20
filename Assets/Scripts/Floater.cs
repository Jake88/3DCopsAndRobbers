using UnityEngine;

public class Floater : MonoBehaviour
{
    const float RADIANS_IN_FULL_CIRCLE = Mathf.PI * 2;
    [SerializeField] FloaterData _floaterData;
    
    [SerializeField] float _maxHeight = 0;
    [SerializeField] float _minHeight = 0;

    float _timeUntilStart;

    float tempTimer;
    int frameCounter;
    string s = "vectorToAlterPositionBy: ";
    string d = "y Position updates: ";

    float _sumOfRadiansPerFrame = 0;
    Vector3 _previousFloaterHeight;

    void OnEnable()
    {
        _timeUntilStart = _floaterData.EnabledDelayTime;
    }

    void FixedUpdate()
    {

        if (tempTimer >= 1 && this.gameObject.name == "THING")
        {
            print($"Ran code {frameCounter} times");
            print(s);
            print(d);
            this.enabled = false;
        }

        _timeUntilStart -= Time.fixedDeltaTime;
        if (_timeUntilStart > 0) return;

        tempTimer += Time.fixedDeltaTime;
        frameCounter++;

        var radiansToAddThisFrame = RADIANS_IN_FULL_CIRCLE * Time.fixedDeltaTime;
        //var radiansManipulatedForExageratedEffect = (radiansToAddThisFrame * Mathf.Abs(Mathf.Sin(_sumOfRadiansPerFrame + radiansToAddThisFrame)) + 1) / 2;
        _sumOfRadiansPerFrame += radiansToAddThisFrame * _floaterData.FloatSpeed;

        var nextFloaterHeight = Mathf.Sin(_sumOfRadiansPerFrame) * _floaterData.FloatVariance;

        var vectorToAlterPositonBy = (Vector3.up * nextFloaterHeight) - _previousFloaterHeight;
        // s += $"{vectorToAlterPositonBy.y}, ";
        transform.position += vectorToAlterPositonBy;
        // d += $"{transform.position.y}, ";
        _previousFloaterHeight = Vector3.up * nextFloaterHeight;



        if (transform.position.y > _maxHeight) _maxHeight = transform.position.y;
        if (transform.position.y < _minHeight) _minHeight = transform.position.y;

        Debug.DrawLine(new Vector3(0, _maxHeight, 0), new Vector3(0, _minHeight, 0), Color.red);
    }
}
