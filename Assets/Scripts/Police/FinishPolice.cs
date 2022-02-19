using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPolice : MonoBehaviour
{
    public Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
     
    }
    public IEnumerator Throw()
    {
        GameObject police=Instantiate(GameData.Instance.gameDataSO.policeRagdoll.gameObject, transform.position,transform.rotation);
        Ragdoll policeRagdoll=  police.GetComponent<Ragdoll>();
        Rigidbody rb=  policeRagdoll.pelvis.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(transform.up * 300 + transform.right * Random.Range(-1, 1) * 75,ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
    

}
