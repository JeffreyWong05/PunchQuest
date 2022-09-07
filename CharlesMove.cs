using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharlesMove : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float dashSpeed = 10f;
    public float bulletSpeed = 3f;
    private Vector3 directionToPlayer;
    private Vector3 directionToTarget;
    private float wristX;
    private float wristY;
    private Vector3 localScale;

    private float timeBtwShots;
    public float startTimeBtwShots = 0.5f;

    public Rigidbody2D bullet;
    private int phase = 1;
    private float phaseDur;
    public float startPhaseDur = 3f;
    public float stabDur = 1f;

    private float dashCD;
    public float startDashCD = 5f;
    private float dashTime=0f;
    public float startDashTime=1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;

        timeBtwShots = startTimeBtwShots;
        phaseDur = 0;

        phase = 1;

        dashCD = startDashCD;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MoveEnemy();
    }

    private void MoveEnemy() 
    {
        //get direction
        directionToPlayer = (player.transform.position - transform.position).normalized;
        //follow player
        if(phase == 1) {
            if(Vector2.Distance(player.transform.position, transform.position) > 1.2f) 
            {
                //dash closer to the player, sometimes.
                dashCD -= Time.deltaTime;
                if(dashCD <= 0) {
                    if(Random.Range(0,2) >= 1) {
                        dashTime = startDashTime;
                    }
                    dashCD = startDashCD;
                }
                if(dashTime > 0) {
                    dashTime -= Time.deltaTime;
                    rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * dashSpeed;
                }
                else {
                    rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed;
                }
                
            }
            //retreat if too close
            else if(Vector2.Distance(player.transform.position, transform.position) < 1f) {
                rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * -moveSpeed;
            }
            //stop when close
            else {
                phase = 2;
                rb.velocity = new Vector2(0,0);
                directionToTarget = (player.transform.position - transform.position).normalized;
                phaseDur = startPhaseDur;
            }
        }
        if(phase == 2) {
            phaseDur -= Time.deltaTime;
            //shoot
            if(timeBtwShots <= 0) {
                //Get bullet position
                wristX = directionToTarget.x/3 + transform.position.x;
                wristY = directionToTarget.y/3 + transform.position.y;
                Vector2 wrist = new Vector2(wristX, wristY);

                //get angle
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

                var knife = Instantiate(bullet, wrist, Quaternion.Euler(0,0, angle));
                knife.AddForce (directionToTarget * bulletSpeed);

                timeBtwShots = startTimeBtwShots;
            }
            else {
                timeBtwShots -=Time.deltaTime;
            }
        }

        //switch to dash phase
        if (phaseDur <= 0 && phase == 2) {
            rb.velocity = new Vector2(0,0);
            phase = 3;
            phaseDur = stabDur;
        }

        if(phase == 3) {
            phaseDur -= Time.deltaTime;
            rb.velocity = Vector2.Perpendicular(new Vector2(directionToPlayer.x, directionToPlayer.y)) * dashSpeed;
            
        }
        //switch back to phase 1
        if (phaseDur <= 0 && phase == 3) {
            phase = 1;
            
        }
    }

    private void LateUpdate() 
    {
        //code for facing player
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }
}
