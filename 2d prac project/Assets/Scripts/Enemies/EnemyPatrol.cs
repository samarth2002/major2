using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform leftEdge;
    public Transform rightEdge;

    [Header("Enemy")]
    public Transform enemy;

    [Header("Movement Parameters")]
    public float speed;
    
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    public float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    public Animator anim;


    private void Awake(){
        initScale = enemy.localScale;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x)*_direction, initScale.y, initScale.z);

        enemy.position = new Vector3( enemy.position.x + Time.deltaTime * speed * _direction, enemy.position.y, enemy.position.z);
    }


    private void OnDisable(){
        anim.SetBool("moving", false);

    }

    private void Update()
    {
        if(movingLeft){
            if(enemy.position.x >= leftEdge.position.x){
                MoveInDirection(-1);
            }else{
                DirectionChange();
            }
        }
        else{
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }

        }
        // CheckEdges();
    }

    private void DirectionChange(){
        anim.SetBool("moving", false);

        idleTimer += Time.deltaTime;

        if(idleTimer >= idleDuration){
            movingLeft = !movingLeft;
        }
    }
    // private void CheckEdges()
    // {
    //     // Check if the enemy has reached the left edge
    //     if (direction == -1 && enemy.position.x <= leftEdge.position.x)
    //     {
    //         direction = 1; // Change direction to right
    //     }
    //     // Check if the enemy has reached the right edge
    //     else if (direction == 1 && enemy.position.x >= rightEdge.position.x)
    //     {
    //         direction = -1; // Change direction to left
    //     }
    // }
}
