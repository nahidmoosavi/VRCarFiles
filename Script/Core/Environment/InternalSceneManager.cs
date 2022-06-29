using UnityEngine;
using TMPro;


namespace Environment
{
    public class InternalSceneManager : MonoBehaviour
    {
        public static InternalSceneManager internalSceneManager;


        [SerializeField] GameObject gameObject;
        [SerializeField] TextMeshProUGUI[] speedLimitTexts;
        [SerializeField] int numOfLaps = 0;

        public int NumOfLaps
        {
            get => numOfLaps;
        }

        public bool denseScene = false;

        private void Awake()
        {

            gameObject.SetActive(false);

            if (SceneSelector.sceneSelector.sceneNumber == 0)
            {
                gameObject.SetActive(false);
                denseScene = false;
            }
            else
            {
                gameObject.SetActive(true);
                denseScene = true;
            }

            foreach (var item in speedLimitTexts)
            {
                item.text = SceneSelector.sceneSelector.speedLimit.ToString();
            }

            if (SceneSelector.sceneSelector.speedLimit == 30)
            {
                SceneSelector.sceneSelector.speedLimit = 50;
            }
            else if (SceneSelector.sceneSelector.speedLimit == 50)
            {
                SceneSelector.sceneSelector.speedLimit = 30;
            }

            DontDestroyOnLoad(gameObject);

            if (internalSceneManager == null)
            {
                internalSceneManager = this;
            }
            else if (internalSceneManager != this)
            {
                Destroy(gameObject);
            }


        }


        private void Start()
        {
            gameObject.SetActive(false);

            if (SceneSelector.sceneSelector.sceneNumber == 0)
            {
                gameObject.SetActive(false);
                denseScene = false;
            }
            else
            {
                gameObject.SetActive(true);
                denseScene = true;
            }

            foreach (var item in speedLimitTexts)
            {
                item.text = SceneSelector.sceneSelector.speedLimit.ToString();
            }

            if (SceneSelector.sceneSelector.speedLimit == 30)
            {
                SceneSelector.sceneSelector.speedLimit = 50;
            }
            else if (SceneSelector.sceneSelector.speedLimit == 50)
            {
                SceneSelector.sceneSelector.speedLimit = 30;
            }
        }


        public void ChangeScene()
        {
            foreach (var item in speedLimitTexts)
            {
                item.text = SceneSelector.sceneSelector.speedLimit.ToString();
            }

            if (denseScene == false)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }
}
