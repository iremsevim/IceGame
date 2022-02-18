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
        rb.useGravity = true;
        rb.AddForce(transform.up * 50 + transform.right * Random.Range(-1, 1) * 35,ForceMode.Impulse);
        yield return new WaitForSeconds(3F);
        Destroy(gameObject);
    }
    

}
