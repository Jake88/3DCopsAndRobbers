using System.Collections;
using UnityEngine;

public class CopAttack : MonoBehaviour
{
    [SerializeField] CopData _initialData;
    [SerializeField] Transform _renderModel;

    Quaternion _originalRotation;

    Robber _target;

    int _damage;
    float _attackSpeed;
    Animator _animator;

    public Robber Target { set { _target = value; } }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _damage = _initialData.InitialDamage;
        _attackSpeed = _initialData.InitialAttackSpeed;
        _originalRotation = _renderModel.localRotation;
    }

    void Start()
    {
        StartCoroutine(Attack());
    }

    void FixedUpdate()
    {
        if (_target)
            _renderModel.LookAt(_target.transform.position);
        else
            _renderModel.localRotation = _originalRotation;
    }

    IEnumerator Attack()
    {
        while (gameObject.activeSelf)
        {
            while (!_target) yield return new WaitForSeconds(.1f);

            if (_target.TakeDamage(_damage)) _target = null;

            _animator.SetTrigger("Shoot");

            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    // TODO: This multiple / add stuff could be moved into a scriptable object or plain class that handles modifying a single value
    // This would be super helpful as there's lots of "buffs" we'd want to apply to values in the game.
    /*EG
     * 
     * (int || float || Range) _value
     * (int || float || Range) Value => _value
     * 
     * void Multiply(multiplyer)
     * void Add(modifer)
     * void MultiplyWithExpiry(multiplyer, time) // Not sure how to implmeent these are Coroutine is a monobehaviour thing.
     * void AddWithExpiry(modifer, time) // ditoo
     */

    public void MultiplyDamage(int multiplyModifer)
    {
        _damage *= multiplyModifer;
    }

    public void AddDamage(int modifer)
    {
        _damage += modifer;
        if (_damage < 0) _damage = 0;
    }
    public void MultiplyDamage(int multiplyModifer, float timeUntilExpires)
    {
        var deltaDamage = (_damage * multiplyModifer) - _damage;
        StartCoroutine(AddDamageWithExpiryCoroutine(deltaDamage, timeUntilExpires));
    }

    public void AddDamageWithExpiry(int addModifer, float timeUntilExpires)
    {
        StartCoroutine(AddDamageWithExpiryCoroutine(addModifer, timeUntilExpires));
    }

    IEnumerator AddDamageWithExpiryCoroutine(int addModifer, float timeUntilExpires)
    {
        if (_damage + addModifer < 0) 
            addModifer = -_damage;

        _damage += addModifer;
        yield return new WaitForSeconds(timeUntilExpires);
        _damage -= addModifer;
    }        

}
