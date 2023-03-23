using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;           // 이동 속도
    public float rotateSpeed = 180.0f;       // 회전 속도
    public float jumpForce = 6.0f;           // 점프력
    public float lifeTimeMax = 3.0f;         // 플레이어의 최대 수명
    float lifeTime = 3.0f;                   // 플레이어의 현재수명. 수명이 0보다 작거나 같아지면 사망처리

    public float LifeTime
    {
        get => lifeTime;
        private set
        {
            lifeTime = value;
            onLifeTimeChange?.Invoke(lifeTime/lifeTimeMax);

            if (lifeTime <= 0.0f)
            {
                Die();
            }
        }
    }

    public Action<float> onLifeTimeChange;
    public Action OnDie;       // onDie 델리게이트

    float moveDir = 0;        // 현재 이동 방향    // -1(뒤) ~ 1(앞)사이
    float rotateDir = 0;      // 현재 회전 방향    // -1(좌) ~ 1(우)사이

    bool isJumping = false;   // 현재 점프 여부. true면 점프 중, false면 점프 중 아님
    bool isAlive = true;
    Rigidbody rigid;
    Animator anim;
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        ItemUseAlarm alarm= GetComponentInChildren<ItemUseAlarm>();  // 아이템 사용 알람이 울리면 실행될 함수 등록
        alarm.onUseableItemUsed += UseObject;
    }
    

    private void OnEnable()
    {
        inputActions.Player.Enable();                        // Player 액션맵 활성화
        inputActions.Player.Move.performed += OnMoveInput;   // 액션들에게 함수 바인딩하기
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Use.performed += OnUseInput;
        inputActions.Player.Jump.performed += OnJumpInput;

        isAlive = true;
        //lifeTime = lifeTimeMax;  // 소문자 l = 변수 값을 변경하는것
        LifeTime = lifeTimeMax;    // 대문자 L = 프로퍼티를 실행시키는것
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJumpInput;   // 액션에 연결된 함수들 바인딩 해제
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();                       // Player 인풋 액션맵 비활성화
    }
    private void Update()
    {
        LifeTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();    // 이동 처리
        Rotate();  // 회전 처린
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // "Ground"와 충돌했을 때만
        {
            onGrounded();   // 착지 함수 실행
        }   
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();  // 현재 키보드 입력 상황 받기
        rotateDir = input.x;   // 좌우(좌: -1, 우 : +1)
        moveDir = input.y;   // 앞뒤(앞: +1, 뒤 : -1)


        // context.performed : 액션에 연결된 키 중 하나라도 입력 중이면 true, 아니면 false
        // context.canceled : 액션에 연결된 키가 모두 입력 중이지 않으면 ture, 아니면 false


        anim.SetBool("IsMove", !context.canceled);   // 애니메이션 파라메터 변경(Idel, Move중 선택)
    }
    private void OnUseInput(InputAction.CallbackContext context)
    {
        anim.SetTrigger("Use");
    }
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        Jump();  // 점프 처리 함수 실행
    }
    void Move() // 이동 처리 함수
    {
        // moveDir 방향으로 이동 시키기 (앞 아니면 뒤)
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDir * transform.forward);
    }
    void Rotate()  // 회전처리 함수
    {/*
        rigid.AddTorque();     // 회전력 추가
        rigid.MovePosition();  // 특정 회전으로 설정하기*/

        //Quaternion rotate2 = Quaternion.Euler(0,Time.fixedDeltaTime * rotateSpeed * rotateDir , 0);
        

        Quaternion rotate = Quaternion.AngleAxis( // 특정 축을 기준으로 회전하는 쿼터니언을 만드는 함수
            Time.fixedDeltaTime * rotateSpeed * rotateDir,transform.up); // 플레이어의 up방향으로 기준으로
        
        rigid.MoveRotation(rigid.rotation * rotate);  // 위에서 만든 회전을 적용
    }

    void Jump() // 점프 처리 함수
    {
        if (!isJumping)  // 점프 중이 아닐때만
        {
            rigid.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);  // 월드의 Up방향으로 힘을 즉시 가하기
            isJumping = true;  // 점프중이라고 표시
        }
    }
    private void onGrounded()   // 착지했을 때 처리 함수
    {
        isJumping = false;      // 점프가 끝났다고 표시
    }
    private void UseObject(IUseableObject obj)  // 아이템 사용한다는 알람이 오면 실행되는 함수. obj : 사용할 오브젝트
    {
        obj.Used();  // 사용
    }

    public void Die()  // 플레이어가 사망했을 때 실행이 되는 함수
    {
        if (isAlive)  // 살아 있는 플레이어만 죽을 수 있다.
        {
            //Debug.Log("Die");
            anim.SetTrigger("Die");

            // 입력 막기
            inputActions.Player.Disable();   // Player 액션맵 비활성화

            // 뒤로 넘어지게 만들기
            rigid.constraints = RigidbodyConstraints.None;   // pitch와 roll 회전이 막혀있던 것을 풀기
            Transform head = transform.GetChild(0);

            rigid.AddForceAtPosition(-transform.forward * 0.5f, head.position, ForceMode.Impulse); // 머리 위치에 플레이어의 뒷방향으로 0.5만큼의 힘을 가하기
            rigid.AddTorque(transform.up * 1.0f, ForceMode.Impulse); // 플레이어의 up벡터를 축으로 1만큼 회전력 더하기

            // 델리게이트로 알림 보내기
            OnDie?.Invoke();

            isAlive = false; // 사망 표시

        }
    }
    
}
