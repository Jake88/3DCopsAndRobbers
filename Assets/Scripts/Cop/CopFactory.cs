using My.Singletons;
using My.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace My.Cops
{
    public class CopFactory : MonoBehaviour
    {
        string[] _firstNames = new string[] { "phill", "Jim", "Margret" };
        string[] _lastNames = new string[] { "Thatcher", "Turner", "Craig" };

        [Range(0, 1f)] [SerializeField] float _movementSpeedVariance = .2f;
        [Range(0, 1f)] [SerializeField] float _attackSpeedVariance = .2f;
        [Range(0, 1f)] [SerializeField] float _damageVariance = .2f;
        [Range(0, 1f)] [SerializeField] float _baseSalaryVariance = .2f;
        [Range(0, 2)] [SerializeField] int _abilityCountVariance;

        public CopResume GenerateCop(CopData _data)
        {
            var name = $"{_firstNames[Random.Range(0, _firstNames.Length)]} {_lastNames[Random.Range(0, _lastNames.Length)]}";

            var abilities = RefManager.AbilityFactory.GetCopAbilities(_data.AbilityFlags, _data.RandomAbilityCount);

            return new CopResume(
                _data,
                name,
                ApplyVariance(_data.InitialMovementSpeed, _movementSpeedVariance),
                ApplyVariance(_data.InitialAttackSpeed, _attackSpeedVariance),
                ApplyVariance(_data.InitialDamage, _damageVariance),
                ApplyVariance(_data.InitialCost, _baseSalaryVariance),
                abilities
            );
        }

        float ApplyVariance(float initialValue, float variance) => initialValue * (Random.Range(-variance, variance) + 1);
        int ApplyVariance(int initialValue, float variance) => Mathf.RoundToInt(initialValue * (Random.Range(-variance, variance) + 1));
    }
}