using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    private Vector3 previousTargetPosition;

    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    public Transform enemyGFX;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        previousTargetPosition = target.position;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (Vector3.Distance(target.position, previousTargetPosition) >= 0.5f || seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
            previousTargetPosition = target.position;
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            rb.velocity = Vector2.zero;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localRotation = Quaternion.Euler(0, 0, 0); // No rotation
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localRotation = Quaternion.Euler(0, 180, 0); // Flip horizontally
        }
    }
}
