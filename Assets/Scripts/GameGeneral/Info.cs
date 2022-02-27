using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Info : MonoBehaviour
{
    public static bool displayed;
    public List<Sprite> Countdown;
    public Sprite gO;
    public Image mainCounter;
    public Image goCounter;
    public Image tutorial;
    public static System.Action onClosedPanel;
    public GameObject mainClosedObject;

    public void Start()
    {
        StartCoroutine(ShowCounter());
    }
    private IEnumerator ShowCounter()
    {
        
        foreach (var item in Countdown)
        {
            mainCounter.sprite = item;
            mainCounter.rectTransform.DOScale(mainCounter.rectTransform.localScale * 1.15f, 0.5f).
                OnComplete(() =>
                {
                    mainCounter.rectTransform.DOScale(mainCounter.rectTransform.localScale / 1.5f, 0.5f);
                });
            yield return new WaitForSeconds(1f);


        }
        mainCounter.gameObject.SetActive(false);
        goCounter.gameObject.SetActive(true);
        goCounter.sprite = gO;
        goCounter.rectTransform.DOScale(goCounter.rectTransform.localScale * 1.65f, 0.5f);
        yield return new WaitForSeconds(1f);
        mainClosedObject.SetActive(false);
        onClosedPanel?.Invoke();
        displayed = true;
    }
}
