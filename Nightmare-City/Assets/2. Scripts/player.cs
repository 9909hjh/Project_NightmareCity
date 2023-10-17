using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 클래스에 System.Serializable이라는 어트리뷰트(Attribute)를 명시해야 Inspector뷰에 노출됨
[System.Serializable]
public class Anim
{
    [Tooltip("정지 애니메이션")]
    public AnimationClip idle;
    [Tooltip("전진 애니메이션")]
    public AnimationClip runForward;
    [Tooltip("후진 애니메이션")]
    public AnimationClip runBackward;
    [Tooltip("오른쪽 이동 애니메이션")]
    public AnimationClip runRight;
    [Tooltip("왼쪽 이동 애니메이션")]
    public AnimationClip runLeft;
}

public class player : MonoBehaviour
{
    // 멤버 변수 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private float h = 0.0f;
    private float v = 0.0f;

    private Transform tr; // 접근해야 하는 컴포넌트는 반드시 변수에 할당한 후에 사용
    [Tooltip("이동 속도 변수")]
    public float moveSpeed = 10.0f;
    [Tooltip("회전 속도 변수")]
    public float rotSpeed = 100.0f;

    // 첫 번째 레포트1ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private bool islcPressed = false;
    private bool islsPressed = false;
    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    // 멤버 함수 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    void Start()
    {
        // 스크립트 처음에 Transform 컴포넌트 할당
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        Move();
    }

    void Move() // Player 움직임과 관련된 함수
    {
        h = Input.GetAxis("Horizontal"); // A, D, Left, Right 키를 눌렀을 때
        v = Input.GetAxis("Vertical"); // W, S, Up, Down 키를 눌렀을 때

        // 전 후 좌 우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Translate(이동 방향 * 속도 * 변위값 *  Time.deltaTime, 기준 좌표)
        //tr.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime, Space.Self);
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed, Space.Self);

        //// Vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전
        //tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

        // 첫 번째 레포트2ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
        // 왼쪽 Ctrl 키를 누르고 있을 경우
        if (!islsPressed && Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveSpeed *= 1.5f;
            islcPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveSpeed = 10f;
            islcPressed = false;
        }

        // 왼쪽 Shift 키를 누르고 있을 경우
        if (!islcPressed && Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            moveSpeed *= 0.5f;
            islsPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
            islsPressed = false;
        }
        // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    }

    static public int key = 0;
    public int stageNum = 0;
    public Text currentKey;
    void OnTriggerEnter(Collider coll)
    {
        //// 충돌한 Collider가 몬스터의 PUNCH이면 Player의 HP 차감
        //if(coll.gameObject.tag == "PUNCH")
        //{
        //    hp -= 10;

        //    // Image UI 항목의 fillAmount 속성을 조절해 생명 게이지 값 조절
        //    imgHpbar.fillAmount = (float)hp / (float)initHp;

        //    Debug.Log("Player HP = " + hp.ToString());
        //    // Player의 생명이 0 이하이면 사망 처리
        //    if(hp <= 0)
        //    {
        //        PlayerDie();
        //    }
        //}
        if(coll.gameObject.tag == "Key")
        {
            key++;
            currentKey.text = key.ToString();
            Destroy(coll.gameObject);
            if (key == 2 || key == 5)
            {
                createPortal();
            }
        }
        else if(coll.gameObject.tag == "Portal")
        {
            stageNum++;
            SceneManager.LoadScene("test " + stageNum);
        }
    }

    public GameObject portal;
    void createPortal()
    {
        portal.SetActive(true);
    }
}