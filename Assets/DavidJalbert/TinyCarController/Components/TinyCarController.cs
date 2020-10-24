using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DavidJalbert
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]

    [ExecuteInEditMode]

    public class TinyCarController : MonoBehaviour
    {
        public enum GRAVITY_MODE
        {
            AlwaysDown, TowardsGround
        }

        [Header("Physics")]
        [Tooltip("Radius of the sphere collider.")]
        public float colliderRadius = 2f;
        [Tooltip("Mass of the rigid body.")]
        public float bodyMass = 1f;
        [Tooltip("Always Down = Gravity always points straight down.\nTowards Ground = Gravity points to the surface when the car is grounded, otherwise it points straight down.")]
        public GRAVITY_MODE gravityMode = GRAVITY_MODE.TowardsGround;
        [Tooltip("Gravity speed.")]
        public float gravityVelocity = 80;
        [Tooltip("Maximum gravity.")]
        public float maxGravity = 50;
        [Tooltip("Maximum angle of climbable slopes.")]
        public float maxSlopeAngle = 50f;
        [Tooltip("Amount of friction applied when colliding with a wall.")]
        public float sideFriction = 1f;
        [Tooltip("Max distance to check for ground. Rarely needs to be changed.")]
        public float groundCheckDistance = 0.25f;
        [Tooltip("Width of the collider skin when checking for ground. Rarely needs to be changed.")]
        public float groundCheckSkinWidth = 0.01f;
        [Tooltip("The layers that will be used for ground checks.")]
        public LayerMask collidableLayers = ~0;

        [Header("Engine")]
        [Tooltip("How much acceleration to apply relative to the speed of the car.")]
        public AnimationCurve accelerationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0) });
        [Tooltip("Maximum acceleration when going forward.")]
        public float maxAccelerationForward = 100;
        [Tooltip("Maximum speed when going forward.")]
        public float maxSpeedForward = 40;
        [Tooltip("Maximum acceleration when going in reverse.")]
        public float maxAccelerationReverse = 50;
        [Tooltip("Maximum speed when going in reverse.")]
        public float maxSpeedReverse = 20;
        [Tooltip("How fast the car will brake when the motor goes in the opposite direction.")]
        public float brakeStrength = 200;
        [Tooltip("How much friction to apply when on a slope. The higher this value, the slower you'll climb up slopes and the faster you'll go down. Setting this to zero adds no additional friction.")]
        public float slopeFriction = 1f;

        [Header("Steering")]
        [Tooltip("Sharpness of the steering.")]
        public float maxSteering = 200;
        [Tooltip("Multiplier applied to steering when in the air. Setting this to zero makes the car unsteerable in the air.")]
        public float steeringMultiplierInAir = 0.25f;
        [Tooltip("How much steering to apply relative to the speed of the car.")]
        public AnimationCurve steeringBySpeed = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(0.25f, 1), new Keyframe(1, 1) });
        [Tooltip("How fast the car stops when releasing the gas.")]
        public float forwardFriction = 40;
        [Tooltip("How much grip the car should have on the road when turning.")]
        public float lateralFriction = 80;

        [Header("Misc")]
        [Tooltip("How fast the car should align to the ground (when using getGroundRotationSmooth()).")]
        public float rotationSmoothingOnGround = 10f;
        [Tooltip("How fast the car should align in the air (when using getGroundRotationSmooth()).")]
        public float rotationSmoothingInAir = 5f;

        private Rigidbody body;
        private SphereCollider sphereCollider;
        private bool onGround = false;
        private float maxSpeedForwardSqr = 0;
        private float maxSpeedReverseSqr = 0;
        private Vector3 crossForward = Vector3.zero;
        private Vector3 crossUp = Vector3.zero;
        private Vector3 crossRight = Vector3.zero;
        private bool hitSide = false;
        private float hitSideForce = 0;
        private Vector3 hitSidePosition = Vector3.zero;
        private bool hitGround = false;
        private float hitGroundForce = 0;
        private bool hitSideStay = false;
        private float groundVelocity = 0;
        private float forwardVelocity = 0;
        private float rightVelocity = 0;
        private Vector3 gravityDirection = Vector3.zero;
        private float inputSteering = 0;
        private float inputMotor = 0;
        private PhysicMaterial customPhysicMaterial;
        private Quaternion groundRotationAbsolute;
        private Quaternion groundRotationSmooth;
        private TinyCarSurfaceParameters surfaceParameters = TinyCarSurface.DEFAULT;
        private float slopeDelta = 0;

        void Start()
        {
            body = GetComponent<Rigidbody>();
            sphereCollider = GetComponent<SphereCollider>();

            customPhysicMaterial = new PhysicMaterial();
            customPhysicMaterial.bounciness = 0;
            customPhysicMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
            customPhysicMaterial.staticFriction = 0;
            customPhysicMaterial.dynamicFriction = 0;
            customPhysicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;

            if (!Application.isPlaying) return;

            groundRotationAbsolute = transform.rotation;
            groundRotationSmooth = transform.rotation;

            crossForward = transform.forward;
            crossRight = transform.right;
            crossUp = transform.up;
        }

        void Update()
        {
            // refresh rigid body and collider parameters
            if (!Application.isPlaying)
            {
                body.hideFlags = HideFlags.NotEditable;
                sphereCollider.hideFlags = HideFlags.NotEditable;

                body.mass = bodyMass;
                body.drag = 0;
                body.angularDrag = 0;
                body.constraints = RigidbodyConstraints.FreezeRotation;
                body.useGravity = false;
                body.isKinematic = false;
                body.interpolation = RigidbodyInterpolation.None;
                body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                sphereCollider.radius = colliderRadius;
                sphereCollider.isTrigger = false;
                sphereCollider.material = customPhysicMaterial;

                return;
            }
            // ---
        }

        void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;
            float motor = inputMotor;
            float steering = maxSteering * inputSteering;
            surfaceParameters = TinyCarSurface.DEFAULT;
            maxSpeedForwardSqr = maxSpeedForward * maxSpeedForward;
            maxSpeedReverseSqr = maxSpeedReverse * maxSpeedReverse;

            // direction and velocity vectors
            //// basic ground check
            onGround = false;
            crossUp = transform.up;
            RaycastHit hitSphere;
            if (Physics.SphereCast(sphereCollider.bounds.center, sphereCollider.radius - groundCheckSkinWidth, Vector3.down, out hitSphere, groundCheckDistance + groundCheckSkinWidth, collidableLayers, QueryTriggerInteraction.Ignore))
            {
                crossUp = hitSphere.normal;

                if (Vector3.Angle(crossUp, Vector3.up) <= maxSlopeAngle)
                {
                    onGround = true;

                    TinyCarSurface surface = hitSphere.collider.GetComponentInParent<TinyCarSurface>();
                    if (surface != null)
                    {
                        surfaceParameters = surface.parameters;
                    }
                }
            }

            //// get average ground direction from corners (forward-left, forward-right, back-left, back-right)
            Vector3[] groundCheckSource = new Vector3[]{
                sphereCollider.bounds.center + Vector3.forward * sphereCollider.radius + Vector3.left * sphereCollider.radius,
                sphereCollider.bounds.center + Vector3.forward * sphereCollider.radius + Vector3.right * sphereCollider.radius,
                sphereCollider.bounds.center + Vector3.back * sphereCollider.radius + Vector3.left * sphereCollider.radius,
                sphereCollider.bounds.center + Vector3.back * sphereCollider.radius + Vector3.right * sphereCollider.radius
            };

            Vector3[] groundCheckHits = new Vector3[groundCheckSource.Length];
            bool[] groundCheckFound = new bool[groundCheckSource.Length];
            RaycastHit rayHit;
            for (int i = 0; i < groundCheckSource.Length; i++)
            {
                groundCheckFound[i] = Physics.Raycast(groundCheckSource[i], Vector3.down, out rayHit, sphereCollider.radius * 2, collidableLayers, QueryTriggerInteraction.Ignore);
                if (groundCheckFound[i])
                {
                    groundCheckHits[i] = rayHit.point;
                }
            }

            //// append the calculated corner normals to the center normal
            Vector3 triFRNormal = Vector3.zero;
            if (groundCheckFound[0] && groundCheckFound[1] && groundCheckFound[2])
            {
                triFRNormal = getTriangleNormal(groundCheckHits[0], groundCheckHits[1], groundCheckHits[2]);
            }

            Vector3 triBLNormal = Vector3.zero;
            if (groundCheckFound[1] && groundCheckFound[3] && groundCheckFound[2])
            {
                triBLNormal = getTriangleNormal(groundCheckHits[1], groundCheckHits[3], groundCheckHits[2]);
            }

            crossUp = (crossUp + triFRNormal + triBLNormal).normalized;

            //// calculate ground rotation
            Vector3 velocity = body.velocity;
            groundVelocity = (velocity - Vector3.up * velocity.y).magnitude;
            crossForward = Vector3.Cross(-crossUp, transform.right);
            crossRight = Vector3.Cross(-crossUp, transform.forward);

            forwardVelocity = Vector3.Dot(velocity, crossForward);
            rightVelocity = Vector3.Dot(velocity, crossRight);

            groundRotationAbsolute = Quaternion.LookRotation(crossForward, crossUp);
            float groundXAngle = groundRotationAbsolute.eulerAngles.x;
            slopeDelta = Mathf.Clamp(groundXAngle > 180 ? groundXAngle - 360 : groundXAngle, -90, 90) / 90f;
            // ---

            // car visuals rotation
            Quaternion groundRotationPlane = Quaternion.Euler(groundRotationSmooth.eulerAngles.x, groundRotationAbsolute.eulerAngles.y, groundRotationSmooth.eulerAngles.z);
            float rotationLerpValue = onGround ? rotationSmoothingOnGround : rotationSmoothingInAir;
            groundRotationSmooth = Quaternion.Slerp(groundRotationPlane, groundRotationAbsolute, rotationLerpValue <= 0 ? 1 : deltaTime * rotationLerpValue);
            // ---

            // steering
            float steeringForce = (onGround ? 1 : steeringMultiplierInAir) * (forwardVelocity < 0 ? -1 : 1) * steeringBySpeed.Evaluate(getForwardVelocityDelta()) * surfaceParameters.steeringMultiplier;
            body.MoveRotation(Quaternion.Euler(0, transform.rotation.eulerAngles.y + steering * deltaTime * steeringForce, 0));
            // ---

            // gravity
            gravityDirection = Vector3.zero;
            switch (gravityMode)
            {
                case GRAVITY_MODE.AlwaysDown:
                    gravityDirection = Vector3.down;
                    break;
                case GRAVITY_MODE.TowardsGround:
                    gravityDirection = onGround ? -crossUp : Vector3.down;
                    break;
            }
            // ---

            // velocity
            if (onGround)
            {
                if (motor == 0 || Mathf.Sign(motor) != Mathf.Sign(forwardVelocity)) velocity -= crossForward * Mathf.Sign(forwardVelocity) * Mathf.Min(Mathf.Abs(forwardVelocity), forwardFriction * deltaTime * surfaceParameters.forwardFrictionMultiplier);
                velocity -= crossRight * Mathf.Sign(rightVelocity) * Mathf.Min(Mathf.Abs(rightVelocity), lateralFriction * deltaTime * surfaceParameters.lateralFrictionMultiplier);

                float slopeMultiplier = Mathf.Max(0, (Mathf.Sign(motor) * slopeDelta * slopeFriction + 1f));
                float accelerationForce = getAcceleration(surfaceParameters.accelerationMultiplier * slopeMultiplier, surfaceParameters.speedMultiplier * slopeMultiplier);

                velocity += crossForward * getMotor() * accelerationForce * deltaTime;
            }

            velocity += gravityDirection * Mathf.Min(Mathf.Max(0, maxGravity + velocity.y), gravityVelocity * deltaTime);
            if (velocity.sqrMagnitude > getMaxSpeedSqr()) velocity = velocity.normalized * getMaxSpeed();

            if (hitSideStay) velocity *= 1f - Mathf.Clamp01(deltaTime * sideFriction * hitSideForce);

            body.velocity = velocity;
            // ---

            // reset current frame vars
            hitSide = false;
            hitSideStay = false;
            hitGround = false;
            hitSideForce = 0;
            hitGroundForce = 0;
            hitSidePosition = Vector3.zero;
            // ---
        }

        private void OnDestroy()
        {
            if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().hideFlags = HideFlags.None;
            if (GetComponent<SphereCollider>() != null) GetComponent<SphereCollider>().hideFlags = HideFlags.None;
        }

        private int getZeroSign(float value)
        {
            if (value == 0) return 0;
            else return (int)Mathf.Sign(value);
        }

        private Vector3 getTriangleNormal(Vector3 pa, Vector3 pb, Vector3 pc)
        {
            Vector3 u = pb - pa;
            Vector3 v = pc - pa;

            return new Vector3(u.y * v.z - u.z * v.y, u.z * v.x - u.x * v.z, u.x * v.y - u.y * v.x).normalized;
        }

        public float getSlopeDelta()
        {
            return slopeDelta;
        }

        public float getAcceleration(float accelerationMultiplier = 1f, float speedMultiplier = 1f)
        {
            if (isBraking())
            {
                return brakeStrength;
            }
            else
            {
                float accelerationCurveDelta = Mathf.Abs(forwardVelocity) / (getMaxSpeed() * speedMultiplier);
                if (accelerationCurveDelta > 1) return -getMaxAcceleration();
                return accelerationCurve.Evaluate(accelerationCurveDelta) * getMaxAcceleration() * accelerationMultiplier;
            }
        }

        public int getVelocityDirection()
        {
            return getZeroSign(forwardVelocity);
        }

        public bool isBraking()
        {
            return getMotor() != 0 && getVelocityDirection() == -getZeroSign(getMotor());
        }

        public float getMaxSpeed()
        {
            return getVelocityDirection() >= 0 ? maxSpeedForward : maxSpeedReverse;
        }

        public float getMaxSpeedSqr()
        {
            return getMaxSpeed() * getMaxSpeed();
        }

        public float getMaxAcceleration()
        {
            return getVelocityDirection() >= 0 ? maxAccelerationForward : maxAccelerationReverse;
        }

        public void setSteering(float value)
        {
            inputSteering = value;
        }

        public void setMotor(float value)
        {
            inputMotor = value;
        }

        public float getSteering()
        {
            return inputSteering;
        }

        public float getMotor()
        {
            return inputMotor;
        }

        public int getMotorDirection()
        {
            return getZeroSign(getMotor());
        }

        public TinyCarSurfaceParameters getSurfaceParameters()
        {
            return surfaceParameters;
        }

        public Rigidbody getBody()
        {
            if (body == null) body = GetComponent<Rigidbody>();
            return body;
        }

        public bool hasHitGround(float minDownwardVelocity = 0)
        {
            return hitGround && hitGroundForce >= minDownwardVelocity;
        }

        public bool isHittingSide()
        {
            return hitSideStay;
        }

        public bool hasHitSide(float minForce = 0)
        {
            return hitSide && hitSideForce >= minForce;
        }

        public Vector3 getSideHitPosition()
        {
            return hitSidePosition;
        }

        public float getSideHitForce()
        {
            return hitSideForce;
        }

        public bool isGrounded()
        {
            return onGround;
        }

        public Vector3 getGravityDirection()
        {
            return gravityDirection;
        }

        public Vector3 getBodyPosition()
        {
            return body.transform.position;
        }

        public Vector3 getGroundPosition()
        {
            return getBodyPosition() + Vector3.down * sphereCollider.radius;
        }

        public Quaternion getBodyRotation()
        {
            return body.transform.rotation;
        }

        public Quaternion getGroundRotation()
        {
            return groundRotationAbsolute;
        }

        public Quaternion getGroundRotationSmooth()
        {
            return groundRotationSmooth;
        }

        public float getForwardVelocity()
        {
            return forwardVelocity;
        }

        public float getLateralVelocity()
        {
            return rightVelocity;
        }

        public float getForwardVelocityDelta()
        {
            return Mathf.Abs(getForwardVelocity()) / getMaxSpeed();
        }

        public float getGroundVelocity()
        {
            return groundVelocity;
        }

        public float getGroundVelocityDelta()
        {
            return getGroundVelocity() / getMaxSpeed();
        }

        public float getVerticalDot(Collision collision)
        {
            return Vector3.Dot(collision.contacts[0].normal, Vector3.up);
        }

        public float getCollisionForce(Collision collision)
        {
            return collision.relativeVelocity.sqrMagnitude > 0.1f ? Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) : 0;
        }

        public float getCollisionForceOnXZ(Collision collision)
        {
            Vector3 xzVelocity = new Vector3(collision.relativeVelocity.x, 0, collision.relativeVelocity.z);
            return collision.relativeVelocity.sqrMagnitude > 0.1f ? Vector3.Dot(collision.contacts[0].normal, xzVelocity) : 0;
        }

        public float getCollisionForceOnY(Collision collision)
        {
            Vector3 yVelocity = Vector3.up * collision.relativeVelocity.y;
            return collision.relativeVelocity.sqrMagnitude > 0.1f ? Vector3.Dot(collision.contacts[0].normal, yVelocity) : 0;
        }

        public float getGroundHitForce()
        {
            return hitGroundForce;
        }

        private void OnCollisionStay(Collision collision)
        {
            ContactPoint hit = collision.contacts[0];
            float verticalDot = getVerticalDot(collision);
            float collisionXZForce = getCollisionForceOnXZ(collision);

            if (verticalDot < 0.1f && collisionXZForce > 0.1f)
            {
                hitSideStay = true;
                if (hitSideForce < collisionXZForce)
                {
                    hitSideForce = collisionXZForce;
                    hitSidePosition = hit.point;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint hit = collision.contacts[0];
            float verticalDot = getVerticalDot(collision);
            float collisionYForce = getCollisionForceOnY(collision);

            if (verticalDot > 0.1f && collisionYForce > 0.1f)
            {
                hitGround = true;
                if (hitGroundForce < collisionYForce)
                {
                    hitGroundForce = collisionYForce;
                }
            }

            float collisionXZForce = getCollisionForceOnXZ(collision);
            if (verticalDot < 0.1f && collisionXZForce > 0.1f)
            {
                hitSide = true;
                hitSideStay = true;
                if (hitSideForce < collisionXZForce)
                {
                    hitSideForce = collisionXZForce;
                    hitSidePosition = hit.point;
                }
            }
        }
    }
}