using My.Abilities;
using System;

namespace My.Cops
{
    public class CopResume
    {
        const string DECIMAL_FORMAT = "{0:0.00}";
        public CopData CopData { get; private set; }
        public string Name { get; private set; }
        public float MovementSpeed { get; private set; }
        public float AttackSpeed { get; private set; }
        public int Damage { get; private set; }
        public int BaseSalary { get; private set; }

        public string MovementSpeedString { get; private set; }
        public string AttackSpeedString { get; private set; }
        public string DamageString { get; private set; }
        public string BaseSalaryString { get; private set; }

        // Maybe add rating if it's a thing...
        public Ability[] Abilities { get; private set; }

        public CopResume(CopData copData, string name, float movementSpeed, float attackSpeed, int damage, int baseSalary, Ability[] abilities = null)
        {
            CopData = copData;
            Name = name;
            MovementSpeed = movementSpeed;
            AttackSpeed = attackSpeed;
            Damage = damage;
            BaseSalary = baseSalary;
            Abilities = abilities != null ? abilities : new Ability[0];

            MovementSpeedString = String.Format(DECIMAL_FORMAT, MovementSpeed);
            AttackSpeedString = String.Format(DECIMAL_FORMAT, AttackSpeed);
            DamageString = Damage.ToString();
            BaseSalaryString = BaseSalary.ToString();
        }
    }
}