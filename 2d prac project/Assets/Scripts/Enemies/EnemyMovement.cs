using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 0.2f;
    public Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isWalking", true);
    }

    void Update()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1f) 
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        // Vector2 point = currentPoint.position - transform.position;
        // if(currentPoint == pointB.transform){
        //     rb.velocity = new Vector2(speed, 0);
        // }else{
        //     rb.velocity = new Vector2(-speed, 0);
        // }

        // if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform){
        //     Flip();
        //     currentPoint = pointA.transform;
        // }
        // if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        // {
        //     Flip();
        //     currentPoint = pointB.transform;
        // }
    }
    private void Flip(){
        Vector3 localScale  = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    // private void OnDrawGizmos() {
    //     Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
    //     Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);

    // }
}
