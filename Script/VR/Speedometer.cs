using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VRCar
{
    /// <summary>
    /// Accurately Shows Speedometer according to Car Speed
    /// </summary>
    public class Speedometer : MonoBehaviour
    {

        [SerializeField] Transform pin;
        [SerializeField] Transform speedLabel;
        [SerializeField] int numberOfSpeedLabels;
        [SerializeField] float maxSpeedLabel = 200;

        [SerializeField] GameObject car;

        private const float Max_PinAngle = -210; //When pin reaches the maximum Speed
        private const float Min_PinAngle = 20; //When pin reaches the Zero Speed

        private Transform pinTransform;
        private Transform labelTransform;


        private void Awake()
        {
            pinTransform = pin;
            labelTransform = speedLabel;
            labelTransform.gameObject.SetActive(false);

            CreateSpeedLabels();

        }

        /// <summary>
        /// Generate Speed Lables on real time
        /// </summary>
        private void CreateSpeedLabels()
        {
            float totalAngle = Min_PinAngle - Max_PinAngle; ;// -Min_PinAngle + Max_PinAngle;
            for (int i = 0; i <= numberOfSpeedLabels; i++)
            {
                Transform speedLabelTransform = Instantiate(labelTransform, transform);
                float lableTransformNormalized = (float)i / numberOfSpeedLabels;
                float speedLabelAngle = -Max_PinAngle - lableTransformNormalized * totalAngle;
                speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
                speedLabelTransform.Find("SpeedText").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(lableTransformNormalized * maxSpeedLabel).ToString();
                speedLabelTransform.Find("SpeedText").eulerAngles = Vector3.zero;
                speedLabelTransform.gameObject.SetActive(true);
            }

            pinTransform.SetAsLastSibling();
            pinTransform.gameObject.SetActive(false);

        }

        /// <summary>
        /// Sets speedometer transform always in it's position
        /// </summary>
        /// <param name="normalizedSpeed"></param>
        public void SpeedometerRotation(float normalizedSpeed)
        {
            float totalAngle = Min_PinAngle - Max_PinAngle;

            float rotationValue = Min_PinAngle + 5 - normalizedSpeed * totalAngle;

            pinTransform.eulerAngles = new Vector3(0, car.transform.rotation.eulerAngles.y, rotationValue);
        }
    }
}
