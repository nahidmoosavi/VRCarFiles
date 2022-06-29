
using System.Collections.Generic;
using UnityEngine;
using VRCar;

namespace Core
{
    public class Car : MonoBehaviour
    {
        [SerializeField] protected List<AxleInfo> axleInfos;  //Holds WheelColider and information about which axels steer or accelerate a Vehicle. For a car with 2 axles the list has two elements
        [SerializeField] protected Transform centerOfMass;   //Cary the transform of a car's Center of Mass. Set in Unity
        [SerializeField] protected float maxSteeringAngle;

        [SerializeField] protected Speedometer speedometer;

        [SerializeField] private List<GameObject> meshes;
        [SerializeField] private float antiRollValue = 5000.0f;
        [SerializeField] private int[] speedThresholds = new int[5];   //5 diffferent acceleration force for 5 gears
        [SerializeField] private int[] brakeThresholds = new int[5];   //5 diffferent barke force for 5 gears
        [SerializeField] private int slowSteerValue;
        [SerializeField] private int middleSteerValue;
        [SerializeField] private int fastSteerValue;

        [SerializeField] float defaultDampeningValue = 0.005f;
        [SerializeField] float stopDampeningValue = 30f;

        [SerializeField] protected Rigidbody carRigidbody;
        float radious;

        private int speedThreshold;
        private int brakeThreshold;
        private int tempSteerValue = 0;
        private const float mPersTokPerh = 3.6f;

        const float maxMotorTorque = 200;

        private void OnEnable()
        {
            radious = axleInfos[0].leftWheel.radius;
            tempSteerValue = slowSteerValue;

            if (carRigidbody && centerOfMass)
            {
                carRigidbody.centerOfMass = centerOfMass.localPosition;
            }


        }


        protected virtual void SteerCar(float input)
        {
            //float tempInput = Mathf.Clamp(input * tempSteerValue, -maxSteeringAngle, maxSteeringAngle);
            float tempInput = input * tempSteerValue;

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.canSteer)
                {
                    axleInfo.leftWheel.steerAngle = tempInput;
                    axleInfo.rightWheel.steerAngle = tempInput;
                }

