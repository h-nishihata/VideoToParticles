using UnityEngine;

namespace Reaktion
{

    public class ConstantMotion : MonoBehaviour
    {
        public enum TransformMode
        {
            Off, XAxis, YAxis, ZAxis, Arbitrary, Random
        };

        // A class for handling each transformation.
        [System.Serializable]
        public class TransformElement
        {
            public TransformMode mode = TransformMode.Off;
            public float velocity = 1;

            // Used only in the arbitrary mode.
            public Vector3 arbitraryVector = Vector3.up;

            // Affects velocity.
            public float randomness = 0;

            // Randomizer states.
            Vector3 randomVector;
            float randomScalar;

            public float limit;
            public bool hasReachedLimit;
            public float startTime;
            public float changeDirSec;
            public float maxVelocity;

            public void Initialize()
            {
                randomVector = Random.onUnitSphere;
                randomScalar = Random.value;
                limit = mode == TransformMode.XAxis ? 0.38f : 0.12f;
                changeDirSec = Mathf.Abs(velocity);
                maxVelocity = velocity;
            }

            // Get a vector corresponds to the current transform mode.
            public Vector3 Vector
            {
                get
                {
                    switch (mode)
                    {
                        case TransformMode.XAxis: return Vector3.right;
                        case TransformMode.YAxis: return Vector3.up;
                        case TransformMode.ZAxis: return Vector3.forward;
                        case TransformMode.Arbitrary: return arbitraryVector;
                        case TransformMode.Random: return randomVector;
                    }
                    return Vector3.zero;
                }
            }

            // Get the current delta value.
            public float Delta
            {
                get
                {
                    var scale = (1.0f - randomness * randomScalar);
                    return velocity * scale * Time.deltaTime;
                }
            }
        }

        public TransformElement position = new TransformElement();
        public TransformElement rotationX = new TransformElement { velocity = 3 };
        public TransformElement rotationY = new TransformElement { velocity = 1 };
        public bool useLocalCoordinate = true;

        void Awake()
        {
            position.Initialize();
            rotationX.Initialize();
            rotationY.Initialize();
        }

        void Update()
        {
            if (position.mode != TransformMode.Off)
            {
                if (useLocalCoordinate)
                    transform.localPosition += position.Vector * position.Delta;
                else
                    transform.position += position.Vector * position.Delta;
            }

            if (rotationX.mode != TransformMode.Off)
            {
                var delta = Quaternion.AngleAxis(rotationX.Delta, rotationX.Vector);

                if (useLocalCoordinate)
                    transform.localRotation = delta * transform.localRotation;
                else
                    transform.rotation = delta * transform.rotation;
            }

            if (rotationY.mode != TransformMode.Off)
            {
                var delta = Quaternion.AngleAxis(rotationY.Delta, rotationY.Vector);

                if (useLocalCoordinate)
                    transform.localRotation = delta * transform.localRotation;
                else
                    transform.rotation = delta * transform.rotation;
            }

            CheckRotateLimit(rotationX, transform.localRotation.x);
            CheckRotateLimit(rotationY, transform.localRotation.y);
        }

        private void CheckRotateLimit(TransformElement element, float rotAngle)
        {
            var rotLimit = element.limit;
            var offset = element.mode == TransformMode.XAxis ? 0.5f : -1f;

            if ((rotAngle > rotLimit) || (rotAngle < rotLimit * offset))
            {
                if (!element.hasReachedLimit)
                {
                    element.startTime = Time.timeSinceLevelLoad;
                    element.hasReachedLimit = true;
                }
            }

            if (element.hasReachedLimit)
            {
                var diff = Time.timeSinceLevelLoad - element.startTime;
                var rate = diff / element.changeDirSec;
                element.velocity = Mathf.Lerp(element.maxVelocity, element.maxVelocity * -1f, rate);
            }

            var resetPos = element.mode == TransformMode.XAxis ? 0.28f : 0;
            if ((rotAngle <= resetPos + 0.01f) && (rotAngle >= resetPos - 0.01f))
            {
                element.hasReachedLimit = false;
                element.maxVelocity = element.velocity;
            }
        }
    }

} // namespace Reaktion
