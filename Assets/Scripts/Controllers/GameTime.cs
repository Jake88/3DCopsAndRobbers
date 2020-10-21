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

    GameEvent _newDayEvent;

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
        _newDayEvent = GetComponent<GameEventListener>().Event;
        _timeUntilPayday = (TimeInAGameDay * _levelGracePeriod) + Payday;
        _timeUntilMidnight = (TimeInAGameDay * _levelGracePeriod) + Midnight;
    }

    void Start()
    {
        _newDayEvent.Raise();
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
    }

    void CheckMidnight()
    {
        _timeUntilMidnight -= _gameDeltaTime;
        if (_timeUntilMidnight < 0)
        {
            _timeUntilMidnight += TimeInAGameDay;
            StartNewDay();
            _newDayEvent.Raise();
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
