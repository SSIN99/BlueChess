using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class DealChartUIHandler : MonoBehaviour
{
    [SerializeField] private Image btnImage;
    [SerializeField] private Sprite[] btnSprite;
    [SerializeField] private RectTransform rect;
    [SerializeField] private RoundManager round;
    [SerializeField] private Player player;
    [SerializeField] private DealChart[] chart;
    private bool isOpen = false;
    private int count;
    private void OnEnable()
    {
        round.OnStepChange += InitChart;
    }
    private void InitChart()
    {
        for(int i = 0; i < player.fieldList.Count; i++)
        {
            player.fieldList[i].OnDealAmountChanged -= UpdateChart;
        }
        if (!round.IsBattleStep) return;
        for(int i = 0; i < chart.Length; i++)
        {
            if (i < player.fieldList.Count)
            {
                chart[i].gameObject.SetActive(true);
                chart[i].SetChart(player.fieldList[i]);
                player.fieldList[i].OnDealAmountChanged += UpdateChart;
            }
            else
            {
                chart[i].gameObject.SetActive(false);
            }
        }
        count = player.fieldList.Count;
    }
    private void UpdateChart()
    {
        float max = player.fieldList.Max(d => d.AllDealAmount);


        for(int i =0; i<count; i++)
        {
            chart[i].UpdateChart(max);
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
