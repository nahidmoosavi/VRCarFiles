using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SceneHandler : MonoBehaviour
{
    [SerializeField] Animator redFadeAnimator;
    [SerializeField] Image image;

    private Color color;
    private Color color1 = Color.grey; 

    private void OnEnable()
    {
        color = image.color;
        image.color = Color.clear;
    }

    public IEnumerator Kill()
    {
        image.color = color;
        redFadeAnimator.SetTrigger("FadeOutTrigger");
        redFadeAnimator.SetTrigger("FadeInTrigger");
        yield return new WaitForSeconds(2);

    }


    public IEnumerator FinalLapFade()
    {
        image.color = color1;
        redFadeAnimator.SetTrigger("FadeOutTrigger");
        redFadeAnimator.SetTrigger("FadeInTrigger");
        yield return new WaitForSeconds(2);

    }

}
