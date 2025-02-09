﻿using System;
using CodeBase.Enums;
using CodeBase.ScriptableObjects.Sound;
using CodeBase.Services.Storages.Sound;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Character.Enemy
{
    public class EffectOnHit : IInitializable, IDisposable
    {
        private readonly IHealth _health;
        private readonly ParticleSystem _hitEffect;
        private readonly AudioSource _hitSound;
        private bool _canPlayEffect = true;

        public EffectOnHit(IHealth health, [Inject(Id = ParticleTypeId.EnemyHitEffect)] ParticleSystem hitEffect,
            SoundsSettings soundsSettings, ISoundStorage soundStorage, EnemyTypeId enemyTypeId)
        {
            SoundTypeId audioTypeId = soundsSettings.HitEnemySounds[enemyTypeId];
            _hitSound = soundStorage.Get(audioTypeId);
            _health = health;
            _hitEffect = hitEffect;
        }

        public void Initialize()
        {
            _health.ValueChanged += PlayEffect;
            _health.ValueZeroReached += Play;
        }

        public void Dispose()
        {
            _health.ValueChanged -= PlayEffect;
            _health.ValueZeroReached -= Play;
        }

        private void Play()
        {
            _hitEffect.Play();
            _hitSound.Play();
        }

        private void PlayEffect(int obj)
        {
            _hitEffect.Play();
            _hitSound.Play();
        }
    }
}