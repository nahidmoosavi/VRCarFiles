using UnityEngine;
using Core;


namespace VRCar
{
    /// <summary>
    /// The main Class reads input from the VR controllers and drives the car according to the input
    /// A layer above the CarController and VRInputManager
    /// </summary>
    [RequireComponent(typeof(VRInputManager))]
    public class VRCarController : Car1
    {
        private VRInputManager vrInputManager;
        private VRSteeringWheelBoxColliderVersion vrSteeringWheel;


        private void Start()
        {
            vrInputManager = GetComponentInChildren<VRInputManager>();
            vrSteeringWheel = GetComponentInChildren<VRSteeringWheelBoxColliderVersion>();
        }


        private void FixedUpdate()
        {
            ChangeGears();
            AccelerateCar(vrInputManager.AccelerateValue, vrInputManager.IsAccelerating);
            SteerCar(vrSteeringWheel.SteerValue);
            BrakeCar(vrInputManager.BrakeValue);
            ShowSpeed();
            ApplyAntiRollBar();

        }

    }
}
