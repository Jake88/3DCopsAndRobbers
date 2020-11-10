using UnityEngine;

namespace My.ModifiableStats
{
    [System.Serializable]
    public class StatModifier
    {
        [SerializeField] protected ModiferType _type;
        public ModiferType Type => _type;

        [SerializeField] protected float _value;
        public float Value => _value;

        [Tooltip("Optional: Will default based on type.")][SerializeField] protected int _order;
        public int Order => _order > 0 ? _order : (int)Type;

        protected readonly object _source;
        public object Source => _source;

        public StatModifier() {}

        public StatModifier(float value, ModiferType type, int order, object source) : this()
        {
            _value = value;
            _type = type;
            _order = order;
            _source = source;
        }

        public StatModifier(float value, ModiferType type) : this(value, type, 0, null) { }

        public StatModifier(float value, ModiferType type, int order) : this(value, type, order, null) { }

        public StatModifier(float value, ModiferType type, object source) : this(value, type, 0, source) { }
    }
}
