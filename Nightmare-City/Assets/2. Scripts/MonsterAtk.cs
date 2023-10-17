using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAtk : MonoBehaviour
{
    public int Damage;

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("µé¾î¿È");
            PlayerCtrl player = other.GetComponent<PlayerCtrl>();

            if(player != null)
            {
                player.GetDamage(Damage);
            }
        }
    }
}
