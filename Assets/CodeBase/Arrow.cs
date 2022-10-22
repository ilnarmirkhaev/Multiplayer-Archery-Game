using System;
using System.Collections;
using UnityEngine;

namespace CodeBase
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Arrow : MonoBehaviour
    {
        public Rigidbody rigidbody;
        [SerializeField] private Collider arrowCollider;
        [SerializeField] private Transform tip;
        [SerializeField] private TrailRenderer arrowVFX;

        private bool _didHit;
        private bool _inAir;
        private Vector3 _lastPosition;
        private bool _isPrecise;

        private Vector3 _hitPositionOrigin;
        private Vector3 _hitDirection;
        private float _hitDistance;

        public void Initialize(CharacterController controller, bool isPrecise)
        {
            SetPhysics(false);
            IgnorePlayerCollider(controller);
            arrowVFX.enabled = false;
            _isPrecise = isPrecise;
        }

        private void FixedUpdate()
        {
            if (!_inAir) return;

            CheckCollision();
            _lastPosition = tip.position;

            if (_lastPosition.y < -10)
                Destroy(gameObject);
        }

        public void Fire(float force)
        {
            Application.targetFrameRate = -1;
            _inAir = true;
            _lastPosition = tip.position;

            arrowCollider.enabled = false;
            SetPhysics(true);

            rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
            StartCoroutine(RotateWithVelocity());
            arrowVFX.enabled = true;
        }

        private void SetPhysics(bool usePhysics)
        {
            rigidbody.useGravity = usePhysics;
            rigidbody.isKinematic = !usePhysics;
            rigidbody.interpolation = usePhysics ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }

        private void IgnorePlayerCollider(Collider playerCollider) =>
            Physics.IgnoreCollision(arrowCollider, playerCollider);

        private void CheckCollision()
        {
            if (!ArrowTrajectoryCast(out var hitInfo)) return;
            Stop();
            transform.position = hitInfo.point;
            PhysicsDebug.DrawDebug(hitInfo.point, 0.1f, 2f);
            transform.SetParent(hitInfo.transform);
        }

        private bool ArrowTrajectoryCast(out RaycastHit hitInfo)
        {
            if (_isPrecise)
                return Physics.Linecast(_lastPosition, tip.position, out hitInfo);
            
            Vector3 movementStep = tip.position - _lastPosition;
            var didHit = Physics.SphereCast(_lastPosition, 0.1f, movementStep, out hitInfo, movementStep.magnitude);
            _hitPositionOrigin = _lastPosition;
            _hitDirection = movementStep.normalized;
            _hitDistance = hitInfo.distance;
            return didHit;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_hitPositionOrigin + _hitDirection * _hitDistance, 0.1f);
        }

        private void Stop()
        {
            _inAir = false;
            SetPhysics(false);

            StartCoroutine(DestroyAfterTime());
        }

        private IEnumerator RotateWithVelocity()
        {
            yield return new WaitForFixedUpdate();
            while (_inAir)
            {
                Quaternion newRotation = Quaternion.LookRotation(rigidbody.velocity, transform.up);
                transform.rotation = newRotation;
                yield return null;
            }
        }

        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}