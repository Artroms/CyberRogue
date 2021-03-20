using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DVInterfaces;

namespace Artromskiy
{
    public sealed class Attacker : MonoBehaviour, IAttack
    {
        [SerializeField]
        [Tooltip("Урон наносимый юниту")]
        private int damage = 1;
        [Range(0,5)]
        [SerializeField]
        [Tooltip("Время перед атакой, для соответствия анимации и нанесения урона")]
        private float delay = 1;
        private Animator anim;
        private IEnumerator attack;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        [ContextMenu("Attack")]
        public void Attack(IHealth health)
        {
            if (attack == null)
            {
                attack = AttackRoutine(health);
                StartCoroutine(attack);
            }
        }

        private IEnumerator AttackRoutine(IHealth health)
        {
            if (health.Equals(null))
            {
                attack = null;
                yield break;
            }
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1f/delay);
            if (!health.Equals(null))
                health.Points -= damage;
            attack = null;
        }
    }
}