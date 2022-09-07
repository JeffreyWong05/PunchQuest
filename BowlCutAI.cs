using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BowlCutAI : MonoBehaviour
{
    //all astars stuff
    public Transform target;

    public float speed = 200f;
    //MAKE SURE DISTANCE IS 0.1f OR GLITCHINESS
    public float nextWaypointDistance = 0.1f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2d;

    //phases
    private int phase=1;
    private int phaseDur;

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
        //Face the player
        facePlayer();

        if (phase == 1) {
            //chase player
            float dist = Vector2.Distance(target.position, transform.position);
            if(dist > 5f) 
            {
                TowardsPlayer();
            }
            else if(dist < 4f)
            { //Run away if player gets close
                Vector2 dirAway = (target.position - transform.position).normalized;
                //float angle = Math.Atan2(dirAway.y, dirAway.x)*Mathf.Rad2Deg;
                
                var away = Vector2.MoveTowards(rb2d.position, target.position, -speed*Time.deltaTime);
                rb2d.MovePosition(away);
            }
            else {
                rb2d.velocity = Vector2.zero;
            }
        }
    }
    //Will check player position and face them
    void facePlayer() {
        print(rb2d.velocity);
        if(target.position.x - transform.position.x >= 0.01f) 
        {
            transform.localScale = new Vector3(1f, 1f, 1f);

        } else if (target.position.x - transform.position.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
        }
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

        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;

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
