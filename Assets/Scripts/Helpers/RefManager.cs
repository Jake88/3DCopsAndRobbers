using My.Abilities;
using UnityEngine;

namespace My.Singletons
{
    public class RefManager : MonoBehaviour
    {
        static RefManager instance;
        void Awake() { instance = this; }

        [SerializeField] PlayerMoney _playerMoney;
        [SerializeField] GameTime _gameTime;
        [SerializeField] AbilityFactory _abilityFactory;

        //use this in prefab script
        public static PlayerMoney PlayerMoney { get { return instance._playerMoney; } }
        public static GameTime GameTime { get { return instance._gameTime; } }
        public static AbilityFactory AbilityFactory { get { return instance._abilityFactory; } }
    }
}