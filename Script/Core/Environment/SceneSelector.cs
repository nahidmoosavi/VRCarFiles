using System;

using UnityEngine;


public class SceneSelector: MonoBehaviour
{
    public static SceneSelector sceneSelector;

    public int sceneNumber = 0;
    public int speedLimit = 0;
    public int rand = 0;

    //public bool denseScene = false;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(sceneSelector == null)
        {
            sceneSelector = this;
        }
        else if(sceneSelector != this)
        {
            Destroy(gameObject);
        }
    }

    public void RnadomSceneGenerator()
    {
        System.Random random = new System.Random();

        int rand = random.Next(0,100);
        rand = rand % 2;
        int speedNum = random.Next(0, 10);


        if (speedNum < 5)
        {
            speedLimit = 30;
        }
        else
        {
            speedLimit = 50;
        }


        if(rand == 0)
        {
            sceneNumber = 0;
        }
        else
        {
            sceneNumber = 1;
        }
    }


}
