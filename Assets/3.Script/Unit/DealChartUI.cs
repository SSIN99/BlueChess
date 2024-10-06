using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class DealChartUI : MonoBehaviour
{
    [SerializeField] private Image btnImage;
    [SerializeField] private Sprite[] btnSprite;
    [SerializeField] private RectTransform rect;
    [SerializeField] private RoundManager round;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private DealBarUI[] chart;
    private bool isOpen = false;
    private int numOfField;
    private void OnEnable()
    {
        round.OnStepChange += InitInfo;
    }
    private void InitInfo()
    {
        for(int i = 0; i < unitManager.fieldList.Count; i++)
        {
            unitManager.fieldList[i].OnDealAmountChanged -= UpdateChart;
        }
        if (!round.IsBattleStep) return;
        for(int i = 0; i < chart.Length; i++)
        {
            if (i < unitManager.fieldList.Count)
            {
                chart[i].gameObject.SetActive(true);
                chart[i].InitInfo(unitManager.fieldList[i]);
                unitManager.fieldList[i].OnDealAmountChanged += UpdateChart;
            }
            else
            {
                chart[i].gameObject.SetActive(false);
            }
        }
        numOfField = unitManager.fieldList.Count;
    }
    private void UpdateChart()
    {
        float max = unitManager.fieldList.Max(d => d.dealAmount);
        for(int i =0; i < numOfField; i++)
        {
            chart[i].UpdateMaxDeal(max);
        }
    }
    public void OpenChart()
    {
        if (isOpen)
        {
            isOpen = false;
            btnImage.sprite = btnSprite[0];
            rect.DOAnchorPosX(150f, 0.1f);
        }
        else
        {
            isOpen = true;
            btnImage.sprite = btnSprite[1];
            rect.DOAnchorPosX(-150f, 0.1f);
        }
    }
}
