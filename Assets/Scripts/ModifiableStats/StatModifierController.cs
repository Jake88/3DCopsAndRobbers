using My.ModifiableStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifierController : MonoBehaviour
{
    static Dictionary<ModifiableStat, Dictionary<StatModifier, Coroutine>> _uniqueExpireyCoroutines;
    static Dictionary<ModifiableStat, List<Coroutine>> _cumulativeExpireyCoroutines;
    static StatModifierController _instance;
    public static StatModifierController Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType<StatModifierController>();

            return _instance;
        }
    }

    private void Awake()
    {
        _uniqueExpireyCoroutines = new Dictionary<ModifiableStat, Dictionary<StatModifier, Coroutine>>();
        _cumulativeExpireyCoroutines = new Dictionary<ModifiableStat, List<Coroutine>>();
    }

    public bool AddOrRenewModExpirey(ModifiableStat ms, StatModifier mod, float expireyTime)
    {
        bool beingRenewed = false;

        // Ensure the modifiable stat has an entry in our controller.
        if (!_uniqueExpireyCoroutines.ContainsKey(ms)) 
            _uniqueExpireyCoroutines.Add(ms, new Dictionary<StatModifier, Coroutine>());

        // 
        if (_uniqueExpireyCoroutines[ms].ContainsKey(mod))
        {
            StopCoroutine(_uniqueExpireyCoroutines[ms][mod]);
            _uniqueExpireyCoroutines[ms].Remove(mod);
            beingRenewed = true;
        }

        _uniqueExpireyCoroutines[ms].Add(mod, StartCoroutine(ExpireUniqueMod(ms, mod, expireyTime)));

        return beingRenewed;
    }

    IEnumerator ExpireUniqueMod(ModifiableStat ms, StatModifier mod, float expireyTime)
    {
        yield return new WaitForSeconds(expireyTime);
        ms.RemoveModifier(mod);
        _uniqueExpireyCoroutines[ms].Remove(mod);
    }

    public void AddCumulativeModExpirey(ModifiableStat ms, StatModifier mod, float expireyTime)
    {
        // Ensure the modifiable stat has an entry in our controller.
        if (!_cumulativeExpireyCoroutines.ContainsKey(ms))
            _cumulativeExpireyCoroutines.Add(ms, new List<Coroutine>());

        _cumulativeExpireyCoroutines[ms].Add(StartCoroutine(ExpireCumulativeMod(ms, mod, expireyTime)));
    }

    IEnumerator ExpireCumulativeMod(ModifiableStat ms, StatModifier mod, float expireyTime)
    {
        yield return new WaitForSeconds(expireyTime);
        ms.RemoveModifier(mod);
    }

    public bool PurgeStat(ModifiableStat ms)
    {
        var didPurge = false;
        if (_cumulativeExpireyCoroutines.ContainsKey(ms))
        {
            didPurge = true;
            foreach (var coroutine in _cumulativeExpireyCoroutines[ms])
            {
                StopCoroutine(coroutine);
            }
            _cumulativeExpireyCoroutines[ms].Clear();
        }

        if (!_uniqueExpireyCoroutines.ContainsKey(ms))
        {
            didPurge = true;
            foreach (var coroutine in _uniqueExpireyCoroutines[ms])
            {
                StopCoroutine(coroutine.Value);
            }
            _uniqueExpireyCoroutines[ms].Clear();
        }

        return didPurge;
    }

    public void PurgeAll(ModifiableStat ms)
    {
        StopAllCoroutines();
        _uniqueExpireyCoroutines.Clear();
        _cumulativeExpireyCoroutines.Clear();
    }
}
