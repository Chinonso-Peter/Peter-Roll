using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // State thy varaibles
    public ParticleSystem moveEffect; 
    private Rigidbody playerRb;
    private float speed = 30;
    private bool isMoving; 
    private Vector3 nextCollision; // gets the position of the next collision
    private Vector3 travelDestination; // direction in which the player should move
    private int minSwipe = 500; // knows when the player has made a swipe 
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;
    private Color ballColor;

    public GameManager gM;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        ballColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = ballColor;
        // gM = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    void Update()
    {
        moveEffect.transform.position = transform.position + new Vector3(0, -1, 0);
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            playerRb.velocity = speed * travelDestination; //sets the player to move in the direction that was swiped
            moveEffect.Play();
            
        }

        gM.CheckIfGameLevelIsFinished();

        Collider[] ballCollider = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f); // created a collider and set its position on the player 
        int i = 0;
        while(i < ballCollider.Length)
        {
            Ground ground = ballCollider[i].transform.GetComponent<Ground>();
            if (ground && !ground.isColored)
            {
                ground.ChangeColor(ballColor);
            }
            i++;
        }

        if (nextCollision != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollision) < 1)
            {
                isMoving = false;
                travelDestination = Vector3.zero;
                nextCollision = Vector3.zero;
            }
        }
        if (isMoving){return;}//stops the function after the return to prevent swiping while moving

        if (Input.GetMouseButton(0)) // when the user holds the screen or click on the mouse 
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // the position the mouse was clicked or the player is holding
            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;
                if (currentSwipe.sqrMagnitude < minSwipe)
                {
                    return;
                }
                currentSwipe.Normalize();
                if (currentSwipe.x > -0.5 && currentSwipe.x < 0.5)
                {
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }
                if (currentSwipe.y > -0.5 && currentSwipe.y < 0.5)
                {
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);  
                }
            }
            swipePosLastFrame = swipePosCurrentFrame;
        }
        if (Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }
    private void SetDestination(Vector3 direction)
    {
        travelDestination = direction;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollision = hit.point;
        }
        isMoving = true;
    }

}
