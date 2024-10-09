using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingBarUI : MonoBehaviour
{
    [SerializeField] private Text No;
    [SerializeField] private Text Name;
    [SerializeField] private Text Record;

    public void InitInfo(int no, string name, int record)
    {
        No.text = $"No.{no}";
        Name.text = name;
        Record.text = $"Round {record}";
    }
}
