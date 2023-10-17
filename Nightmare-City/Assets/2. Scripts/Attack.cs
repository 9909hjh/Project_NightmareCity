using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public string gunName; //총이름
    public float range; // 사정 거리
    public float accuracy; // 정확도
    public float fireRate; //연사속도
    public float fireRate2; //따발총 연사속도
    public float reloadTime; // 재장전 속도

    public int damege; //총의 데미지

    public int reloadBulletCount; // 총알 재장전 개수
    public int reloadBulletCount2; // 우지 재장전 개수
    public int currentBulletCount; // 현재 탄알집에 남아있는 총알의 개수.
    //public int maxBulletCount; //최대 소유 가능한 총알 개수.
    public int carryBulletCount; // 현재 소유하고 있는 총알 개수.
    public int carryBulletCount2; // 따발총 현재 소유하고 있는 총알 개수.

    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; //정조준시의 반동세기

    public Vector3 fineSightOriginPos;

    public Animator anim;
    public AudioClip fire_Sound; // 총알 발사 사운드
    public AudioClip NONfire_Sound; // 총알 없을때 사운드

}
