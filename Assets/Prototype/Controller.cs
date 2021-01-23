using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [SerializeField, Range(1, 40)]
    protected float forwardSpeed;
    [SerializeField, Range(1, 20)]
    protected float sideSpeed;
    [SerializeField, Range(1, 20)]
    protected float backSpeed;
    [SerializeField, Range(1, 10)]
    protected float acceleration = 1;
    [SerializeField, Range(1, 10)]
    protected float deacceleration = 1;
    [SerializeField, Range(1, 10)]
    protected float division = 1;

    private Vector3 prevDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        var direction = new Vector2(horizontal, vertical);
        Move(direction);
        Rotate(direction);
    }


    public void Move(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y) * forwardSpeed;
        prevDirection = Vector3.Lerp(prevDirection, moveDirection, acceleration * Time.deltaTime);
        controller.SimpleMove(prevDirection);
        animator.SetFloat("Speed", prevDirection.magnitude / forwardSpeed);
    }

    public void Rotate(Vector2 look)
    {
        transform.LookAt(transform.position + new Vector3(look.x, 0, look.y));
    }
}
