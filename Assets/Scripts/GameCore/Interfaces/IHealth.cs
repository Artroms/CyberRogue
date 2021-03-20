using System;
using System.Collections.Generic;
using UnityEngine;

namespace DVInterfaces
{
    /// <summary>
    /// Базовый интерфейс всех компонентов здоровья
    /// </summary>
    public interface IHealth
    {
        /// <summary>
        /// Доступ к здоровью
        /// </summary>
        int Points { get; set; }
    }
}