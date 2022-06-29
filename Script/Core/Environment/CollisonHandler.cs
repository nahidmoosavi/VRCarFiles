using System.Collections;
using UnityEngine;

namespace Environment
{
    public class CollisonHandler : MonoBehaviour
    {
        [SerializeField] Transform nextLoadPosition;

        [SerializeField] GameObject carBody;
        private SceneHandler sceneHandler;
        private Rigidbody carRigidbody;

        private void Awake()
        {
            sceneHandler = FindObjectOfType<SceneHandler>();
            carRigidbody = carBody.GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("VRCar"))
            {
                StartCoroutine(sceneHandler.Kill());
                StartCoroutine(LoadToNextPosition());
            }
        }


        IEnumerator LoadToNextPosition()
        {

            carRigidbody.isKinematic = true;
            carBody.transform.position = nextLoadPosition.position;
            carBody.transform.rotation = nextLoadPosition.rotation;
            carRigidbody.velocity = new Vector3(0, 0, 0);
            yield return new WaitForFixedUpdate();
            carRigidbody.isKinematic = false;
        }
    }
}
