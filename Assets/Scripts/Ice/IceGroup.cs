using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum IceType
{
    A,
    B,C, D, E, F, G, H,I,J,K,L,M,N,O,P,R,S,T,U,V,Y,Z
}
public class IceGroup : MonoBehaviour
{
    public List<IceProfile> iceProfiles;
    [System.Serializable]
    public class IceProfile
    {
        public IceType iceName;
        public Ice ice;
    }
   
}
