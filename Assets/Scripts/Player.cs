using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 5.0f;
    float moveDir = 0;            // -1(��) ~ 1(��)����
    float rotateDir = 0;          // -1(��) ~ 1(��)����

    Rigidbody rigid;
    PlayerInputActions inputActions;
    Animator anim;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
        anim = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Move"))  
        {
            anim.SetBool("IsMove", true);
        }
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Use.performed += OnUseInput;
        inputActions.Player.Jump.performed += OnJumpInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        rotateDir = input.x;   // �¿�(��: -1, �� : +1)
        moveDir = input.y;   // �յ�(��: +1, �� : -1)
    }
    private void OnUseInput(InputAction.CallbackContext context)
    {
        
    }
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        
    }
    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDir * transform.forward);
    }
    void Rotate()
    {/*
        rigid.AddTorque();     // ȸ���� �߰�
        rigid.MovePosition();  // Ư�� ȸ������ �����ϱ�*/

        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDir,transform.up);
        //Quaternion rotate2 = Quaternion.Euler(0,Time.fixedDeltaTime * rotateSpeed * rotateDir , 0);
        rigid.MoveRotation(rigid.rotation * rotate);
    }


}
