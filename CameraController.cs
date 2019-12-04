using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //player object to follow/watch
    public Transform playerObject;
    //use it to have a distance between our player and our camera
    //this type of endless runner never moves on the x axis
    //the y gives us a 5 up view
    //the z gives us 10 metre away
    public Vector3 offset = new Vector3(0, 2.5f, -2f);
    public Vector3 rotation = new Vector3(35, 0, 0);

    public bool IsMoving { set; get; }

    private void LateUpdate()
    {
        if (!IsMoving)
            return;
        //make sure the player moves first before the camera moves with it
        //this is where we should be
        Vector3 targetPos = playerObject.position + offset;
        //this is for not to be at x of player always 0 for x because we don't
        //move when lanes move
        targetPos.x = 0;
        //this is for smoothing so it's not jumpy
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.1f);
    }
}
