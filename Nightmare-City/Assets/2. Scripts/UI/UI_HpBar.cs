using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : MonoBehaviour
{
    public Image hpbar;

    public void SetHP(float hp)
    {
        hpbar.fillAmount = (float)hp / 100f;
    }
}
