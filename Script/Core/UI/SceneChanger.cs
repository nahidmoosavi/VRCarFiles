
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] Animator fadeAnimator;
    private int nextLevel;

   

    public void LoadNextLevel()
    {
        FadeOut(SceneManager.GetActiveScene().buildIndex + 1);
        SceneSelector.sceneSelector.RnadomSceneGenerator();
    }


    private void FadeOut(int sceneIndex)
    {
        nextLevel = sceneIndex;
        fadeAnimator.SetTrigger("FadeOutTrigger");
    }

    public void OnAnimationEnd()
    {
        SceneManager.LoadScene(nextLevel);
    }



    //public IEnumerator FadeInOut()
    //{
    //    fadeAnimator.SetTrigger("FadeOutTrigger");
    //    yield return new WaitForSeconds(2);
    //    fadeAnimator.SetTrigger("FadeInTrigger");

        
    //}
}
