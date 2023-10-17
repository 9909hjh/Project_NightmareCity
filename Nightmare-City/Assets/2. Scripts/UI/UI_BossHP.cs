using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHP : MonoBehaviour
{
    public Image hpbar;

    public void SetHP(float hp)
    {
        hpbar.fillAmount = (float)hp / 3000f;
    }
}
