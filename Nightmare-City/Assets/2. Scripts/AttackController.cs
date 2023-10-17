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


    // ������
    private float clickTime; // Ŭ������ �ð�.
    public float minClicTime = 1; // �ּ� Ŭ���ð�
    private bool isClick; // Ŭ�� ������ �Ǵ�
    // �������
    
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

    private void GunFireRateCalc() // ����ӵ� �� 0�̿��� �߻縦 �� �� ����
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire() // ���⿡ ������ ���콺 ���� ������ �ɷ���.
    {
        if (isfineSightMode) // �������϶��� ���ݰ���.
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
            currentFireRate = currentGun.fireRate2; //���� ����ӵ� ����(�ٽ� �������� 0.1�� ���ư�)
            
        }
        else
        {
            currentFireRate = currentGun.fireRate; //����ӵ� ����(�ٽ� �������� 0.5�� ���ư�)
        }

        currentGun.anim.SetTrigger("Fire"); // �ݵ� �ִϸ��̼�. ���⿡ �ݵ� �ִϸ��̼� �߰�.
        PlaySE(currentGun.fire_Sound); // �Ѿ˹߻� ����
        Hit();
    }

    
    private void MachineGuncarry() // ���� �κ�.
    {
        PlayerCtrl player = GetComponent<PlayerCtrl>();
        if (player.hasGun[1])
        {
            currentGun.carryBulletCount = currentGun.carryBulletCount2; // ���� źâ 9999 ����
            currentGun.reloadBulletCount = currentGun.reloadBulletCount2; // ���� �ִ� ���� ��.
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
            //Debug.Log("����ź�� ����");
        }
    }

    // ������ �õ�
    private void TryFineSight() // ������ ���콺�� �������� ������ Ȱ��ȭ
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

    private void ClickTimeer() // ���� Ȱ��ȭ�� ���� �Լ�.
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

    // ������ ���
    public void CancelFineSight()
    {
        if (isfineSightMode)
            FineSight();
    }

    // ������
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

    // ���� �߰�
    // źâ�� ������ �Ѿ� �߰� �� źâ ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Bullet:
                    currentGun.carryBulletCount += item.value; // �Ѿ� �߰� 
                    break;
            }

            Destroy(other.gameObject); // ����
        }
    }
}