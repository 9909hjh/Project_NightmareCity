using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    //필요한 컴포넌트
    [SerializeField]
    private AttackController theAttackController;
    private Attack currentGun;

    //총알 개수 반영
    [SerializeField]
    private Text[] text_Bullet;
    
    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = theAttackController.GetAttack();
        text_Bullet[0].text = currentGun.currentBulletCount.ToString();
        text_Bullet[1].text = currentGun.carryBulletCount.ToString();
        //text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
    }
}