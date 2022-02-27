using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Game Data",menuName ="Ice Game/Game Data")]
public class GameDataSO : ScriptableObject
{
    public GameObject characterPrefab;
    public Ragdoll policeRagdoll;
    public Ragdoll playerRagdoll;
    public List<IceChar> iceCharsPrefabs;

    private int index = 0;
    public IceChar GetIceChar
    {
        get
        {
            index++;
            if (index > iceCharsPrefabs.Count - 1) index = 0;
            return iceCharsPrefabs[index];
        }
    }
}
