using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Coskunerov.Actors;
using UnityEngine.UI;

public class UIActor : GameSingleActor<UIActor>
{
    public RectTransform letterpanel;
    private Vector3 letterPanelFirstPos;
    public Text typedletters;
    private Color panelColor;

    public override void ActorAwake()
    {
        letterPanelFirstPos = letterpanel.GetComponent<RectTransform>().position;
        panelColor = letterpanel.GetComponent<Image>().color;
    }

    public void ShowHideLetterPanel(bool status)
    {
        if(status)
        {
            letterpanel.DOLocalMoveY(letterPanelFirstPos.y-230, 0.75f);
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
}
