using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// This Class can controls other cars in the scene but the VR car
    /// </summary>
    public class CarController : Car
    {
        [SerializeField] private float brakeValue;
 

        private float steer;
        private float accelerate;
        private float brake;


        // Update is called once per frame
        void Update()
        {
            accelerate = Input.GetAxis("Vertical");
            steer = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                brake = brakeValue;
            }
            else
            {
                brake = 0;
            }

        }

        private void FixedUpdate()
        {
            ChangeGears();
            AccelerateCar(accelerate);
            SteerCar(steer);
            BrakeCar(brake);
            ShowSpeed();
            ApplyAntiRollBar();
           
        }
    }
}
