using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DavidJalbert
{
    [RequireComponent(typeof(TinyCarController))]

    public class TinyCarPlayer : MonoBehaviour
    {
        public enum InputType
        {
            None, Axis, RawAxis, Key, Button
        }

        [System.Serializable]
        public struct InputValue
        {
            [Tooltip("Type of input.")]
            public InputType type;
            [Tooltip("Name of the input entry.")]
            public string name;
            [Tooltip("Returns the negative value when using an axis type.")]
            public bool invert;
        }

        [Header("Input")]
        [Tooltip("Whether to let this script control the vehicle.")]
        public bool enableInput = true;
        [Tooltip("Input type to check to make the vehicle move forward.")]
        public InputValue forwardInput = new InputValue() { type = InputType.RawAxis, name = "Vertical", invert = false };
        [Tooltip("Input type to check to make the vehicle move backward.")]
        public InputValue reverseInput = new InputValue() { type = InputType.RawAxis, name = "Vertical", invert = true };
        [Tooltip("Input type to check to make the vehicle turn right.")]
        public InputValue steerRightInput = new InputValue() { type = InputType.RawAxis, name = "Horizontal", invert = false };
        [Tooltip("Input type to check to make the vehicle turn left.")]
        public InputValue steerLeftInput = new InputValue() { type = InputType.RawAxis, name = "Horizontal", invert = true };

        [Header("Visuals")]
        [Tooltip("Object on which to apply the controller's position and rotation.")]
        public Transform vehicleBody;
        [Tooltip("How fast the wheels will spin relative to the car's forward velocity.")]
        public float wheelsSpinForce = 100;
        [Tooltip("Maximum angle at which to turn the wheels when steering.")]
        public float wheelsMaxTurnAngle = 45;
        [Tooltip("How much to smooth the rotation when wheels are turning.")]
        public float wheelsTurnSmoothing = 10;
        [Tooltip("Wheel objects that will turn and spin with steering and acceleration.")]
        public Transform[] wheelsFront;
        [Tooltip("Wheel objects that will spin with acceleration.")]
        public Transform[] wheelsBack;
        [Tooltip("Whether to rotate the vehicle forward and back on slopes.")]
        public bool rotatePitch = true;
        [Tooltip("Whether to rotate the vehicle left and right on slopes.")]
        public bool rotateRoll = true;

        [Header("Particles")]

        [Tooltip("Minimum velocity when scraping against a wall to play the particle system.")]
        public float minSideFrictionVelocity = 0;
        [Tooltip("Particle system to play when scraping against a wall.")]
        public ParticleSystem particlesSideFriction;

        [Tooltip("Minimum force when hitting a wall to play the particle system.")]
        public float minSideCollisionForce = 0;
        [Tooltip("Particle system to play when hitting a wall.")]
        public ParticleSystem particlesSideCollision;

        [Tooltip("Minimum force when landing on the ground to play the particle system.")]
        public float minLandingForce = 0;
        [Tooltip("Particle system to play when landing on the ground.")]
        public ParticleSystem particlesLanding;

        [Tooltip("Minimum lateral velocity when drifting to play the particle system.")]
        public float minDriftingSpeed = 0;
        [Tooltip("Particle system to play when drifting on the ground.")]
        public ParticleSystem particlesDrifting;

        private TinyCarController car;
        private float wheelRotation = 0;
        private float wheelSpin = 0;

        void Start()
        {
            car = GetComponent<TinyCarController>();

            if (particlesSideFriction != null) particlesSideFriction.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (particlesSideCollision != null) particlesSideCollision.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (particlesLanding != null) particlesLanding.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (particlesDrifting != null) particlesDrifting.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        
        void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            // input

            if (enableInput)
            {
                float motorDelta = getInput(forwardInput) - getInput(reverseInput);
                float steeringDelta = getInput(steerRightInput) - getInput(steerLeftInput);

                car.setSteering(steeringDelta);
                car.setMotor(motorDelta);
            }

            // visuals

            if (vehicleBody != null)
            {
                Vector3 smoothRotation = car.getGroundRotationSmooth().eulerAngles;
                if (!rotatePitch)
                {
                    smoothRotation.x = 0;
                }
                if (!rotateRoll)
                {
                    smoothRotation.z = 0;
                }

                vehicleBody.rotation = Quaternion.Euler(smoothRotation);

                vehicleBody.position = car.getBodyPosition();
            }

            wheelSpin += car.getForwardVelocity() * deltaTime * wheelsSpinForce;
            wheelRotation = Mathf.Lerp(wheelRotation, car.getSteering() * wheelsMaxTurnAngle, wheelsTurnSmoothing <= 0 ? 1 : Mathf.Clamp01(wheelsTurnSmoothing * deltaTime));

            foreach (Transform t in wheelsFront)
            {
                t.transform.localRotation = Quaternion.Euler(wheelSpin, wheelRotation, 0);
            }
            foreach (Transform t in wheelsBack)
            {
                t.transform.localRotation = Quaternion.Euler(wheelSpin, 0, 0);
            }

            // particles

            // drifting smoke

            if (particlesDrifting != null)
            {
                if (Mathf.Abs(car.getLateralVelocity()) > minDriftingSpeed && car.isGrounded())
                {
                    if (!particlesDrifting.isPlaying)
                    {
                        particlesDrifting.Play();
                    }
                }
                else
                {
                    if (particlesDrifting.isPlaying)
                    {
                        particlesDrifting.Stop();
                    }
                }
            }

            // collision sparks

            if (particlesSideCollision != null)
            {
                if (car.hasHitSide(minSideCollisionForce))
                {
                    particlesSideCollision.transform.position = car.getSideHitPosition();
                    particlesSideCollision.Play();
                }
            }

            if (particlesLanding != null)
            {
                if (car.hasHitGround(minLandingForce))
                {
                    particlesLanding.Play();
                }
            }
            
            if (particlesSideFriction != null)
            {
                if (car.isHittingSide() && car.getGroundVelocity() > minSideFrictionVelocity)
                {
                    particlesSideFriction.transform.position = car.getSideHitPosition();
                    if (!particlesSideFriction.isPlaying) particlesSideFriction.Play();
                }
                else
                {
                    if (particlesSideFriction.isPlaying) particlesSideFriction.Stop();
                }
            }
        }

        public float getInput(InputValue v)
        {
            float value = 0;
            switch (v.type)
            {
            case InputType.Axis: value = Input.GetAxis(v.name); break;
            case InputType.RawAxis: value = Input.GetAxisRaw(v.name); break;
            case InputType.Key: value = Input.GetKey(v.name) ? 1 : 0; break;
            case InputType.Button: value = Input.GetButton(v.name) ? 1 : 0; break;
            }
            if (v.invert) value *= -1;
            return Mathf.Clamp01(value);
        }
    }
}