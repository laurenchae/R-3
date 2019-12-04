using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//size of the asset so that this is stacked perfectly
	private const float LANE_GAP = 2.5f;
    //the lerp or turn speed for velocity like to face where we go
    private const float ROT_SPEED = 0.05f;
    //tap to start function variable
    private bool turnOn = false;
	//character movement
	private CharacterController controller;
    private float jumpForce = 4.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;

    //speed modifier
    private float originalSpeed = 7.0f;
    private float speed = 7.0f;
    private float speedUpCount;
    //how long it takes to increase the difficulty
    //every 2.5 seconds we increase the speed by 0.1
    private float speedUpTime = 2.5f;
    private float speedUpFactor = 0.1f;

    //lane movement - 0 = left, 1 = centre, 2 = right
    private int lane = 1;
    
    //animation for jumping motion
    private Animator animator;

    public GameManager gameManager;

    private void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //this must be true, turned on to start the game aka the rest of the code
        if (!turnOn)
        {
            return;
        }
        //turn on means player running, if so we measure time since the last upgrade
        if(Time.time - speedUpCount > speedUpTime)
        {
            speedUpCount = Time.time;
            speed += speedUpFactor;
            GameManager.Instance.UpdateLevelSpeed(speed - originalSpeed);
        }

        //updating direction we are swiping between lanes
        if (MobileInput.Instance.SwipeLeft){
            //move left
            MoveLane(false);
        }
        if (MobileInput.Instance.SwipeRight)
        {
            //move right
            MoveLane(true);
        }

        //calculating lane to move to - this is getting the vector to move to/with

        //first, we always move forward - current position * the forward direction
        //gives 0, 0, and a z position
        Vector3 targetPosition = transform.position.z * Vector3.forward;

        //if we are going to left lane - go to the left by the distance between lanes
        //in order to end up in left lane
        if(lane == 0)
        {
            targetPosition += Vector3.left * LANE_GAP;
        }
        else if(lane == 2)
        {
            targetPosition += Vector3.right * LANE_GAP;
        }

        //calculate the move delta vector
        Vector3 distanceVector = Vector3.zero;
        //how far to move in x to get to our target position
        //normalized - make sure it is only based off a single metre
        targetPosition.y = Mathf.Clamp(targetPosition.y, -1, jumpForce);
		distanceVector.x = (targetPosition - transform.position).x * speed;
        //calculating our Y
        bool isGrounded = IsGrounded();
		animator.SetBool("Grounded", isGrounded);
		//calculate Y we use gravity factor - changes when jumping
		if (IsGrounded())
		{
            //if we are grounded, snap to floor at all times
            verticalVelocity = -0.1f;

			if (MobileInput.Instance.SwipeUp)
			{
                //jump
                animator.SetTrigger("Jump");
                verticalVelocity = jumpForce;
			}
		}
		else
		{
            //number always goes down when you're not grounded
            //this is for you to transition back down to ground when jumping
            verticalVelocity -= (gravity * Time.deltaTime);

			//way to fall faster aka right away by swiping down, space bar again for now
			if (MobileInput.Instance.SwipeDown)
			{
                verticalVelocity = -jumpForce;
			}
		}
		//keep us grounded for now
		distanceVector.y = verticalVelocity;
        //how far out we go in a single frame is the speed
        distanceVector.z = speed;

        //actual movement of player
        //Time - so thing runs on same speed no matter what frame rate is
        //if super laggy still run on same speed - consistency
        controller.Move(distanceVector * Time.deltaTime);

        //rotate the player to where they're going
        //point 00 to point 0,1 velocity is 0,1
        Vector3 direction = controller.velocity;
        if(direction != Vector3.zero)
		{
			//kill y axis because player can jump and stuff
			direction.y = 0;
			//smooth the turning of body to direction we are going
			transform.forward = Vector3.Lerp(transform.forward, direction, ROT_SPEED);
		}


    }

    private void MoveLane(bool toRight)
    {
        //if toRight is true we increment 1 if false we decrement 1
        lane += toRight ? 1 : -1;
        //clamp the lane number between 0 and 2 so we don't get -1 and 3 which
        //is out of range
        lane = Mathf.Clamp(lane, 0, 2);
    }

    private bool IsGrounded()
	{
        //2 parameter object that takes an original position and a direction
        //original - takes center of character controller aka pivot respective x,y
        //positions.
        //x is just the x of pivot point so the centre of char controller by the feet
        //y takes the middle of the controller capsule but subtract the radius like half the height which is extent to the bottom but add
        //0.2 so that we are in the capsule
        //z is just the center z position of pivot
        //altogether this is basically a little above our pivot point
        //direction is down bc we are casting a ray from above pivot point down to the floor
        Ray groundRay = new Ray(
            new Vector3(
                controller.bounds.center.x,
                (controller.bounds.center.y-controller.bounds.extents.y) + 0.2f,
                controller.bounds.center.z),
            Vector3.down);
        //a ray from our ground ray to the ground well a little below it to give buffer
        //returns true if it is grounded false if not
        return(Physics.Raycast(groundRay, 0.2f + 0.1f));
		
	}

    public void TurnOn() //start running variable for tutorial
    {
        turnOn = true;
        animator.SetTrigger("StartRun");
    }

    private void Trip()
    {
        animator.SetTrigger("Trip"); //need time delay tho
        animator.SetTrigger("SecondChance");
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        turnOn = false;
        GameManager.Instance.OnDeath();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Trash":
                if (gameManager.lives == 0)
                {
                    Die();
                }
                else
                {
                    gameManager.lives--;
                    Trip();
                }
                break;

            case "Recyclable":
                gameManager.score++;
                break;
        }
    }
}
