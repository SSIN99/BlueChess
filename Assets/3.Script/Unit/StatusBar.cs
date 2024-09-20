using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider mpBar;

    public void InitStatusBar(float maxHp, int maxMp, int curMp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;

        mpBar.maxValue = maxMp;
        mpBar.value = curMp;
    }
    public void UpdateStatusBar(float curHp)
    {
        hpBar.value = curHp;
    }
    private void Update()
    {
        pivot.transform.forward = Camera.main.transform.forward;
    }


}
