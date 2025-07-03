using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Body : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (moveDirection == Vector2.right) animator.Play("Walk_right");
        else if (moveDirection == Vector2.left) animator.Play("Walk_left");
        else if (moveDirection == Vector2.up) animator.Play("Walk_up");
        else if (moveDirection == Vector2.down) animator.Play("Walk_down");
    }
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
    }
}