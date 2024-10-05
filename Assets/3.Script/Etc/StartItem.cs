using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartItem : MonoBehaviour
{
    #region UI
    [SerializeField] private Image memorial;
    [SerializeField] private Image originIcon;
    [SerializeField] private Image classIcon;
    [SerializeField] private Text nameText;
    [SerializeField] private Text originText;
    [SerializeField] private Text classText;
    #endregion

    #region Info
    [SerializeField] private Info info;
    public int no;
    private Dictionary<string, string> data;
    #endregion

    public void SetItem(int n)
    {
        no = n;
        data = info.Units[n];
        memorial.sprite = info.memorials[n];
        originIcon.sprite = info.traitIcon[int.Parse(data["Origin"])];
        classIcon.sprite = info.traitIcon[int.Parse(data["Class"])];
        nameText.text = data["Name"];
        originText.text = info.Traits[int.Parse(data["Origin"])]["Name"];
        classText.text = info.Traits[int.Parse(data["Class"])]["Name"];
    }
}
