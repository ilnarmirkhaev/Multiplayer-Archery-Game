using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace CodeBase
{
    [RequireComponent(typeof(Rigidbody))]
    public class Arrow : NetworkBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Transform tip;
        [SerializeField] private TrailRenderer arrowVFX;

        private ulong _ownerClientId;
        private float _force;

        private bool _didHit;
        private bool _inAir;
        private Vector3 _lastPosition;

        private Vector3 _hitPositionOrigin;
        private Vector3 _hitDirection;
        private float _hitDistance;
        private WaitForFixedUpdate _waitForFixedUpdate;

        public void Initialize(ulong ownerClientId, float force)
        {
            _ownerClientId = ownerClientId;
            _force = force;

            SetPhysics(false);
            arrowVFX.enabled = false;
            _waitForFixedUpdate = new WaitForFixedUpdate();
        }

        public override void OnNetworkSpawn()
        {
            // if (!IsServer) enabled = false;
        }

        private void FixedUpdate()
        {
            if (!_inAir) return;

            CheckCollision();
            _lastPosition = tip.position;

            if (_lastPosition.y < -10)
                Destroy(gameObject);
        }

        public void Launch()
        {
            _inAir = true;
            _lastPosition = tip.position;

            SetPhysics(true);

            rigidbody.AddForce(transform.forward * _force, ForceMode.Impulse);
            StartCoroutine(RotateWithVelocity());
            arrowVFX.enabled = true;
        }

        private void SetPhysics(bool usePhysics)
        {
            rigidbody.useGravity = usePhysics;
            rigidbody.isKinematic = !usePhysics;
            rigidbody.interpolation = usePhysics ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }

        private void CheckCollision()
        {
            if (!ArrowTrajectoryCast(out var hitInfo)) return;
            Stop();
            transform.position = hitInfo.point;
            // transform.SetParent(hitInfo.transform);
            PhysicsDebug.DrawDebug(hitInfo.point, 0.1f, 2f);
        }

        private bool ArrowTrajectoryCast(out RaycastHit hitInfo) =>
            Physics.Linecast(_lastPosition, tip.position, out hitInfo, Layers.ArrowsMask);

        private void Stop()
        {
            _inAir = false;
            SetPhysics(false);

            StartCoroutine(DestroyAfterTime());
        }

        private IEnumerator RotateWithVelocity()
        {
            yield return _waitForFixedUpdate;
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