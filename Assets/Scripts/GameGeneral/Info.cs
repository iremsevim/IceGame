using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Info : MonoBehaviour
{
    public List<Sprite> Countdown;
    public Sprite gO;
    public Image mainCounter;
    public Image goCounter;
    public Image tutorial;
   
    public void Start()
    {
        StartCoroutine(ShowCounter());
    }
    private IEnumerator ShowCounter()
    {
        foreach (var item in Countdown)
        {
            mainCounter.sprite = item;
            mainCounter.rectTransform.DOScale(mainCounter.rectTransform.localScale * 1.35f, 0.5f).
                OnComplete(() => 
                {
                    mainCounter.rectTransform.DOScale(mainCounter.rectTransform.localScale / 1.35f, 0.5f);
                });
            yield return new WaitForSeconds(1f);

           
        }
        mainCounter.gameObject.SetActive(false);
        goCounter.gameObject.SetActive(true);
        goCounter.rectTransform.DOScale(goCounter.rectTransform.localScale * 1.65f, 0.5f);
        goCounter.sprite = gO;
        yield return new WaitForSeconds(1.5f);
        goCounter.gameObject.SetActive(false);
        tutorial.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        NextLevel();
        
         }
    public void NextLevel()
    {
        SceneManager.LoadScene("Game");
    }
}
