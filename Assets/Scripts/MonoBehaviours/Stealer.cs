using My.ModifiableStats;
using UnityEngine;

public class Stealer : MonoBehaviour, ISteal
{
    [SerializeField] CashDropManager _cashDropManager;
    PlayerMoney _playerMoney;

    ModifiableStat _amountToSteal;
    int _amountStolen;

    public ModifiableStat AmountToSteal => _amountToSteal;

    private void Start()
    {
        _playerMoney = RefManager.PlayerMoney;

    }
    public void Initilise(int initialStealAmount)
    {
        _amountToSteal = new ModifiableStat(initialStealAmount);
    }

    public void Steal()
    {
        _playerMoney.LoseMoney(_amountToSteal.IntValue);
        _amountStolen = _amountToSteal.IntValue;
    }

    public bool DropCash()
    {
        var hasCash = _amountStolen > 0;
        
        if (hasCash) _cashDropManager.DropCash(transform.position, _amountStolen, CashSource.Robber);
        return hasCash;
    }

    public void Reset()
    {
        _amountToSteal.Reset();
        _amountStolen = 0;
    }
}