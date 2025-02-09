using CodeBase.Gameplay.Character.Enemy;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace CodeBase.Gameplay.Bullet
{
    public class ThrowingBulletMovement : BulletMovement
    {
        [SerializeField] private Vector3 _bulletModelRotation;
        [SerializeField] private Transform _knifeEnd;
        [SerializeField] private bool _needSetParent = true;
        [SerializeField] private float _targetRotationX = 104f;

        private ThrowingBullet _throwingBullet;
        private bool _blockedRotation;
        private Coroutine _moveCoroutine;
        private Vector3 _hitPosition;

        protected override void Awake()
        {
            base.Awake();
            _throwingBullet = GetComponent<ThrowingBullet>();
            SetInitialRotation();
            GetComponent<Collider>().isTrigger = false;
            RigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            RigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        public override void Move(Vector3 target, Vector3 startPosition)
        {
            transform.position = startPosition;
            RigidBody.position = startPosition;
            MoveDirection = target - startPosition;
            MoveDirection = MoveDirection.normalized;
            transform.forward = MoveDirection;
            SetInitialRotation();
        }

        protected override void OnCollisionEntered(UnityEngine.Collision target)
        {
            _hitPosition = target.GetContact(0).point;
            transform.forward = MoveDirection;
            RigidBody.useGravity = false;
            RigidBody.velocity = Vector3.zero;
            RigidBody.angularVelocity = Vector3.zero;
            SetInitialRotation();

            if (target.gameObject.TryGetComponent(out EnemyPartForKnifeHolder enemyPartForKnifeHolder) &&
                _needSetParent)
            {
                RigidBody.interpolation = RigidbodyInterpolation.None;
                transform.SetParent(target.transform);
            }

            Vector3 offset = _hitPosition - _knifeEnd.position;
            transform.position += offset;

            IsHit = true;
        }

        private void SetInitialRotation()
        {
            Vector3 startTargetRotation = new Vector3(_targetRotationX, transform.eulerAngles.y,
                transform.eulerAngles.z);

            transform.rotation = Quaternion.Euler(startTargetRotation);
            _throwingBullet.BulletModel.localEulerAngles = _bulletModelRotation;
        }
    }
}