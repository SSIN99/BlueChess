using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Text gradeText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider mpBar;

    public void InitStatus(int grade ,float maxHp, int maxMp, int curMp)
    {
        gradeText.text = $"{grade}";

        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;

        mpBar.maxValue = maxMp;
        mpBar.value = curMp;
    }
    public void UpdateStatus(float curHp, float curMp)
    {
        hpBar.value = curHp;
        mpBar.value = curMp;
    }
    private void Update()
    {
        pivot.transform.forward = Camera.main.transform.forward;
    }


}
