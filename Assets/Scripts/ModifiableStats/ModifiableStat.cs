using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using UnityEngine;

namespace My.ModifiableStats
{
    [Serializable]
    public class ModifiableStat
    {
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;
        readonly List<StatModifier> _statModifiers;

        bool _isDirty = true;
        [SerializeField] float _maximumValue = Mathf.NegativeInfinity;
        [SerializeField] float _baseValue;
        float _value;

        public float BaseValue => _baseValue;

        // TODO: Not doiung anything right now!!! Adding CheckMaxValue might have been stopping the value from being set
        public float MaxValue => _maximumValue;
        public float Value
        {
            get
            {
                if (_isDirty)
                {
                    CalculateFinalValue();
                    _isDirty = false;
                }
                return _value;
            }
        }
        public int IntValue => Mathf.RoundToInt(Value);

        public ModifiableStat()
        {
            _statModifiers = new List<StatModifier>();
            StatModifiers = _statModifiers.AsReadOnly();
            CalculateFinalValue();
        }

        public ModifiableStat(float baseValue) : this()
        {
            _baseValue = baseValue;
        }

        public ModifiableStat(float baseValue, float maximumValue) : this()
        {
            _maximumValue = maximumValue;
            _baseValue = baseValue;
        }


        void CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float cumulativeMultiplierValue = 0f;

            for (int i = 0; i < _statModifiers.Count; i++)
            {
                // TODO: I COULD return to having ModifierBehaviours that all have an "apply" method.
                // This switch statement could looke like
                /* 
                 * switch (mod.Type) {
                 *   case (ModiferType.CumulativeMultiplier) :
                 *       // Allows us to either typecast the mod behaviour to a special one such as
                 *       // (mod as CumulativeMod).ApplyCumulative()
                 *       // Or just allows us to build do sepcial stuff in this case block and call mod.Apply() at the end once we've determined the final value.
                 *       // Note if doing the line above, we would need to extend the Apply() method to take the amount, since this cumulative amount is a sum similar modifiers.
                 *       break;
                 *   default: finalValue += mod.Apply(_baseValue, finalValue);
                 * }

                */

                var mod = _statModifiers[i];
                switch (mod.Type)
                {
                    case ModiferType.Override:
                        // Special case - overrides our value and stops calculation. Useful if wanting to set a value to 0 or 999 for an amount of time.
                        // Prevents any other mods having an affect, including additional "Override" mods.
                        _value = mod.Value;
                        return;

                    case ModiferType.Flat:
                        // Basic flat addition of any number.
                        // Unaffected by other mods.
                        finalValue += mod.Value;
                        break;

                    case ModiferType.BaseMultiplier:
                        // Multiply the mod value by the BASE value and add the outcome.
                        // Unaffected by other mods.
                        finalValue += _baseValue * mod.Value;
                        break;

                    case ModiferType.CumulativeMultiplier:
                        // Add all the CumulativeMultipliers together and apply them at once.
                        // Affected by Flat and Base modifiers. Unaffected by _other_ cumulative modifiers.
                        cumulativeMultiplierValue += mod.Value;

                        if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].Type != ModiferType.CumulativeMultiplier)
                        {
                            finalValue *= 1 + cumulativeMultiplierValue;
                            cumulativeMultiplierValue = 0;
                        }
                        break;

                    case ModiferType.ExponentialMultiplier:
                        // Multiply the value by the current value. Caution: scales quickly!
                        // Affected by every other prior to this in the order chain, including previous ExponentialMultipliers.
                        finalValue *= 1 + mod.Value;
                        break;
                }
            }

            _value = (float)Math.Round(finalValue, 4);
        }

        float CheckMaxValue(float value) {
            return MaxValue != Mathf.NegativeInfinity && Mathf.Abs(value) > Mathf.Abs(MaxValue)? MaxValue : value;
        }

        public void SetBaseValue(float newValue)
        {
            _baseValue = newValue;
            CalculateFinalValue();
        }
        
        public void SetMaxValue(float newValue)
        {
            _maximumValue = newValue;
            CalculateFinalValue();
        }

        public void AddModifier(StatModifier mod)
        {
            _statModifiers.Add(mod);
            _statModifiers.Sort(CompareModifierPriority);
            CalculateFinalValue();
        }

        public void AddModifiers(StatModifier[] mods)
        {
            foreach (var mod in mods)
                _statModifiers.Add(mod);

            _statModifiers.Sort(CompareModifierPriority);
            CalculateFinalValue();
        }

        public void AddUniqueExpiringModifier(StatModifier mod, float expireyTime)
        {
            if (StatModifierController.Instance.AddOrRenewModExpirey(this, mod, expireyTime)) return;
            AddModifier(mod);
        }

        public void AddCumulativeExpiringModifier(StatModifier mod, float expireyTime)
        {
            // TODO: add a variable stacking limit, eg (slow that stacks up to 5 times). Probably handled in the controller, but maybe handled within this class.
            StatModifierController.Instance.AddCumulativeModExpirey(this, mod, expireyTime);
            AddModifier(mod);
        }

        public bool RemoveModifier(StatModifier mod)
        {

            if (_statModifiers.Remove(mod))
            {
                CalculateFinalValue();
                return true;
            }
            return false;
        }

        int CompareModifierPriority(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = _statModifiers.Count - 1; i >= 0; i--)
            {
                if (_statModifiers[i].Source == source)
                {
                    didRemove = true;
                    _statModifiers.RemoveAt(i);
                }
            }

            if (didRemove) CalculateFinalValue();

            return didRemove;
        }

        public void Reset()
        {
            _statModifiers.Clear();
            CalculateFinalValue();
        }
    }
}