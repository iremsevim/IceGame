using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Game Data",menuName ="Ice Game/Game Data")]
public class GameDataSO : ScriptableObject
{
    public GameObject characterPrefab;
    public Ragdoll policeRagdoll;
    public Ragdoll playerRagdoll;
    
}