                ApplyLocalPositionToTires(axleInfo.leftWheel);
                ApplyLocalPositionToTires(axleInfo.rightWheel);
            }
        }


        protected virtual void AccelerateCar(float input)
        {

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (input == 0)
                {
                    axleInfo.leftWheel.wheelDampingRate = stopDampeningValue;
                    axleInfo.rightWheel.wheelDampingRate = stopDampeningValue;
                    return;
                }
                else
                {
                    axleInfo.leftWheel.wheelDampingRate = defaultDampeningValue;
                    axleInfo.rightWheel.wheelDampingRate = defaultDampeningValue;

                    if (axleInfo.canAccelerate)
                    {
                        axleInfo.leftWheel.motorTorque = input * speedThreshold * Time.deltaTime;
                        axleInfo.rightWheel.motorTorque = input * speedThreshold * Time.deltaTime;
                    }
                }
                //ApplyLocalPositionToTires(axleInfo.leftWheel);
                //ApplyLocalPositionToTires(axleInfo.rightWheel);
            }
        }


        protected virtual void AccelerateCar(float input, bool isAccelerating)
        {
            if (isAccelerating == false)
            {
                speedThreshold = 1;
            }

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (input == 0)
                {
                    axleInfo.leftWheel.wheelDampingRate = stopDampeningValue;
                    axleInfo.rightWheel.wheelDampingRate = stopDampeningValue;
                    return;
                }
                else
                {
                    axleInfo.leftWheel.wheelDampingRate = defaultDampeningValue;
                    axleInfo.rightWheel.wheelDampingRate = defaultDampeningValue;

                    if (axleInfo.canAccelerate)
                    {
                        axleInfo.leftWheel.motorTorque = input * speedThreshold * Time.deltaTime;
                        axleInfo.rightWheel.motorTorque = input * speedThreshold * Time.deltaTime;
                    }
                }
                //ApplyLocalPositionToTires(axleInfo.leftWheel);
                //ApplyLocalPositionToTires(axleInfo.rightWheel);
            }
        }
        protected virtual void BrakeCar(float input)
        {
            foreach (AxleInfo axleInfo in axleInfos)
            {
                axleInfo.leftWheel.brakeTorque = input * brakeThreshold * Time.deltaTime;
                axleInfo.rightWheel.brakeTorque = input * brakeThreshold * Time.deltaTime;
            }
        }


        private void ApplyLocalPositionToTires(WheelCollider wheelCollider)
        {
            if (wheelCollider.transform.childCount == 0) return;

            Transform tire = wheelCollider.transform.GetChild(0);

            Vector3 position;
            Quaternion rotation;

            wheelCollider.GetWorldPose(out position, out rotation);

            tire.transform.position = position;
            tire.transform.rotation = rotation;
        }



        private void AntiRollBar(WheelCollider leftWheel, WheelCollider rightWheel)
        {
            WheelHit wheelHit;
            float travelLeftWheel = 1.0f;
            float travelRightWheel = 1.0f;

            bool isGroundedLeft = leftWheel.GetGroundHit(out wheelHit);
            if (isGroundedLeft)
            {
                travelLeftWheel = (-leftWheel.transform.InverseTransformPoint(wheelHit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
            }

            bool isGroundedRight = rightWheel.GetGroundHit(out wheelHit);
            if (isGroundedRight)
            {
                travelRightWheel = (-rightWheel.transform.InverseTransformPoint(wheelHit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;
            }

            float antiRollForce = (travelLeftWheel - travelRightWheel) * antiRollValue;

            if (isGroundedLeft)
            {
                carRigidbody.AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);
            }

            if (isGroundedRight)
            {
                carRigidbody.AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
            }
        }

        protected void ApplyAntiRollBar()
        {
            foreach (AxleInfo axleInfo in axleInfos)
            {
                AntiRollBar(axleInfo.leftWheel, axleInfo.rightWheel);
            }
        }

        protected void ChangeGears()
        {
            float speed = carRigidbody.velocity.magnitude * mPersTokPerh;
            speedThreshold = speedThresholds[0];

            switch (speed)
            {
                case <= 10:
                    speedThreshold = speedThresholds[0];
                    brakeThreshold = brakeThresholds[0];
                    tempSteerValue = slowSteerValue;
                    carRigidbody.angularDrag = 0.01f;
                    break;
                case (> 10 and <= 20):
                    speedThreshold = speedThresholds[1];
                    brakeThreshold = brakeThresholds[1];
                    tempSteerValue = slowSteerValue;
                    carRigidbody.angularDrag = 0.1f;
                    break;

                case (> 20 and <= 40):
                    speedThreshold = speedThresholds[2];
                    brakeThreshold = brakeThresholds[2];
                    tempSteerValue = middleSteerValue;
                    carRigidbody.angularDrag = 1f;
                    break;
                case (> 40 and <= 50):
                    speedThreshold = speedThresholds[3];
                    brakeThreshold = brakeThresholds[3];
                    tempSteerValue = middleSteerValue;
                    carRigidbody.angularDrag = 2f;
                    break;
                case (> 50 and <= maxMotorTorque):
                    speedThreshold = speedThresholds[4];
                    brakeThreshold = brakeThresholds[4];
                    tempSteerValue = fastSteerValue;
                    carRigidbody.angularDrag = 4f;
                    break;
                case (> maxMotorTorque):
                    speedThreshold = 1;
                    brakeThreshold = brakeThresholds[5];
                    tempSteerValue = fastSteerValue;
                    carRigidbody.angularDrag = 8f;
                    break;

                default:
                    break;
            }
        }

        protected void ShowSpeed()
        {
            float speed = carRigidbody.velocity.magnitude * mPersTokPerh;
            float normalizedSpeed = speed / 200;
            //print("normalizedSpeed" + normalizedSpeed);
            speedometer.SpeedometerRotation(normalizedSpeed);
        }

        /// <summary>
        /// A helper function taht calculates the car speed based on Tire raduis and Wheel RPM
        /// The result was as same as Rigidbody
        /// </summary>
        /// <returns></returns>
        protected float CalculateCarSpeed()
        {

            float speed = (int)(axleInfos[0].leftWheel.rpm * axleInfos[0].leftWheel.radius * 20 * 3.14f * 60 * 1.609f / 63360);
            //float speed = (int)(axleInfos[0].leftWheel.rpm * axleInfos[0].leftWheel.radius * 20 * 0.1885f);
            return speed;
        }


        //protected void AnimateTiers()
        //{
        //    foreach (GameObject tire in meshes)
        //    {
        //        tire.transform.Rotate(carRigidbody.velocity.magnitude *
        //            (transform.InverseTransformDirection(carRigidbody.velocity).z) / (2 * Mathf.PI * radious), 0, 0);
        //    }
        //}
    }
}

