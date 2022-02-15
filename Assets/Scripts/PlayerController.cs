using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;

public class PlayerController : GameSingleActor<PlayerController>
{
    [Header("Companents")]
    public Animator anim;
    public Rigidbody rb;
    public InputManager inputManager;
    [Header("Refs")]
    public float movementSpeed;
    public float rotateSpeed;
    public override void ActorStart()
    {
        anim.SetBool("run", true);
    }
    public override void ActorFixedUpdate()
    {
        Movement();
    }
    public void Movement()
    {
        rb.velocity = new Vector3(inputManager.Result * rotateSpeed, rb.velocity.y, movementSpeed);
        float rot_y = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
        rot_y = Mathf.Clamp(rot_y, -25, 25);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.localEulerAngles.x, rot_y, transform.localEulerAngles.x), 0.5f);


    }
    public void OnTouchedIceGroupCollider(IceGroupCarrier touchedGroup)
    {

    }

}
