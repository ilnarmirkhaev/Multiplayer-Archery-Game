using Unity.Netcode;
using UnityEngine;

namespace CodeBase.Player
{
    public struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y, _z;
        private short _yRotation;

        public Vector3 Position
        {
            get => new Vector3(_x, _y, _z);
            set
            {
                _x = value.x;
                _y = value.y;
                _z = value.z;
            }
        }

        public Vector3 Rotation
        {
            get => new Vector3(0, _yRotation, 0);
            set => _yRotation = (short)value.y;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _z);

            serializer.SerializeValue(ref _yRotation);
        }
    }
}