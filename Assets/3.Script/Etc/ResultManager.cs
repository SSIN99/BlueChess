using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] RoundManager roundManager;
    [SerializeField] UnitManager unitManager;
    [SerializeField] GameObject resultPanel;
    [SerializeField] UsedUnitUI[] usedUnitList;
    [SerializeField] Text bestRecord;
    [SerializeField] Text curRecord;

    public void ShowResult()
    {
        resultPanel.SetActive(true);
        bestRecord.text = $"나의 최고 기록 : {FirebaseManager.userData.record}";
        curRecord.text = $"이번 최고 라운드 : {roundManager.curRound - 1}";
        for (int i = 0; i < unitManager.fieldList.Count; i++)
        {
            usedUnitList[i].gameObject.SetActive(true);
            usedUnitList[i].InitInfo(unitManager.fieldList[i]);
        }
    }
}
