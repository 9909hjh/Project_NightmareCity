using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    Camera _camera;
    CharacterController _controller;
    //public float runspeed = 8f;
    //public float finalspeed;
    //public bool run;

    public float smoothness = 10f;

    //------------------------------------------------
    //private Rigidbody playerRigidbody;
    private float stop = 0f;
    public float speed = 10f;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;

    //상태.
    private bool isWalk = true;
    private bool isRun = false;
    //private bool isGround = true;

    private Vector3 lastPos;
    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    Animator animator;

    [SerializeField]
    private AttackController theAttackController;
    private Crosshair thecrosshair; // 조준점.

    //------------------------------------------------찬희 12.18일
    GameObject nearObject; // 아이템 저장
    GameObject equipGun; // 지금 장착된 무기 저장 변수
    public GameObject[] Gun; // 먹은 아이템을 저장
    public bool[] hasGun; // 먹은 아이템 참/불

    //-----------------------------------------------주형 플레이어 채력
    public int hp = 150;

    //-------------------------
    private AudioSource audioSource2;
    public AudioClip warking;
    public AudioClip ruuning;
    public AudioClip emp;
    public AudioClip AudisDIE;

    private bool test = true;
    private bool test2 = true;
    // ======================================================준하
    public UI_HpBar hpBar;
    Rigidbody rigid;
    public bool isDie = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //playerRigidbody = GetComponent<Rigidbody>();
        _controller = this.GetComponent<CharacterController>();

        applySpeed = speed;

        _camera = Camera.main;

        theAttackController = FindObjectOfType<AttackController>();
        thecrosshair = FindObjectOfType<Crosshair>(); // 조준점.

        rigid = GetComponent<Rigidbody>();

        audioSource2 = GetComponent<AudioSource>();
    }

    public Text currentKey;
    public Text totalKey;
    void Update()
    {
        TryRun();
        PlayerMove(); 
        Swap(); // 무기 교체되는 식
        //MoveCheck();

        if(hp <= 0)
        {
            applySpeed = stop;
        }

        if(stageNum != 3)
        {
            calc();
        }
    }
    private void LateUpdate()
    {
        // 카메라 부분.
        Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
    }

    //달리기를 하는지, 취소하는지 결정
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isDie)
        {
            Running();
            if (test2)
            {
                PlaySE2(ruuning);
                test2 = false;
            }
            animator.SetBool("Run", isRun);

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isDie)
        {
            RunningCancel();
            animator.SetBool("Run", isRun);
            test2 = true;
            PlaySE2(emp);
        }
    }

    //달리기
    private void Running()
    {
        theAttackController.CancelFineSight(); // 정조준시 뛸때 정조준이 해제되게만듦

        isRun = true;
        thecrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        thecrosshair.RunningAnimation(isRun);
        applySpeed = speed;
    }

    private void PlayerMove()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * _moveDirZ + right * _moveDirX;

        _controller.Move(moveDirection.normalized * applySpeed * Time.deltaTime);

        if (!isRun)
        {
            if (theAttackController.isfineSightMode)
            {
                applySpeed = stop;
            }
            else
            {
                applySpeed = speed;
                if (_moveDirZ > 0.1f)
                {
                    animator.SetBool("Up", true);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    isWalk = true;
                    if (test) 
                    {
                        PlaySE2(warking);
                        test = false;
                    }
                }
                else if (_moveDirZ <= -0.1f)
                {
                    
                    animator.SetBool("Up", true);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    isWalk = true;
                    if (test)
                    {
                        PlaySE2(warking);
                        test = false;
                    }
                }
                else if (_moveDirX >= 0.1f)
                {
                    
                    animator.SetBool("Up", false);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", true);
                    isWalk = true;
                    if (test)
                    {
                        PlaySE2(warking);
                        test = false;
                    }
                }
                else if (_moveDirX <= -0.1f)
                {
                    
                    animator.SetBool("Up", false);
                    animator.SetBool("Left", true);
                    animator.SetBool("Right", false);
                    isWalk = true;
                    if (test)
                    {
                        PlaySE2(warking);
                        test = false;
                    }
                }
                else
                {
                    animator.SetBool("Up", false);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    isWalk = false;
                    test = true;
                    PlaySE2(emp);
                }
                thecrosshair.WalkingAnimation(isWalk);
            }
        }
    }
    

    // 손에 든 총 바꿔주는 식 (Update()에서 사용)
    void Swap()
    {
        if(hasGun[1])
        {
            equipGun = Gun[0]; // 권총을 꺼줌.
            equipGun.SetActive(false); // 원래 들고있던 총 끄기
            equipGun = Gun[1]; // 1번 배열 값을 받고
            Gun[1].SetActive(true); // 먹은 총으로 교체
        }
    }

    // Item 스크립트에 값 받아오고 배열에 저장하고 삭제
    public GameObject weaponImg1;
    public GameObject weaponImg2;
    static public int key = 0; // 열쇠 개수
    static public int stageNum = 1; // stage 번호
    void Interation()
    {
        if (nearObject != null)
        {
            if(nearObject.tag == "MachineGun")
            {
                Item item = nearObject.GetComponent<Item>();
                int GunIndex = item.value; // 몇번째 배열 아이템인지 값 받고
                hasGun[GunIndex] = true; // 그 몇번째 아이템 먹었다고 확인

                weaponImg1.SetActive(false);
                weaponImg2.SetActive(true);

                Destroy(nearObject);// 삭제
            }
            else if (nearObject.tag == "Key")
            {
                key++;
                currentKey.text = key.ToString();
                Destroy(nearObject);
                if(stageNum == 1)
                {
                    if(key == 2)
                    {
                        createPortal();
                    }
                }
                else if(stageNum == 2)
                {
                    if(key == 3)
                    {
                        createPortal();
                    }
                }
            }
            else if (nearObject.tag == "Portal")
            {
                stageNum++;
                SceneManager.LoadScene(stageNum + "stage");
            }
        }
    }

    bool isNext = true;
    void calc()
    {
        if (stageNum == 1)
        {
            currentKey.text = key.ToString();
            totalKey.text = "2";
        }
        else if (stageNum == 2)
        {
            if(isNext == true)
            {
                key = 0;
                isNext = false;
            }
            currentKey.text = key.ToString();
            totalKey.text = "3";
        }
    }

    public GameObject door;
    public GameObject DoorToPortal;
    void createPortal()
    {
        DoorToPortal.SetActive(false);
        door.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MachineGun")
        {
            nearObject = other.gameObject;
            Interation();
        }
        else if(other.tag == "Key")
        {
            nearObject = other.gameObject;
            Interation();
        }
        else if (other.tag == "Portal")
        {
            nearObject = other.gameObject;
            Interation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MachineGun")
        {
            nearObject = null;
        }
    }

    private void PlaySE2(AudioClip _clip)
    {
        audioSource2.clip = _clip;
        audioSource2.Play();
        if(isRun)
        {
            audioSource2.loop = true;
        }
        else
            audioSource2.loop = false;
    }

    private void Playdie(AudioClip _clip)
    {
        audioSource2.clip = _clip;
        audioSource2.PlayOneShot(_clip);
    }

    public UI_Died uiDied;
    public GameObject dieCamera;
    public void GetDamage(int damage) // 플레이어 죽음.
    {
        hp -= damage;
        hpBar.SetHP(hp);
        if (hp <= 0)
        {
            animator.SetTrigger("Die");
            Playdie(AudisDIE);
            dieCamera.GetComponent<CameraMove>().enabled = false; // 죽었을 때 카메라 움직이지 않게 하려고 이름 저따구임
            //rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            isDie = true;
            StartCoroutine(diePlayer());
            uiDied.playerDie();
        }
    }

    public IEnumerator diePlayer()
    {
        yield return new WaitForSeconds(1f);
        animator.enabled = false;
    }
}