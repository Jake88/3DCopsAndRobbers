using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefManager : MonoBehaviour
{
    static RefManager instance;
    void Awake() { instance = this; }

    [SerializeField] PlayerMoney _playerMoney;

    //use this in prefab script
    public static PlayerMoney PlayerMoney { get { return instance._playerMoney; } }
}