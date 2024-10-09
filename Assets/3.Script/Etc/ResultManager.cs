using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public void BackToLobby()
    {
        if (FirebaseManager.userData.record < roundManager.curRound - 1)
        {
            FirebaseManager.UpdateUserRecord(FirebaseManager.user.UserId, roundManager.curRound - 1);
        }
        SceneManager.LoadScene("Lobby");
    }
}
