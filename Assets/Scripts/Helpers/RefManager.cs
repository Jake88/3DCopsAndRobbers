using UnityEngine;

public class RefManager : MonoBehaviour
{
    static RefManager instance;
    void Awake() { instance = this; }

    [SerializeField] PlayerMoney _playerMoney;
    [SerializeField] GameTime _gameTime;

    //use this in prefab script
    public static PlayerMoney PlayerMoney { get { return instance._playerMoney; } }
    public static GameTime GameTime { get { return instance._gameTime; } }
}