using My.Abilities;
using My.Buildables;
using My.Cops;
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
        [SerializeField] BlueprintManager _blueprintManager;
        [SerializeField] ShopBuilder _shopBuilder;
        [SerializeField] CopResumeManager _copManager;

        //use this in prefab script
        public static PlayerMoney PlayerMoney { get { return instance._playerMoney; } }
        public static GameTime GameTime { get { return instance._gameTime; } }
        public static AbilityFactory AbilityFactory { get { return instance._abilityFactory; } }
        public static BlueprintManager BlueprintManager { get { return instance._blueprintManager; } }
        public static ShopBuilder ShopBuilder { get { return instance._shopBuilder; } }
        public static CopResumeManager CopResumeManager { get { return instance._copManager; } }
    }
}