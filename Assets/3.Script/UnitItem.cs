using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UnitItem : MonoBehaviour
{
    [Header("UnitData Indicator UI")]
    public Image memorial;
    public Image originIcon;
    public Image classIcon;
    public Image border;
    public Text nameText;
    public Text costText;
    public Text originText;
    public Text classText;

    public int unitNo;
    private GameObject unitPrefab;
    private Color[] rankColor =
    {
        new Color(1f, 1f, 1f),
        new Color(0.35f, 1f, 0.7f),
        new Color(0.45f, 1f, 1f),
        new Color(1f, 0.35f, 1f),
        new Color(1f, 0.81f, 0.25f)
    };

    public void SetItemData(int no, GameObject unit, Sprite memo, Dictionary<string, string> data)
    {
        if (unitPrefab.Equals(unit)) return;

        unitNo = no;
        unitPrefab = unit;
        memorial.sprite = memo;
        border.color = rankColor[int.Parse(data["Cost"]) - 1];
        nameText.text = data["Name"];
        costText.text = data["Cost"];
        originText.text = data["Origin"];
        classText.text = data["Class"];

    }

    public void OnClicked()
    {
        GameObject newUnit = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);
        newUnit.SetActive(false);
        unitPrefab = null;
        gameObject.SetActive(false);
    }
}
