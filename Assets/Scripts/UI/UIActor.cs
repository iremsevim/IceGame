using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Coskunerov.Actors;
using UnityEngine.UI;
using Coskunerov.EventBehaviour;
using Coskunerov.EventBehaviour.Attributes;
using ElephantSDK;


public class UIActor : GameSingleActor<UIActor>
{
    public RectTransform letterpanel;
    private Vector3 letterPanelFirstPos;
    public Text typedletters;
    private Color panelColor;
    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject failPanel;

    public override void ActorAwake()
    {
        letterPanelFirstPos = letterpanel.GetComponent<RectTransform>().position;
        panelColor = letterpanel.GetComponent<Image>().color;
    }

    public void ShowHideLetterPanel(bool status)
    {
        if(status)
        {
            letterpanel.DOLocalMoveY(letterPanelFirstPos.y-300, 0.75f);
        }
        else
        {
            letterpanel.DOMove(letterPanelFirstPos, 0.75f);
        }
    }
    public IEnumerator ClearTypedLetter(bool status)
    {
        if(status)
        {
            letterpanel.GetComponent<Image>().color = Color.green;
            typedletters.text = string.Empty;
            yield return new WaitForSeconds(2F);
            letterpanel.GetComponent<Image>().color = panelColor;
        }
        else
        {
           
            typedletters.text = string.Empty;
            letterpanel.GetComponent<Image>().color = Color.red;
            letterpanel.DOShakePosition(0.5f, 5, 45).OnComplete(() => 
            {
                letterpanel.GetComponent<Image>().color = panelColor;
            });
          //  yield return new WaitForSeconds(1.5f);
            

        }
      


    }
    public void DeleteText(List<string> currentWords)
    {
        currentWords.RemoveAt(currentWords.Count - 1);
       typedletters.text = typedletters.text.Substring(0, typedletters.text.Length - 1);
    }


    public void NextLevel()
    {
        Coskunerov.Managers.GameManager.Instance.NextLevel();
       


    }

    [GE(BaseGameEvents.WinGame)]
    public void WinGame()
    {
        winPanel.SetActive(true);
        failPanel.SetActive(false);
        Elephant.LevelCompleted(Coskunerov.Managers.GameManager.Instance.runtime.currentLevelIndex);
    }
    [GE(BaseGameEvents.LoseGame)]
    public void FailGame()
    {
        winPanel.SetActive(false);
        failPanel.SetActive(true);
       Elephant.LevelFailed(Coskunerov.Managers.GameManager.Instance.runtime.currentLevelIndex);
    }
   
    public void Retry()
    {
     
        Coskunerov.Managers.GameManager.Instance.RestartLevel();
        PlayerController.Instance.currentWords.Clear();
        PlayerController.Instance.LevelLoaded();
       
     



    }
    [GE(BaseGameEvents.LevelLoaded)]
    public void LoadLevel()
    {
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        CameraActor.Instance.firstFollowCamera.Follow = PlayerController.Instance.transform;
        CameraActor.Instance.SwitchCamera(CameraType.PoliceChase);
        PlayerController.Instance.LevelLoaded();
        typedletters.text = string.Empty;
        PlayerController.Instance.currentWords.Clear();




    }
}
