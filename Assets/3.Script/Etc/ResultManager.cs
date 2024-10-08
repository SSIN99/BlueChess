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
        bestRecord.text = $"���� �ְ� ��� : {FirebaseManager.userData.record}";
        curRecord.text = $"�̹� �ְ� ���� : {roundManager.curRound - 1}";
        for (int i = 0; i < unitManager.fieldList.Count; i++)
        {
            usedUnitList[i].gameObject.SetActive(true);
            usedUnitList[i].InitInfo(unitManager.fieldList[i]);
        }
    }
}
