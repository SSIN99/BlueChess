using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider hpBar;

    public void InitHpBar(int maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;
    }

   
}
