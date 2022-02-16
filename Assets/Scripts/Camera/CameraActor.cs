using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using Cinemachine;


public enum CameraType
{
    CharacterFollow,
    FinishCamera,
    AddCharacterMoment
}
public class CameraActor : GameSingleActor<CameraActor>
{
    [Header("All CameraDatas")]
    public List<CameraProfil> cameraProfils;
    public CinemachineVirtualCamera firstFollowCamera;


    public override void ActorStart()
    {
        SwitchCamera(CameraType.CharacterFollow);
    }
    

    public void SwitchCamera(CameraType currentcamera)
    {
      CameraProfil findedcamera= cameraProfils.Find(x => x.cameraType == currentcamera);
        if (findedcamera == null) return;
        firstFollowCamera = findedcamera.camera.GetComponent<CinemachineVirtualCamera>();
        cameraProfils.ForEach(x => x.camera.SetActive(false));
        findedcamera.camera.SetActive(true);
    }

    [System.Serializable]
    public class CameraProfil
    {
        public CameraType cameraType;
        public GameObject camera;
      
    }
}
