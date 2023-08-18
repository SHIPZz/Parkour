using CodeBase.Enums;
using CodeBase.Gameplay.Weapons.Price;
using UnityEngine;

namespace CodeBase.ScriptableObjects.Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Gameplay/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Range(0.3f, 1)] public float ShootDelay;

        public Price Price;
        public string Name;
        public WeaponTypeId WeaponTypeId;
        public ParticleSystem ShootEffect;
        public AudioSource ShootSound;
        public Gameplay.Weapons.Weapon Prefab;
    }
}
