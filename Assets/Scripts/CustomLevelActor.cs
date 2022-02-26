using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;

public class CustomLevelActor : LevelActor
{
    public Transform characterPoint;
    public IceGroupCarrier firsticeGroupCarrier;
    public override void SetupLevel()
    {
        GameObject character=Instantiate(GameData.Instance.gameDataSO.characterPrefab, characterPoint.position, Quaternion.identity);
        character.transform.SetParent(transform);
        StartCoroutine(Delay());
        CameraActor.Instance.policeCam.Follow = character.transform;
    }

    IEnumerator Delay() 
    {
        yield return new WaitForEndOfFrame();
        PlayerController.Instance.currentIceGroup = firsticeGroupCarrier;
    }
}
