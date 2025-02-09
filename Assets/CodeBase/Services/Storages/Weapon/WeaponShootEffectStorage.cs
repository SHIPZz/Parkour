﻿using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.ScriptableObjects.Weapon;
using CodeBase.Services.Data;
using CodeBase.Services.Factories;
using UnityEngine;

namespace CodeBase.Services.Storages.Weapon
{
    public class WeaponShootEffectStorage
    {
        private readonly IEffectFactory _effectFactory;
        private readonly Dictionary<WeaponTypeId, ParticleSystem> _shootEffects = new();
        private readonly Dictionary<WeaponTypeId, AudioSource> _shootSounds = new();

        public WeaponShootEffectStorage(IEffectFactory effectFactory, WeaponStaticDataService weaponStaticDataService)
        {
            _effectFactory = effectFactory;
            List<WeaponData> weaponDatas = weaponStaticDataService.GetAll();

            FillShootEffects(weaponDatas);
            FillShootSounds(weaponDatas);
        }

        public IEnumerable<AudioSource> GetAllSounds() => _shootSounds.Values;

        private void FillShootSounds(List<WeaponData> weaponDatas)
        {
            foreach (WeaponData weaponData in weaponDatas)
            {
                if (weaponData.ShootSound == null)
                    continue;

                AudioSource targetSound = _effectFactory.Create(weaponData.ShootSound);

                targetSound.playOnAwake = false;
                _shootSounds[weaponData.WeaponTypeId] = targetSound;
            }
        }

        private void FillShootEffects(List<WeaponData> weaponDatas)
        {
            foreach (WeaponData weaponData in weaponDatas)
            {
                if (weaponData.ShootEffect == null)
                    continue;

                ParticleSystem targetEffect = _effectFactory.Create(weaponData.ShootEffect);
                
                _shootEffects[weaponData.WeaponTypeId] = targetEffect;
            }
        }

        public AudioSource GetSoundBy(WeaponTypeId weaponTypeId) => 
         _shootSounds[weaponTypeId];

        public ParticleSystem GetEffectBy(WeaponTypeId weaponTypeId) => 
            !_shootEffects.TryGetValue(weaponTypeId, out var effect) ? 
                null :
                effect;
    }
}