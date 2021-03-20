using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DVInterfaces
{
    /// <summary>
    /// Базовый интерфейс атакующих компонентов
    /// </summary>
    public interface IAttack
    {
        /// <summary>
        /// Атакует
        /// </summary>
        /// <param name="health">атакуемый компонент</param>
        void Attack(IHealth health);
    }
}