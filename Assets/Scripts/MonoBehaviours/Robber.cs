using Pathfinding;
using System.Collections;
using UnityEngine;

public class Robber : MonoBehaviour, IRobber
{
    const float SecondsSpentCollectingMoney = 3f;

    [Header("Required game managers")]
    [SerializeField] CashDropManager _cashDropManager;
    PlayerMoney _playerMoney;

    [Header("Unique robber fields")]
    [SerializeField] RobberData _data;
    public bool RepeatOffender;

    int _amountToSteal;
    int _amountStolen;

    float _moveSpeed;
    float _maxHp;
    float _hp;
    float _difficultyWeight;

    int _destinationIndex = 1;
    Path _path;

    // Other components
    AIPath _ai;
    Floater _floater;

    void Awake()
    {
        _playerMoney = RefManager.PlayerMoney;
        _ai = GetComponent<AIPath>();
        _floater = GetComponentInChildren<Floater>();
    }

    void Update()
    {
        if (!_ai.pathPending && (_ai.reachedEndOfPath || !_ai.hasPath))
        {
            CheckForBank();
            SetDestination();
        }
    }

    public void Spawn(Path path)
    {
        _difficultyWeight = _data.InitialDifficultyWeight;
        _amountToSteal = _data.InitialStealAmount;
        _moveSpeed = _data.InitialMoveSpeed; // Is _moveSpeed requiured, or can we always just use _ai.maxSpeed? I assume they will be the same
        _ai.maxSpeed = _moveSpeed;
        _maxHp = _data.InitialHP;
        _hp = _maxHp;

        // Setup pathing
        _path = path;
        transform.position = _path.StartNode.RadialPosition;
        _destinationIndex = 0;
        SetDestination();
    }

    void Steal()
    {
        _playerMoney.LoseMoney(_amountToSteal);
        _amountStolen = _amountToSteal;
        _floater.Pause();
        _ai.maxSpeed = 0;

        StartCoroutine(Resume());
    }

    IEnumerator Resume()
    {
        yield return new WaitForSeconds(SecondsSpentCollectingMoney);

        _floater.Resume();
        _ai.maxSpeed = _moveSpeed;
    }

    void CheckForBank()
    {
        if (_path.HasArrivedAtBank(_destinationIndex)) Steal();
    }


    void SetDestination()
    {
        // Maybe turn this into a coroutine so we can pause at the start if we're at a bank or distraction
        _destinationIndex++;
        RadialNode nextDestination = _path.GetNode(_destinationIndex);

        if (nextDestination == null)
        {
            if (RepeatOffender)
            {
                _destinationIndex = 1;
                nextDestination = _path.GetNode(_destinationIndex);
            }
            else
            {
                Escape();
                return;
            }
        }

        Vector3 randomPoint = nextDestination.RadialPosition;
        _ai.destination = randomPoint;
        _ai.SearchPath();
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if (_hp < 0) Die();
    }

    void Die()
    {
        if (_amountStolen > 0)
            _cashDropManager.DropCash(transform.position, _amountStolen, CashSource.Robber);

        CleanUp();
    }


    void Escape()
    {
        CleanUp();
    }

    void CleanUp()
    {
        _amountStolen = 0;
        _data.Pool.Release(gameObject);
        // required if Release doesn't do it
        // gameObject.SetActive(false);

    }
}
