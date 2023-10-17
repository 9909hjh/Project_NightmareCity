using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    private Attack currentGun;

    private float currentFireRate;

    private bool isReload = false;
    public bool isfineSightMode = false;

    private AudioSource audioSource;

    private RaycastHit hitInfo;
    private MonsterCtrl themonsterCtrl;

    [SerializeField]
    private Camera theCam;
    private Crosshair thecrosshair;

    public int damage = 30;


    // 실험적
    private float clickTime; // 클릭중인 시간.
    public float minClicTime = 1; // 최소 클릭시간
    private bool isClick; // 클릭 중인지 판단
    // 여기까지
    
    void Start()
    {
        themonsterCtrl = GetComponent<MonsterCtrl>();
        audioSource = GetComponent<AudioSource>();
        thecrosshair = FindObjectOfType<Crosshair>();
    }

    void Update()
    {
        ClickTimeer();
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
        MachineGuncarry();
    }

    private void GunFireRateCalc() // 연사속도 가 0이여야 발사를 할 수 있음
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire() // 여기에 오른쪽 마우스 값을 넣으면 될려나.
    {
        if (isfineSightMode) // 정조준일때만 공격가능.
        {
            if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroution());
            }
        }
    }

    private void Shoot()
    {
        //thecrosshair.FireAnimation();
        currentGun.currentBulletCount--;

        PlayerCtrl player = GetComponent<PlayerCtrl>();
        if(player.hasGun[1])
        {
            currentFireRate = currentGun.fireRate2; //우지 연사속도 재계산(다시 설정해준 0.1로 돌아감)
            
        }
        else
        {
            currentFireRate = currentGun.fireRate; //연사속도 재계산(다시 설정해준 0.5로 돌아감)
        }

        currentGun.anim.SetTrigger("Fire"); // 반동 애니메이션. 여기에 반동 애니메이션 추가.
        PlaySE(currentGun.fire_Sound); // 총알발사 사운드
        Hit();
    }

    
    private void MachineGuncarry() // 우지 부분.
    {
        PlayerCtrl player = GetComponent<PlayerCtrl>();
        if (player.hasGun[1])
        {
            currentGun.carryBulletCount = currentGun.carryBulletCount2; // 우지 탄창 9999 설정
            currentGun.reloadBulletCount = currentGun.reloadBulletCount2; // 우지 최대 장전 수.
        }
    }



    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward +
            new Vector3(Random.Range(-thecrosshair.GetAccuracy() - currentGun.accuracy, thecrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-thecrosshair.GetAccuracy() - currentGun.accuracy, thecrosshair.GetAccuracy() + currentGun.accuracy),
                        0)
            , out hitInfo, currentGun.range))
        {
            
            if (hitInfo.transform.tag == "Enemy")
            {
                hitInfo.transform.GetComponent<MonsterCtrl>().TakeDamage(damage);
                hitInfo.transform.GetComponent<MonsterCtrl>().CreateBloodEffect();
                Debug.Log(hitInfo.transform.name);
            }

        }
    }
    //  && currentGun.currentBulletCount < currentGun.reloadBulletCount
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroution());
        }
    }

    IEnumerator ReloadCoroution()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");
                
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            PlaySE(currentGun.NONfire_Sound);
            //Debug.Log("남은탄이 없음");
        }
    }

    // 정조준 시도
    private void TryFineSight() // 오른쪽 마우스를 누를때만 조준이 활성화
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isClick = true;
            if (clickTime >= minClicTime)
            {
                if ( !isReload)
                {
                    FineSight();
                }
            }
        }
        else if(Input.GetButtonUp("Fire2"))
        {
            CancelFineSight();
        }
        //if (Input.GetButtonDown("Fire2") && !isReload)
        //{
        //    FineSight();
        //}
        //Input.GetButton("Fire2") &&
    }

    private void ClickTimeer() // 조준 활성화를 위한 함수.
    {
        if (isClick)
        {
            clickTime += Time.deltaTime;
        }
        else
        {
            clickTime = 0;
        }
    }

    // 정조준 취소
    public void CancelFineSight()
    {
        if (isfineSightMode)
            FineSight();
    }

    // 정조준
    private void FineSight()
    {
        isfineSightMode = !isfineSightMode;
        currentGun.anim.SetBool("FineSightMode", isfineSightMode);
    }


    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Attack GetAttack()
    {
        return currentGun;
    }

    // 찬희 추가
    // 탄창을 먹으면 총알 추가 및 탄창 삭제
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Bullet:
                    currentGun.carryBulletCount += item.value; // 총알 추가 
                    break;
            }

            Destroy(other.gameObject); // 삭제
        }
    }
}
