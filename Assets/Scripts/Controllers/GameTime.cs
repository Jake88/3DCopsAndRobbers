using System.Collections;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    // ---------------- STANDARD TIME -----------------
    // 1 second (1f deltaTime) represents 1 game minute 
    public const float GameSecondsInOneRealSecond = 60;
    const float TimeInAGameDay = 24 * GameSecondsInOneRealSecond;
    const float Midnight = 24 * GameSecondsInOneRealSecond; // TODO Allow the flexibility to have multiple midnights?
    const float Payday = 12 * GameSecondsInOneRealSecond; // TODO
    // ------------------------------------------------
    
    [SerializeField] float _gameSpeed = 1;
    [SerializeField] int _levelGracePeriod = 1;

    [SerializeField] GameEvent _newDayEvent;
    [SerializeField] GameEvent<string> _timeChangedEvent;

    int _daysPast;
    float _currentTime;
    float _timeUntilPayday;
    float _timeUntilMidnight;
    float _gameDeltaTime; // This is specific to the level, and to the game time. This variable is not for use for speeding up other stuff in the game.

    public float CurrentDay => _daysPast;
    public float TimeUntilPayday => _timeUntilPayday;
    public float TimeUntilMidnight => _timeUntilMidnight;

    void Awake()
    {
        _timeUntilPayday = (TimeInAGameDay * _levelGracePeriod) + Payday;
        _timeUntilMidnight = (TimeInAGameDay * _levelGracePeriod) + Midnight;
    }

    void Start()
    {
        _newDayEvent.Raise();
        StartCoroutine(TriggerNewMinute());
    }

    // Update is called once per frame
    void Update()
    {
        _gameDeltaTime = Time.deltaTime * _gameSpeed;
        _currentTime += _gameDeltaTime;

        CheckPayday();
        CheckMidnight();
    }

    void StartNewDay()
    {
        _daysPast++;
        _currentTime %= TimeInAGameDay;
        _newDayEvent.Raise();
    }

    int Minute => Mathf.FloorToInt(_currentTime) % 60;
    int SecondMinute => Minute % 10;
    int FirstMinute => Mathf.FloorToInt(Minute / 10);

    int Hour => Mathf.FloorToInt(_currentTime) / 60;
    int SecondHour => Hour % 10;
    int FirstHour => Mathf.FloorToInt(Hour / 10);

    IEnumerator TriggerNewMinute()
    {
        while (true)
        {
            var time = $"{FirstHour}{SecondHour}:{FirstMinute}{SecondMinute}";
            _timeChangedEvent.Raise(time);
            yield return new WaitForSeconds(1f);
        }
    }

    void CheckMidnight()
    {
        _timeUntilMidnight -= _gameDeltaTime;
        if (_timeUntilMidnight < 0)
        {
            _timeUntilMidnight += TimeInAGameDay;
            // fire midnight event
        }
    }

    void CheckPayday()
    {
        _timeUntilPayday -= _gameDeltaTime;
        if (_timeUntilPayday < 0)
        {
            _timeUntilPayday += TimeInAGameDay;
            // fire PaydaY event
        }
    }
}
