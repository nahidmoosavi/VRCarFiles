using UnityEngine;
using TMPro;

namespace VRCar
{
    /// <summary>
    /// This script handles Virtual steering Wheel (Project does not support Physical Steering wheel controller)
    /// VR Steering wheel maps the users hand position and rotation from the real world to VR steering wheel movement
    /// and navigates the car according to the hand input
    /// </summary>
    public class VRSteeringWheel : MonoBehaviour
    {
        [SerializeField] GameObject LeftHandObj, rightHandObj;

        [SerializeField] Transform steeringWheelDirection;
        [SerializeField] GameObject[] handSnapPosition = new GameObject[2];

        [SerializeField] GameObject car;
        [SerializeField] TextMeshProUGUI textObj;

        private bool isRightHandOnSteering, isLeftHandOnSteering = false;
        private float steerValue = 0;
        private Transform rightHandTransform, leftHandTransform;
        private SteeringGrabHandler steeringGrabHandlerRight, SteeringGrabHandlerLeft;

        public float currentSteeringWheelRotation;

        public float SteerValue
        {
            get => steerValue;
        }


        private void Start()
        {
            steeringGrabHandlerRight = handSnapPosition[0].GetComponent<SteeringGrabHandler>();
            SteeringGrabHandlerLeft = handSnapPosition[1].GetComponent<SteeringGrabHandler>();
        }

        private void Update()
        {
            ReleaseHandsOnWheel();
            //ReleaseAllHands();
            CheckHandsOnWheel();


            HandRotationToSteeringWheelRotation();

            CalculateSteerValue();
        }



        /// <summary>
        /// This method converts usre's hand positin to numbers between [0,1]
        /// to mimic the physical steering wheel controlers
        /// Todo try to use a math calculation instead of if-elses
        /// </summary>
        private void CalculateSteerValue()
        {
            currentSteeringWheelRotation = this.transform.rotation.eulerAngles.z;

            float steerAngle = Mathf.Round(currentSteeringWheelRotation);

            if (steerAngle > 0 && steerAngle <= 90)
            {
                steerAngle = Mathf.Clamp(steerAngle, 0, 90);
                steerAngle = -steerAngle / 90;
            }
            else if (steerAngle >= 270 && steerAngle < 360)
            {
                steerAngle = Mathf.Clamp(steerAngle, 270, 360);
                steerAngle -= 360;
                steerAngle = -steerAngle / 90;
            }
            else if (steerAngle > 90 && steerAngle <= 180)
            {
                steerAngle = Mathf.Clamp(steerAngle, 90, 180);
                steerAngle = -steerAngle;
                steerAngle = Mathf.Clamp(steerAngle, -1, 0);
                //steerAngle -= 180;
                //steerAngle = steerAngle / 90;
            }
            else if (steerAngle > 180 && steerAngle < 270)
            {
                steerAngle = Mathf.Clamp(steerAngle, 180, 270);
                steerAngle = Mathf.Clamp(steerAngle, 0, 1);
                //steerAngle -= 270;
                //steerAngle = -steerAngle / 90;
            }
            else if (steerAngle == 0 || steerAngle == 360)
            {
                steerAngle = 0;
            }
            steerValue = steerAngle;

            textObj.text = steerValue.ToString();
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

        /// <summary>
        /// Release hands from steering wheel when grip button is presseds
        /// </summary>
        private void ReleaseHandsOnWheel()
        {
            if (steeringGrabHandlerRight.IsTouched == false && isRightHandOnSteering == true)
            {
                ReleaseHand(ref rightHandObj, ref isRightHandOnSteering, rightHandTransform);
            }

            if (SteeringGrabHandlerLeft.IsTouched == false && isLeftHandOnSteering == true)
            {
                ReleaseHand(ref LeftHandObj, ref isLeftHandOnSteering, leftHandTransform);
            }


            if ((steeringGrabHandlerRight.IsTouched == false && isRightHandOnSteering == false) && (SteeringGrabHandlerLeft.IsTouched == false && isLeftHandOnSteering == false))
            {
                transform.parent = transform.root;
            }
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
    }
}


//private void ReleaseAllHands()
//{
//    if (isRightHandOnSteering == true)
//    {
//        ReleaseHand(ref rightHandObj, ref isRightHandOnSteering, rightHandTransform);
//    }

//    if (isLeftHandOnSteering == true)
//    {
//        ReleaseHand(ref LeftHandObj, ref isLeftHandOnSteering, leftHandTransform);
//    }


//    if (isRightHandOnSteering == false && isLeftHandOnSteering == false)
//    {
//        transform.parent = transform.root;
//    }
//}