using UnityEngine;

namespace Environment
{
    public class SpeedOnSign : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("VRCar"))
            {
                float speed = Mathf.RoundToInt(other.attachedRigidbody.velocity.magnitude * 3.6f);
                Debug.LogWarning("The Collision Speed: " + speed);
            }
        }
    }
}
