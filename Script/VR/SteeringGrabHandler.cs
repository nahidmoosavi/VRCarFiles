using UnityEngine;

namespace VRCar
{
    /// <summary>
    /// When Palyer touches red handles on steering wheel, the material changes form Red to transparent
    /// </summary>
    public class SteeringGrabHandler : MonoBehaviour
    {
        [SerializeField] MaterialChanger changer;

        private bool isTouched = false;

        public bool IsTouched
        {
            get => isTouched;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerHand"))
            {
                changer.HalfTarnsparent();
                isTouched = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerHand"))
            {
                changer.Transparent();
                isTouched = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerHand"))
            {
                changer.Highlight();
                isTouched = false;
            }
        }
    }
}
