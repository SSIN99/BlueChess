using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUIHandler : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private GameObject Panel;
    [SerializeField] private Image Icon;
    [SerializeField] private Text Name;
    [SerializeField] private Text Script;

    public void InitInfo(int no)
    {
        Panel.SetActive(true);
        Icon.sprite = info.itemIcon[no];
        Name.text = info.Items[no]["Name"];
        Script.text = info.Items[no]["Script"];
    }
    public void Off()
    {
        Panel.SetActive(false);
    }
}
