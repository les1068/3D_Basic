using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretHunter : Turret
{
    public float sightRange = 10.0f; // 이 터렛의 시야 범위
    public float fireAngle = 10.0f;  // 터렛이 발사를 시작하는 각도
    public float turnSpeed = 360.0f; // 이 터렛의 회전 속도

    bool isFiring = false;       // 발사 중인지 아닌지 표시하는 변수 true면 발사중, false면 발사하지 않는 중

    Transform target = null;      // 이 터렛이 추적할 대상
    SphereCollider sightTrigger;  // 이 터렛이 가지고 있는 시야범위를 표시하는 트리거
    
    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();  // 컬라이더 찾기
    }
    protected override void Start()
    {
        base.Start();
        sightTrigger.radius = sightRange;  // 시야 범위로 컬라이더 크기 변경
    }
    private void Update()
    {
        LookTarget();                // 매 프레임마다 타켓을 바라보게 만들기
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;   // 트리거에 Player가 들어오면 타겟으로 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;              // 트리거에서 Player가 나가면 타겟은 null로 설정
        }
    }
    
    private void OnDrawGizmos()  // 씬창에서 보이는 테스트용 정보 그리는 함수
    {
        if (barrelBodyTransform == null)  // 에디터에서 플레이를 안했을 때 찾기 위해서 사용
        {
            barrelBodyTransform = transform.GetChild(2);  // 없으면 찾아 놓기
        }
        Gizmos.color = Color.yellow;                      // 기본 색 노란색으로 설정
        Vector3 from = barrelBodyTransform.position;      // 시작 위치 설정하기
        Vector3 to;                                       // 도착 위치

        Ray ray = new Ray(barrelBodyTransform.position, barrelBodyTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, sightRange))  // 선이 충돌하는지 체크
        {
            to = hitInfo.point;                        // 충돌했으면 도착지점은 충돌한 위치
            Gizmos.color = Color.red;                  // 충돌했으면 빨간색으로 보이기
            Gizmos.DrawSphere(to, 0.1f);               // 충돌한 지점을 빨간원으로 강조하기
        }
        else
        {
            to = barrelBodyTransform.position + barrelBodyTransform.forward * sightRange;  // 도착 위치 계산
          //to는 barrelBodyTransform.position위치에서 barrelBodyTransform.forward 방향으로 sightRange만큼 이동한 위치
        }

        Gizmos.DrawLine(from, to);    // 최종적으로 선을 그리기
    }
    private void LookTarget()  // 타겟이 있고 터렛이 볼수 있으면 해당 방향으로 터렛의 고개를 돌리는 함수
    {
        if (IsVisibleTarget())  // 타겟이 있고 볼수 있는지 확인
        {
            Vector3 dir = target.transform.position - barrelBodyTransform.position;
            dir.y = 0;                           // 높낮이는 영향을 안끼치게 0으로 설정

            float angle = Vector3.SignedAngle(barrelBodyTransform.forward, dir, barrelBodyTransform.up);
            // 터렛이 forward와 플레이어를 향하는 방향 백터의 사이각을 구함

            /*if (angle > 0)        // 위 삼항 연산자와 같은 내용의 코드
            {
                rotateDir = 10.0f;
            }
            else
            {
                rotateDir = -1.0f;
            }*/

            if (angle < 1 && angle > -1)  // 충분히 사이각이 작으면 dir로 직접 설정
            {
                barrelBodyTransform.rotation = Quaternion.LookRotation(dir);
            }
            else  // 어느 정도 사이각이 떨어져 있으면 등속도로 회전
            {
                // 정방향 회전인지 역방향 회전인지 결정
                float rotateDir = angle > 0 ? 1.0f : -1.0f;  // 삼항 연산자. 성능은 차이가없음 코드 줄수 줄이는 용도
                barrelBodyTransform.Rotate(0, Time.deltaTime * turnSpeed * rotateDir, 0);
            }
            /*currentAngle += (Time.deltaTime * turnSpeed * rotateDir);  // 정해진 방향으로 각도 변화
            Vector3 resultDir = Quaternion.Euler(0, currentAngle, 0) * initialForward; // 변화된 각도만큼 initialForward를 y축으로 회전

            barrelBodyTransform.rotation = Quaternion.LookRotation(resultDir);*/
            //barrelBodyTransform.forward = dir;   // 대상을 바라보기

            // 발사각 안이면 총알 발사, 밖이면 발사 정지 (-fireAngle ~ fireAngle 사이 인지 확인)
            if (angle < fireAngle && angle > -fireAngle)
            {
                //발사
                if (!isFiring)
                {
                    StartCoroutine(fireCoroutine);   // 연사 시작
                    isFiring = true;
                }
            }
            else
            {
                //발사 정지
                if (isFiring)
                {
                    StopCoroutine(fireCoroutine);    // 연사 중지
                    isFiring = false;
                }
            }
        }

    }

    bool IsVisibleTarget() // 타겟이 존재하고 볼 수 있는지 확인하는 함수
    { // 타겟이 있고 볼 수 있으면 true, 타겟이 없거나 다른 물체에 가려져 있으면 false
        bool result = false;
        if (target != null)   // 타겟이 있는지 확인
        {
            Vector3 toTargetDir = target.position - barrelBodyTransform.position;
            Ray ray = new Ray(barrelBodyTransform.position, toTargetDir);          // 터렛의 barrelBodyTransform에서 target방향으로 나가는 레이 생성
            if (Physics.Raycast(ray, out RaycastHit hitInfo, sightTrigger.radius))// 레이케스트 시도
            {
                // 레이케스트에 성공했으면 hitInfo에 각종 충돌 정보들이 들어옴
                //Debug.Log($"충돌:{ hitInfo.collider.gameObject.name}");
                if (hitInfo.transform == target) // 충돌 한 것의 transform과 target이 같으면 충돌한 것은 플레이어
                {
                    result = true;               // 그러면 true를 리턴
                }
            }
        }
        return result;
    }
    protected override void OnFire()
    {
        Factory.Inst.GetBullet(fireTransform);
    }

}
