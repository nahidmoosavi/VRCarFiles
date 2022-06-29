
using UnityEngine;
using UnityEngine.InputSystem;


namespace VRCar
{
    /// <summary>
    /// This script handles Virtual steering Wheel (Project does not support Physical Steering wheel controller)
    /// VR Steering wheel maps the users hand position and rotation from the real world to VR steering wheel movement
    /// and navigates the car according to the hand input
    /// </summary>

    public class VRSteeringWheelBoxColliderVersion : MonoBehaviour
    {
        [SerializeField] GameObject LeftHandObj, rightHandObj;

        [SerializeField] Transform steeringWheelDirection;
        [SerializeField] GameObject[] handSnapPosition = new GameObject[2];

        [SerializeField] GameObject car;

        [SerializeField] InputActionReference releaseInput = null;

        private bool isRightHandOnSteering, isLeftHandOnSteering = false;
        private float steerValue = 0;
        private Transform rightHandTransform, leftHandTransform;
        private SteeringGrabHandler steeringGrabHandlerRight, SteeringGrabHandlerLeft;
        private bool isReleased = false;

        const float steeringWheelDeadZone = 10;

        public float currentSteeringWheelRotation;

        public float SteerValue
        {
            get => steerValue;
        }



        private void Start()
        {
            steeringGrabHandlerRight = handSnapPosition[0].GetComponent<SteeringGrabHandler>();
            SteeringGrabHandlerLeft = handSnapPosition[1].GetComponent<SteeringGrabHandler>();
            releaseInput.action.started += ReleaseSteeringWheel;

        }


        private void Update()
        {
            if (isReleased)
            {
                ReleaseHandsOnWheel();
            }

            HandRotationToSteeringWheelRotation();

        }

        private void FixedUpdate()
        {
            CalculateSteerValue();
        }

        /// <summary>
        /// Reads Grip Value
        /// </summary>
        /// <param name="context"></param>
        private void ReleaseSteeringWheel(InputAction.CallbackContext context)
        {
            isReleased = true;
        }


        /// <summary>
        /// This method converts usre's hand positin to numbers between [0,1]
        /// to mimic the physical steering wheel controlers
        /// Todo try to use a math calculation instead of Switch
        /// </summary>
        private void CalculateSteerValue()
        {
            currentSteeringWheelRotation = this.transform.rotation.eulerAngles.z;

            float steerAngle = Mathf.Round(currentSteeringWheelRotation);

            switch (steerAngle)
            {
                case (> steeringWheelDeadZone and <= 90):
                    steerAngle = -steerAngle / 90;
                    break;

                case (>= 270 and < 360 - steeringWheelDeadZone):
                    steerAngle -= 360;
                    steerAngle = -steerAngle / 90;
                    break;

                case (> 90 and <= 180):
                    steerAngle -= 90;
                    steerAngle = -steerAngle / 90;
                    steerAngle -= 1;
                    break;

                case (> 180 and < 270):
                    steerAngle -= 270;
                    steerAngle = -steerAngle / 90;
                    steerAngle += 1;
                    break;

                case ((>= 0 and <= steeringWheelDeadZone) or (<= 360 and >= 360 - steeringWheelDeadZone)):
                    steerAngle = 0;
                    break;

                default:
                    break;
            }

            steerValue = steerAngle;
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerHand"))
            {
                CheckHandsOnWheel();
            }
        }

        private void OnTriggerExit(Collider other)
        {

            if (other.gameObject.CompareTag("PlayerHand"))
            {
                ReleaseAllHands();
            }
        }

        /// <summary>
        /// If user hands touched the virtual sterring wheel, locks the hands to the handles, until they press grip button
        /// By this they can put their hands in a relaxing state
        /// </summary>
        private void CheckHandsOnWheel()
        {

            if (steeringGrabHandlerRight.IsTouched == true && isRightHandOnSteering == false)
            {
                PlaceHand(ref rightHandObj, ref rightHandTransform, ref isRightHandOnSteering, handSnapPosition[0]);
            }
            if (SteeringGrabHandlerLeft.IsTouched == true && isLeftHandOnSteering == false)
            {
                PlaceHand(ref LeftHandObj, ref leftHandTransform, ref isLeftHandOnSteering, handSnapPosition[1]);
            }
        }


        private void PlaceHand(ref GameObject hand, ref Transform handTransform, ref bool onWheel, GameObject snapPosition)
        {
            handTransform = hand.transform.parent;

            hand.transform.parent = snapPosition.transform;
            hand.transform.position = snapPosition.transform.position;

            onWheel = true;
        }


        private void ReleaseAllHands()
        {
            if (isRightHandOnSteering == true)
            {
                ReleaseHand(ref rightHandObj, ref isRightHandOnSteering, rightHandTransform);
            }

            if (isLeftHandOnSteering == true)
            {
                ReleaseHand(ref LeftHandObj, ref isLeftHandOnSteering, leftHandTransform);
            }


            if (isRightHandOnSteering == false && isLeftHandOnSteering == false)
            {
                transform.parent = transform.root;
            }
        }

        /// <summary>
        /// Release hands from steering wheel when grip button is presseds
        /// </summary>
        private void ReleaseHandsOnWheel()
        {
            if (isRightHandOnSteering == true)
            {
                ReleaseHand(ref rightHandObj, ref isRightHandOnSteering, rightHandTransform);
            }

            if (isLeftHandOnSteering == true)
            {
                ReleaseHand(ref LeftHandObj, ref isLeftHandOnSteering, leftHandTransform);
            }


            if (isRightHandOnSteering == false && isLeftHandOnSteering == false)
            {
                transform.parent = transform.root;
            }

            isReleased = false;
        }

        private void ReleaseHand(ref GameObject hand, ref bool onWheel, Transform handTransform)
        {
            hand.transform.parent = handTransform;
            hand.transform.position = handTransform.position;
            hand.transform.rotation = handTransform.rotation;
            onWheel = false;
        }

        /// <summary>
        /// Conbverts the user hand rotation to steering wheel's rotation
        /// </summary>
        private void HandRotationToSteeringWheelRotation()
        {
            if (isRightHandOnSteering == true && isLeftHandOnSteering == false)
            {
                Quaternion newRot = Quaternion.Euler(0, car.transform.rotation.eulerAngles.y, rightHandTransform.rotation.eulerAngles.z);
                steeringWheelDirection.rotation = newRot;
                this.transform.parent = steeringWheelDirection;
            }
            else if (isRightHandOnSteering == false && isLeftHandOnSteering == true)
            {
                Quaternion newRot = Quaternion.Euler(0, car.transform.rotation.eulerAngles.y, leftHandTransform.rotation.eulerAngles.z);
                steeringWheelDirection.rotation = newRot;
                this.transform.parent = steeringWheelDirection;
            }
            else if (isRightHandOnSteering == true && isLeftHandOnSteering == true)
            {
                Quaternion newRotRight = Quaternion.Euler(0, car.transform.rotation.eulerAngles.y, rightHandTransform.rotation.eulerAngles.z);
                Quaternion newRotLeft = Quaternion.Euler(0, car.transform.rotation.eulerAngles.y, rightHandTransform.rotation.eulerAngles.z);
                Quaternion newRot = Quaternion.Slerp(newRotLeft, newRotRight, 1.0f / 2.0f);


                steeringWheelDirection.rotation = newRot;
                this.transform.parent = steeringWheelDirection;
            }
        }

        private void OnDestroy()
        {
            releaseInput.action.started -= ReleaseSteeringWheel;
        }
    }
}
