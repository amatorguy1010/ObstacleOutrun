using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] int speed;
    float speedMultiplier;

    [Range(1,10)]
    [SerializeField] float acceleration;

    bool btnPressed;

    //detect when player hit the wall and change direction
    bool isWallTouch;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;

    Vector2 relativeTransform;

    public void Start()
    {
        UpdateRelativeTransform();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        updateSpeedMultiplier();
        float targetSpeed = speed * speedMultiplier * relativeTransform.x;

        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);

        isWallTouch = Physics2D.OverlapBox(wallCheckPoint.position, new Vector2(0.06f, 0.3f), 0, wallLayer);

        if(isWallTouch)
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        UpdateRelativeTransform();
    }

    public void UpdateRelativeTransform()
    {
        relativeTransform = transform.InverseTransformVector(Vector2.one);
    }


    public void Move(InputAction.CallbackContext value)
    {
        if(value.started)
        { 
            btnPressed = true;
        }
        else if(value.canceled) 
        {
            btnPressed= false;
        }
        
    }

    void updateSpeedMultiplier()
    {
        if (btnPressed && speedMultiplier < 1) 
        {
            speedMultiplier += Time.deltaTime * acceleration ;
        }

        if (!btnPressed && speedMultiplier > 0)
        {
            speedMultiplier -= Time.deltaTime * acceleration;
            if (speedMultiplier < 0) speedMultiplier = 0;
        }
    }
}
