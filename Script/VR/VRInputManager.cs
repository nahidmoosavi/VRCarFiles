
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


namespace VRCar
{
    /// <summary>
    /// Reads Input from VR controllers
    /// </summary>
    public class VRInputManager : MonoBehaviour
    {
        [SerializeField] InputActionReference accelerationInput;
        [SerializeField] InputActionReference brakeInput;



        private float steerValue = 0;
        private float accelerateValue = 0;
        private float brakeValue = 0;
        private float lastAccelerationValue = 0;
        private List<float> AccelerationValues = new List<float>(); ///Keeps Two previous movement values
        private bool isAccelerating = false;

        public float SteerValue
        {
            get => steerValue;
        }

        public float AccelerateValue
        {
            get => accelerateValue;
        }

        public float BrakeValue
        {
            get => brakeValue;
        }

        public bool IsAccelerating
        {
            get => isAccelerating;
        }

        private void Awake()
        {
            AccelerationValues.Insert(0, 0);
            AccelerationValues.Insert(1, 0);
            AccelerationValues.Insert(2, 0);
        }


        private void Update()
        {
            brakeValue = brakeInput.action.ReadValue<float>();//Trigger Button

            if (Mathf.Abs(brakeValue) == 0)
            {
                Vector2 accel = accelerationInput.action.ReadValue<Vector2>();//JoyStick

                float difference = CalculateDifference(accel);

                if (accelerateValue <= 0.9 && difference <= 0.002)
                {
                    isAccelerating = false;
                }
                else
                {
                    isAccelerating = true;
                }

                AccelerationValues.Insert(AccelerationValues.Count, accelerateValue);
                AccelerationValues.RemoveAt(0);

            }
            else
            {
                accelerateValue = 0;
            }
        }

        /// <summary>
        /// Calculates Input Difference for VR joysteack to maintain speed (Simulating Gas Pedal)
        /// </summary>
        /// <param name="accel"></param>
        /// <returns></returns>
        private float CalculateDifference(Vector2 accel)
        {
            decimal accelerate = Math.Round((decimal)accel.y, 3);
            decimal averageAccelerate = 0;

            foreach (float item in AccelerationValues)
            {
                averageAccelerate += Math.Round((decimal)item, 3);
            }

            averageAccelerate = averageAccelerate / AccelerationValues.Count;
            lastAccelerationValue = (float)averageAccelerate;
            accelerateValue = (float)accelerate;
            float difference = Mathf.Abs(accelerateValue - lastAccelerationValue);
            return difference;
        }
    }
}