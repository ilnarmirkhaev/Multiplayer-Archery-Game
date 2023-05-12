﻿using System.Collections;
using Fusion;
using UnityEngine;

namespace Photon
{
    [RequireComponent(typeof(NetworkRigidbody))]
    public class ArrowNew : NetworkBehaviour
    {
        [SerializeField] private NetworkRigidbody networkRigidbody;
        [SerializeField] private Transform tip;
        [SerializeField] private TrailRenderer arrowVFX;

        private Rigidbody _rigidbody;

        private float _force;

        private bool _didHit;
        private bool _inAir;
        private Vector3 _lastPosition;

        private Vector3 _hitPositionOrigin;
        private Vector3 _hitDirection;
        private float _hitDistance;

        public void Initialize(float force)
        {
            _force = force;
        }

        public override void Spawned()
        {
            _rigidbody = networkRigidbody.Rigidbody;
            Launch();
        }

        public override void FixedUpdateNetwork()
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
            
            _rigidbody.AddForce(transform.forward * _force, ForceMode.Impulse);
            StartCoroutine(RotateWithVelocity());
            arrowVFX.enabled = true;
        }

        private void SetPhysics(bool usePhysics)
        {
            _rigidbody.useGravity = usePhysics;
            _rigidbody.isKinematic = !usePhysics;
            _rigidbody.interpolation = usePhysics ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }

        private void CheckCollision()
        {
            if (!ArrowTrajectoryCast(out var hitInfo)) return;

            var collidedGameObject = hitInfo.transform.gameObject;

            if (collidedGameObject.TryGetComponent<PlayerHealth>(out var health))
            {
                health.HitPoints -= (int)_force;
            }

            Stop();
            transform.position = hitInfo.point;
            // transform.SetParent(hitInfo.transform);
            PhysicsDebug.DrawDebug(hitInfo.point, 0.1f, 2f);
        }

        private bool ArrowTrajectoryCast(out RaycastHit hitInfo) =>
            Physics.Linecast(_lastPosition, tip.position, out hitInfo, Layers.ArrowsMask);

        // private bool HitObjectIsSender(NetworkBehaviour networkBehaviour) =>
        //     _senderId == networkBehaviour.OwnerClientId;

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
                Quaternion newRotation = Quaternion.LookRotation(_rigidbody.velocity, transform.up);
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