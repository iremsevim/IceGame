using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using Cinemachine;


public enum CameraType
{
    CharacterFollow,
    FinishCamera,
    AddCharacterMoment,
    PoliceChase,
    AfterChase
}
public class CameraActor : GameSingleActor<CameraActor>
{
    [Header("All CameraDatas")]
    public List<CameraProfil> cameraProfils;
    public CinemachineVirtualCamera firstFollowCamera;
    public CinemachineVirtualCamera policeCam;


    public override void ActorStart()
    {
        SwitchCamera(CameraType.PoliceChase);
    }
    

    public void SwitchCamera(CameraType currentcamera)
    {
      CameraProfil findedcamera= cameraProfils.Find(x => x.cameraType == currentcamera);
        if (findedcamera == null) return;
        firstFollowCamera = findedcamera.camera.GetComponent<CinemachineVirtualCamera>();
        cameraProfils.ForEach(x => x.camera.SetActive(false));
        findedcamera.camera.SetActive(true);
    }
    public void SwitchCameraUpdateMode(CinemachineBrain.UpdateMethod updateMethod)
    {
        Camera.main.GetComponent<CinemachineBrain>().m_UpdateMethod = updateMethod;
         
    }

    [System.Serializable]
    public class CameraProfil
    {
        public CameraType cameraType;
        public GameObject camera;
      
    }
}
