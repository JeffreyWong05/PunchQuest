using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fistDuration : MonoBehaviour
{
    public float duration=0.5f;
    public float timer = 0.05f;
    public Rigidbody2D fist;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0) {
            fist.velocity = new Vector2(0,0);
        }
        Destroy(gameObject, duration);
    }
}
