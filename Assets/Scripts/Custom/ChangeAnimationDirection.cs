using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Controller))]
public class ChangeAnimationDirection : MonoBehaviour
{
    private Controller controller;
    
    private SpriteRenderer spriteRenderer;

    private Camera mainCamera;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();
        mainCamera = FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        Vector2 point = controller.GetLookPosition();

        Vector2 mouse = mainCamera.ScreenToWorldPoint(point) - transform.position;

        spriteRenderer.flipX = mouse.x < 0;

        Vector2 move = controller.GetInput();
        
        animator.SetBool("isMoving", move.x != 0 || move.y != 0);
        animator.SetFloat("mouseX", mouse.x);
        animator.SetFloat("mouseY", mouse.y);
        animator.SetBool("isXForward", move.x != 0 && SameSign(move.x, mouse.x));
        animator.SetBool("isYForward", move.y != 0 && SameSign(move.y, mouse.y));
    }
    
    private bool SameSign(float num1, float num2)
    {
        return num1 >= 0 && num2 >= 0 || num1 < 0 && num2 < 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        print(other);
    }
}