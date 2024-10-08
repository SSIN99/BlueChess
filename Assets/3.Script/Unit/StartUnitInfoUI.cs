using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUnitInfoUI : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Image originIcon;
    [SerializeField] private Image classIcon;
    [SerializeField] private Text nameText;

    public int no;

    public void InitInfo(int no)
    {
        this.no = no;
        originIcon.sprite = info.traitIcon[int.Parse(info.Units[no]["Origin"])];
        classIcon.sprite = info.traitIcon[int.Parse(info.Units[no]["Class"])];
        nameText.text = info.Units[no]["Name"];
    }
}
