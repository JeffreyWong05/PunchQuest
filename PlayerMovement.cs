using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _Speed=3;
    PlayerInput _Input;
    Vector2 _Movement;
    Vector2 LastMove;
    Rigidbody2D _Rigidbody;
    private float wristX;
    private float wristY;
    private float punchTime = 0;

    public Animator animator; 
    public Rigidbody2D fist;
    public float punchSpeed;
    public float punchCD;

    private void Awake() {
        _Input = new PlayerInput();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        _Input.Enable();

        _Input.Gameplay.Movement.performed += OnMovement;
        _Input.Gameplay.Movement.canceled += OnMovement;
    }

    private void OnDisable() {
        _Input.Disable();
    }

    private void OnMovement(InputAction.CallbackContext context) {
        _Movement = context.ReadValue<Vector2> ();
    }

    private void FixedUpdate() {
        _Rigidbody.velocity = _Movement * _Speed;
        if(_Movement.x != 0 || _Movement.y != 0) {
            LastMove = _Movement;
        }
        
        animator.SetFloat("IsRight", _Movement.x);

        if (punchTime > 0) {
            punchTime -= Time.deltaTime;
            
        }
        else {
            OnEnable();
        }
        //button "," is pressed
        if (_Input.Gameplay.Attack.ReadValue<float>()==1 && punchTime <= 0) {
            
            punchTime = punchCD; 
            OnDisable();

            //Update position of fist
            wristX = _Rigidbody.position.x + LastMove.x/2;
            wristY = _Rigidbody.position.y + LastMove.y/2;
            Vector2 wrist = new Vector2(wristX, wristY);
            Vector2 direction = new Vector2(LastMove.x, LastMove.y);
            
            //update rotation of fist
            //right
            
            if(LastMove.x > 0) {
                var punch = Instantiate (fist, wrist, Quaternion.Euler(new Vector3(0, 0, 0)));
                
                punch.AddForce (direction * punchSpeed);
            }
            else if(LastMove.x < 0) {
                var punch =Instantiate (fist, wrist, Quaternion.Euler(new Vector3(0, 0, 180)));
                punch.AddForce (direction * punchSpeed);
            }
            else if(LastMove.y > 0) {
                var punch =Instantiate (fist, wrist, Quaternion.Euler(new Vector3(0, 0, 90)));
                punch.AddForce (direction * punchSpeed);
            }
            else if(LastMove.y < 0) {
                var punch =Instantiate (fist, wrist, Quaternion.Euler(new Vector3(0, 0, -90)));
                punch.AddForce (direction * punchSpeed);
            }

            //var firedFist;
        }

    }
}
