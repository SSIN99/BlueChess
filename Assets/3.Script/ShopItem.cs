using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("UnitInfo Image")]
    public Image border;
    public Image memorial;
    public Image originIcon;
    public Image classIcon;
    [Header("UnitInfo Text")]
    public Text nameText;
    public Text costText;
    public Text originText;
    public Text classText;

    public Info info;
    public int unitNo;
    private GameObject unitPrefab;
    private Color[] colorOfCost =
    {
        new Color(1f, 1f, 1f),
        new Color(0.35f, 1f, 0.7f),
        new Color(0.45f, 1f, 1f),
        new Color(1f, 0.35f, 1f),
        new Color(1f, 0.81f, 0.25f)
    };

    public void SetItemInfo(int no)
    {
        //if (unitPrefab.Equals(info.unitPrefabs[no])) return;

        unitNo = no;
        //unitPrefab = info.unitPrefabs[no];

        border.color = colorOfCost[int.Parse(info.dataPerUnit[no]["Cost"]) - 1];
        //Debug.Log("컬러세팅");
        memorial.sprite = info.unitMemorials[no];
        //Debug.Log("메모리얼");
        originIcon.sprite = info.traitIcons[int.Parse(info.dataPerUnit[no]["Origin"])];
        //Debug.Log("소속");
        classIcon.sprite = info.traitIcons[int.Parse(info.dataPerUnit[no]["Class"])];
        //Debug.Log("직업");

        nameText.text = info.dataPerUnit[no]["Name"];
        //Debug.Log("이름");
        costText.text = info.dataPerUnit[no]["Cost"];
        //Debug.Log("비용");
        originText.text = info.dataPerTrait[int.Parse(info.dataPerUnit[no]["Origin"])]["Name"];
        //Debug.Log("소속 텍스트");
        classText.text = info.dataPerTrait[int.Parse(info.dataPerUnit[no]["Class"])]["Name"];
        //Debug.Log("직업 텍스트");
    }

    public void OnClicked()
    {
        //GameObject newUnit = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);
        unitPrefab = null;
        gameObject.SetActive(false);
    }
}
