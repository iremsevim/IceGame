using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Coskunerov.Actors;

public class UIActor : GameSingleActor<UIActor>
{
    public RectTransform letterpanel;
    private Vector3 letterPanelFirstPos;
   
    public override void ActorAwake()
    {
        letterPanelFirstPos = letterpanel.GetComponent<RectTransform>().position;
    }

    public void ShowHideLetterPanel(bool status)
    {
        if(status)
        {
            letterpanel.DOLocalMoveY(letterPanelFirstPos.y-230f, 0.75f);
        }
        else
        {
            letterpanel.DOMove(letterPanelFirstPos, 0.75f);
        }
    }
}
