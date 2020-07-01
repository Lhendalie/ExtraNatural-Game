using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    private Vector3 posA;
    private Vector3 posB;
    private Vector3 nexPos;

    public bool isAttacking = false;
    public bool facingRight = false;

    private bool isFollowing = false;

    public GameObject checkPoint;

    private GameControl gc;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform childTransform;

    [SerializeField]
    private Transform transformB;

    // Use this for initialization
    void Start()
    {
        posA = childTransform.localPosition;
        posB = transformB.localPosition;
        nexPos = posB;

        gc = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameControl>();
    }

    void Update()
    {
        if (isFollowing)
        {
            if (gc.playerDead)
            {
                isFollowing = false;
                Move();
            }
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            Move();
            isAttacking = false;
        }
    }

    //Moves the object as a patrolling enemy
    private void Move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nexPos, speed * Time.deltaTime);

        if (Vector3.Distance(childTransform.localPosition, nexPos) <= 0.1)
        {
            ChangeDestination();
            Flip();
        }
    }

    //Makes the object follow the player
    private void FollowPlayer()
    {
        nexPos = checkPoint.transform.position;
        nexPos.Set(nexPos.x + (facingRight ? -1 : 1), nexPos.y, nexPos.z);

        if (Math.Abs(transform.position.x - nexPos.x) < 0.2)
        {
            return;
        }        

        transform.position = Vector3.MoveTowards(transform.position, nexPos, speed * Time.deltaTime);


        if (facingRight == false && transform.position.x < nexPos.x)
        {
            Flip();
        }
        else if (facingRight == true && transform.position.x > nexPos.x)
        {
            Flip();
        }
    }

    private void ChangeDestination()
    {
        nexPos = nexPos != posA ? posA : posB;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Here is what happens when the object collides with the player
        if(col.CompareTag("Player"))
        {
            FollowPlayer();
            isFollowing = true;
        }
    }

    public void AlertObserver(string message)
    {
        //Checks if the ghost is attacking the player and if not stops playing attacking animation
        if(message.Equals("AnimationEnded"))
        {
            isAttacking = true;
        }
    }
}
