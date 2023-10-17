using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Tooltip("추적할 타깃 게임오브젝트의 Transform 변수")]
    [SerializeField] private Transform targetTr;
    [Tooltip("카메라와의 일정 거리")]
    [SerializeField] private float dist = 5.0f;
    [Tooltip("카메라의 높이 설정")]
    [SerializeField] private float height = 3.0f;
    [Tooltip("부드러운 추적을 위한 변수")]
    [SerializeField] private float dampTrace = 20.0f;

    private Transform tr; // 카메라 자신의 Transform 변수

    void Start()
    {
        // 카메라 자신의 Transform 컴포넌트를 tr에 할당
        tr = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        // 카메라의 위치를 추적 대상의 dist 변수 만큼 뒤쪽으로 배치하고 height 변수 만큼 위로 올림
        tr.position = Vector3.Lerp(tr.position, // 시작 위치
                                    targetTr.position - (targetTr.forward * dist) + (Vector3.up * height), // 종료 위치
                                    Time.deltaTime * dampTrace); // 보간 시간
        // 카메라가 타킷 게임오브젝트를 바라보게 설정
        tr.LookAt(targetTr.position);
    }
}
