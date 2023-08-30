using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace CodeBase.Gameplay.Bullet
{
    public class ThrowingBulletMovement : BulletMovement
    {
        [SerializeField] private Vector3 _bulletModelRotation;
        [SerializeField] private Transform _knifeEnd;

        private ThrowingBullet _throwingBullet;
        private bool _blockedRotation;
        private Coroutine _moveCoroutine;
        private Vector3 _hitPosition;

        protected override void Awake()
        {
            base.Awake();
            _throwingBullet = GetComponent<ThrowingBullet>();
            SetInitialRotation(_throwingBullet);
            RigidBody.isKinematic = false;
            RigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public override void Move(Vector3 target, Vector3 startPosition)
        {
            MoveDirection = target - startPosition;
            MoveDirection.Normalize();
            transform.forward = MoveDirection;
            SetInitialRotation(_throwingBullet);
        }

        protected override void OnCollisionEntered(UnityEngine.Collision target)
        {
            _hitPosition = target.GetContact(0).point;
            transform.forward = MoveDirection;
            RigidBody.useGravity = false;
            RigidBody.velocity = Vector3.zero;
            RigidBody.angularVelocity = Vector3.zero;
            SetInitialRotation(_throwingBullet);

            if (target.gameObject.TryGetComponent(out EnemyPartForKnifeHolder enemyPartForKnifeHolder))
            {
                RigidBody.interpolation = RigidbodyInterpolation.None;
                transform.SetParent(target.transform);
            }

            Vector3 offset = _hitPosition - _knifeEnd.position;
            RigidBody.position += offset; 

            IsHit = true;
        }
        
        private void SetInitialRotation(ThrowingBullet throwingBullet)
        {
            Vector3 startTargetRotation = new Vector3(104, transform.eulerAngles.y,
                transform.eulerAngles.z);

            transform.rotation = Quaternion.Euler(startTargetRotation);

            throwingBullet.BulletModel.localRotation = Quaternion.Euler(_bulletModelRotation);
        }
    }
}