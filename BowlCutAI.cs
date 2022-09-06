using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BowlCutAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    //MAKE SURE DISTANCE IS 0.1f OR GLITCHINESS
    public float nextWaypointDistance = 0.1f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        
    }

    void UpdatePath()
    {
        if (seeker.IsDone()) 
        {
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }
            
    }

    void OnPathComplete(Path p) 
    {
        
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TowardsPlayer();
    }

    void TowardsPlayer() {
        if (path == null) {
            return;
        }
        
        if(currentWaypoint >= path.vectorPath.Count)
        {
            
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        var nextPoint = Vector2.MoveTowards(rb2d.position,
                                    path.vectorPath[currentWaypoint],
                                    speed * Time.deltaTime
                 );
        rb2d.MovePosition(nextPoint);

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
