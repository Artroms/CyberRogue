using UnityEngine;
using System;
using DVInterfaces;

namespace Artromskiy
{
    public sealed class Health : MonoBehaviour, IHealth
    {
        [SerializeField]
        [Tooltip("Количество здоровья")]
        private int points;

        public event Action<int> OnDamage;
        public event Action<int> OnHeal;
        public event Action OnDead;
        public int Points
        {
            get => points;
            set
            {
                if (value < points)
                    OnDamage?.Invoke(value);
                if (value > points)
                    OnHeal?.Invoke(value);
                if (value <= 0)
                    OnDead?.Invoke();
                points = value;
            }
        }
    }
}
