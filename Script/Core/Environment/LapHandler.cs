using System.Collections;
using UnityEngine;


namespace Environment
{
    public class LapHandler : MonoBehaviour
    {

        [SerializeField] Transform nextLoadPosition;

        [SerializeField] GameObject carBody;
        private SceneHandler sceneHandler;
        private Rigidbody carRigidbody;

        [SerializeField] int playedLaps = 0;

        private void Awake()
        {
            sceneHandler = FindObjectOfType<SceneHandler>();
            carRigidbody = carBody.GetComponent<Rigidbody>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("VRCar"))
            {
                playedLaps += 1;

                if (playedLaps >= 3 * InternalSceneManager.internalSceneManager.NumOfLaps)
                {
                    StartCoroutine(sceneHandler.FinalLapFade());
                    StartCoroutine(LoadToNextPosition());
                    InternalSceneManager.internalSceneManager.ChangeScene();
                }
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
