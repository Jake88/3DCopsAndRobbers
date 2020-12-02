using My.Abilities;
using My.Singletons;
using My.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CopResume
{
    const string DECIMAL_FORMAT = "{0:0.00}";
    public CopData CopData { get; private set; }
    public string Name { get; private set; }
    public float MovementSpeed { get; private set; }
    public float AttackSpeed { get; private set; }
    public int Damage { get; private set; }
    public int BaseSalary { get; private set; }

    public string MovementSpeedString { get; private set; }
    public string AttackSpeedString { get; private set; }
    public string DamageString { get; private set; }
    public string BaseSalaryString { get; private set; }

    // Maybe add rating if it's a thing...
    public Ability[] Abilities { get; private set; }

    public CopResume(CopData copData, string name, float movementSpeed, float attackSpeed, int damage, int baseSalary, Ability[] abilities = null)
    {
        CopData = copData;
        Name = name;
        MovementSpeed = movementSpeed;
        AttackSpeed = attackSpeed;
        Damage = damage;
        BaseSalary = baseSalary;
        Abilities = abilities != null ? abilities : new Ability[0];

        MovementSpeedString = String.Format(DECIMAL_FORMAT, MovementSpeed);
        AttackSpeedString = String.Format(DECIMAL_FORMAT, AttackSpeed);
        DamageString = Damage.ToString();
        BaseSalaryString = BaseSalary.ToString();
    }
}

public class CopFactory : MonoBehaviour
{
    string[] _firstNames;
    string[] _lastNames;

    [Range(0, 1f)][SerializeField] float _movementSpeedVariance = .2f;
    [Range(0, 1f)][SerializeField] float _attackSpeedVariance = .2f;
    [Range(0, 1f)][SerializeField] float _damageVariance = .2f;
    [Range(0, 1f)][SerializeField] float _baseSalaryVariance = .2f;
    [Range(0, 2)][SerializeField] int _abilityCountVariance;

    public CopResume GenerateCop(CopData _data)
    {
        var name = $"{_firstNames[Random.Range(0, _firstNames.Length)]} {_lastNames[Random.Range(0, _lastNames.Length)]}";

        // var abilities = RefManager.AbilityFactory.GetCopAbilities(_data.prerequisites, _data.randomAbilityCount);

        return new CopResume(
            _data,
            name,
            ApplyVariance(_data.InitialMovementSpeed, _movementSpeedVariance),
            ApplyVariance(_data.InitialAttackSpeed, _attackSpeedVariance),
            ApplyVariance(_data.InitialDamage, _damageVariance),
            ApplyVariance(_data.InitialCost, _baseSalaryVariance));
    }

    float ApplyVariance(float initialValue, float variance) => initialValue * (Random.Range(-variance, variance) + 1);
    int ApplyVariance(int initialValue, float variance) => Mathf.RoundToInt(initialValue * (Random.Range(-variance, variance) + 1));
}

public class CopResumeManager : MonoBehaviour
{
    [SerializeField] CopData[] _availableCops;
    [SerializeField] Range _copsToGenerate = new Range(1, 3);
    [SerializeField] GameEvent _onCopsGenerated;

    CopFactory _copFactory;

    public Dictionary<CopData, CopResume[]> CopResumes { get; private set; } = new Dictionary<CopData, CopResume[]>();

    void Awake()
    {
        _copFactory = GetComponent<CopFactory>();
        RefManager.GameTime.PaydayEvent.AddListener(AtNoon);
        foreach (var copType in _availableCops)
            CopResumes.Add(copType, new CopResume[(int)_copsToGenerate.Max]);
    }

    void AtNoon()
    {
        GenerateCops();
        _onCopsGenerated.Raise();
    }

    void GenerateCops()
    {
        foreach (var copType in _availableCops)
        {
            var numberToGenerate = _copsToGenerate.RandomInt;
            for (int i = 0; i < _copsToGenerate.Max; i++)
            {
                CopResumes[copType][i] = null;
                if (i < numberToGenerate)
                    CopResumes[copType][i] = _copFactory.GenerateCop(copType);
            }
        }
    }

    void OnDestroy()
    {
        RefManager.GameTime.PaydayEvent.RemoveListener(AtNoon);
    }


    // On noon event, generate a new set of cops with random abilities.
    // This set of robbers should be configurable.


    // This pure data class should then be the foundation supplied to a new Cop on retrieve from the the pool.
    // Robbers can have abilities added to them for a cost?
    // How does this affect the game balance.. how many "different" abilties can be on a cop?
    // Is there anything stopping a user from rerolling on a single cop to get the uber nuts and just restarting if they fail?
    // -- does that matter??


    // The new resumes should be sent to the UI panels for displaying and selecting
    // These UI panels also need to clean up the old ones.
    // Should there be a 35-50% chance for a Cop's resume to remain in the pool?
    // Can we lock resumes?
}
