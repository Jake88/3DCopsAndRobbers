using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    /*
     * Maybe I should refactor this. - CURRENTLY I AM USING A REF_MANAGER TO PASS THIS AROUND.
     * Have a "PlayerMoney" object which is a scene mono behaviour that holds the data local to the scene
     * Then have a scriptable object which is basically just a delegate class with a bunch of functions
     * These functions would be all the "LoseMoney" etc
     * The scriptable object would look something like
     * 
     * {
     *   PlayerMoney _scenePlayerMoney
     *   void Register(PlayerMoney pm) _scenePlayerMoney = pm;
     *   
     *   // all those other functions
     * }
     * 
     * Then the PlayerMoney class (this one) would have
     * 
     * {
     *   [SerializeField] PlayerMoneyController _pmc;
     *   void Awake() { _pmc.Register(this) }
     * }
     * 
     * Then Cops, Robbers, Cash objects etc can all take a reference to the PlayerMoneyController scriptable object and interact with the playerMoney via that.
     */


    private class EarningsRecord
    {
        public int moneyEarned;
        public int moneySpent;
        public int moneyLost;
        public int moneyForeverLost;
        public int moneyRecovered;
    }

    [Header("Required game managers")]
    [SerializeField] GameTime _gameTime;

    [Header("Analysis helpers")]
    [SerializeField] int _daysToUseForDataAnalysis = 3;

    int _playerMoney = 0;
    GameEvent<int> _moneyChangedEvent;

    // MetaData
    float _timeStarted;
    EarningsRecord _totalEarnings = new EarningsRecord();
    List<EarningsRecord> _dailyEarnings = new List<EarningsRecord>();

    // GET Properties
    int Money => _playerMoney;
    float TotalAverageIncomePerSecond => _totalEarnings.moneyEarned / _timeStarted;
    float ShortenedAverageIncomePerSecond => CalculateEarnings(_daysToUseForDataAnalysis).moneyEarned / _daysToUseForDataAnalysis;// / _gameTime.SecondsToGameSeconds;
    int ForecastAtPayday => _playerMoney + Mathf.RoundToInt(ShortenedAverageIncomePerSecond * _gameTime.TimeUntilPayday);

    EarningsRecord CalculateEarnings(int days)
    {
        var daysBeen = _dailyEarnings.Count;
        if (days > daysBeen) days = daysBeen;

        var calculatedEarnings = new EarningsRecord();
        for (int i = 0; i < days; i++)
        {
            var currentIndex = daysBeen - i - 1;
            calculatedEarnings.moneyEarned += _dailyEarnings[currentIndex].moneyEarned;
            calculatedEarnings.moneySpent += _dailyEarnings[currentIndex].moneySpent;
            calculatedEarnings.moneyLost += _dailyEarnings[currentIndex].moneyLost;
            calculatedEarnings.moneyForeverLost += _dailyEarnings[currentIndex].moneyForeverLost;
            calculatedEarnings.moneyRecovered += _dailyEarnings[currentIndex].moneyRecovered;
        }
        return calculatedEarnings;
    }

    private void Awake()
    {
        _moneyChangedEvent = GetComponent<GameEventListener_Int>().Event;
    }

    public void OnNewDayEvent()
    {
         // TODO: Set up event
        _dailyEarnings.Add(new EarningsRecord());
    }

    public void FixedUpdate()
    {
        _timeStarted += Time.fixedDeltaTime;
    }

    public void AlterMoneyWithoutTrace(int amount)
    {
        _playerMoney += amount;
        _moneyChangedEvent.Raise(_playerMoney);
    }

    public void EarnMoney(int amount)
    {
        AlterMoneyWithoutTrace(amount);
        _totalEarnings.moneyEarned += amount;
        _dailyEarnings[_dailyEarnings.Count - 1].moneyEarned += amount;
    }

    public void SpendMoney(int amount)
    {
        AlterMoneyWithoutTrace(-amount);
        _totalEarnings.moneySpent += amount;
        _dailyEarnings[_dailyEarnings.Count - 1].moneySpent += amount;
    }
    public void LoseMoney(int amount)
    {
        AlterMoneyWithoutTrace(-amount);
        _totalEarnings.moneyLost += amount;
        _totalEarnings.moneyForeverLost += amount;
        _dailyEarnings[_dailyEarnings.Count - 1].moneyLost += amount;
        _dailyEarnings[_dailyEarnings.Count - 1].moneyForeverLost += amount;

    }
    public void RecoverMoney(int amount)
    {
        AlterMoneyWithoutTrace(-amount);
        _totalEarnings.moneyRecovered += amount;
        _totalEarnings.moneyForeverLost -= amount;
        _dailyEarnings[_dailyEarnings.Count - 1].moneyRecovered += amount;
        _dailyEarnings[_dailyEarnings.Count - 1].moneyForeverLost -= amount;
    }
}
