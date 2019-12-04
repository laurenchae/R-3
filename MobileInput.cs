using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    private const float DEADZONE = 100;
    //gets this instance to refer to from the outside
    public static MobileInput Instance { set; get; }

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    //swipe delta is distance to move in respective direction
    //need start to see where you start tapping and holding down to calculate delta
    private Vector2 swipeDelta, startTouch;

    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private void Awake()
    {
        //need script somewhere in scene and it instantly binds itself to the isntance
        Instance = this;
    }

    private void Update()
    {
        //always right to left
        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;

        //check for inputs
        #region Standalone Inputs
        //if it was for PC only, checking mouse inputs
        //left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            //true in this frame since it resets every frame
            tap = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }
        #endregion

        #region Mobile Inputs
        //mobile checks finger input
        if (Input.touches.Length != 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }

        }

        #endregion

        //calculate distance between start touch
        swipeDelta = Vector2.zero;
        //we did tap somewhere touched
        if(startTouch != Vector2.zero)
        {
            //with mobile
            if(Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            //standalone
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //need a deadzone
        if(swipeDelta.magnitude > DEADZONE)
        {
            //this is a valid swipe but what direction?
            //check which has biggest magnitude to determine direction
            //which vector is longest
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right
                //negative
                if(x < 0)
                {
                    swipeLeft = true;
                }
                //positive
                else
                {
                    swipeRight = true;
                }
            }

            else
            {
                //up or down
                if (y < 0)
                {
                    swipeDown = true;
                }
                //positive
                else
                {
                    swipeUp = true;
                }
            }

            startTouch = swipeDelta = Vector2.zero;
        }

    }
}
