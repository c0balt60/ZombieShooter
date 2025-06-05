﻿// Copyright 2021, Infima Games. All Rights Reserved.

using System;
using UnityEngine;

namespace ZombieShooter
{
    /// <summary>
    /// Weapon Scope.
    /// </summary>
    public class Scope : ScopeBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Interface")]

        [Tooltip("Interface Sprite.")]
        [SerializeField]
        private Sprite sprite;
        
        #endregion

        #region GETTERS
        
        public override Sprite GetSprite() => sprite;

        #endregion
    }
}