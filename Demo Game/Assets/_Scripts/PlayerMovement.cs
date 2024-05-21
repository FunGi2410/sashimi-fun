using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 5f;
    bool isMoving;
    Animator animator;

    public VariableJoystick joystick;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
    }
    void Update()
    {
        this.Move();
    }

    void Move()
    {
        /*// Get input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");*/
        // Joystick input
        
        Vector3 direction = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        direction = direction.normalized;
        direction = Quaternion.Euler(0, 45f, 0) * direction;
        transform.Translate(direction * this.moveSpeed * Time.deltaTime, Space.World);

        // Rotate player
        if (direction != Vector3.zero)
        {
            this.animator.SetFloat("move", 1);
            this.isMoving = true;
            Quaternion rotate = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, this.rotateSpeed * Time.deltaTime);
        }
        else
        {
            this.animator.SetFloat("move", 0);
            this.isMoving = false;
        }

        // Animation
        this.animator.SetBool("isMoving", this.isMoving);
    }
}
