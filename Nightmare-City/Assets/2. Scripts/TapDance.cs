using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapDance : MonoBehaviour
{
    private Animator ani;

    public bool check = true;
    private void Start()
    {
        ani = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        ani.SetBool("IsTrace", true);
        Invoke("MonsterTab", 1f);
        
    }

    private void MonsterTab()
    {
        ani.SetBool("IsTrace", false);
    }

    
}
