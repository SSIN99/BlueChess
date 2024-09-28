using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    #region UI
    [SerializeField] private Image border;
    [SerializeField] private Image memorial;
    [SerializeField] private Image originIcon;
    [SerializeField] private Image classIcon;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text originText;
    [SerializeField] private Text classText;
    [SerializeField] private Material fx;
    [SerializeField] private Image grade;
    [SerializeField] private Sprite[] gradeImage;
    private Color[] colorList =
    {
        new Color(1f, 1f, 1f),
        new Color(0.35f, 1f, 0.7f),
        new Color(0.45f, 1f, 1f),
        new Color(1f, 0.35f, 1f),
        new Color(1f, 0.81f, 0.25f)
    };
    #endregion

    #region Info
    [SerializeField] private Info info;
    [SerializeField] private Player player;
    public int no;
    private int cost;
    private Dictionary<string, string> data;
    #endregion

    public void SetItem(int n)
    {
        no = n;
        info.unitCount[no]--;
        data = info.Units[n];

        border.color = colorList[int.Parse(data["Cost"]) - 1];
        memorial.sprite = info.memorials[n];
        originIcon.sprite = info.traitIcon[int.Parse(data["Origin"])];
        classIcon.sprite = info.traitIcon[int.Parse(data["Class"])];
        nameText.text = data["Name"];
        cost = int.Parse(data["Cost"]);
        costText.text = cost.ToString(); 
        originText.text = info.Traits[int.Parse(data["Origin"])]["Name"];
        classText.text = info.Traits[int.Parse(data["Class"])]["Name"];
        SetFx();
        SetGrade();
    }
    public void ReturnItem()
    {
        info.unitCount[no]++;
    }
    public void OnClicked()
    {
        if (player.Gold < cost ||
            player.isFullBench)
        {
            return;
        }
        player.PurchaseUnit(no);
        gameObject.SetActive(false);
    }
    public void SetFx()
    {
        memorial.material = player.CheckHaveUnit(no) ? fx : null;
    }
    public void SetGrade()
    {
        grade.gameObject.SetActive(true);
        switch (player.CheckUnitGrade(no))
        {
            case 0:
                grade.gameObject.SetActive(false);
                break;
            case 1:
                grade.sprite = gradeImage[0];
                break;
            case 2:
                grade.sprite = gradeImage[1];
                break;

        }
    }
    private void OnEnable()
    {
        player.OnPurchaseUnit += SetFx; 
        player.OnSellUnit += SetFx;
        player.OnPurchaseUnit += SetGrade;
        player.OnSellUnit += SetGrade;
    }
    private void OnDisable()
    {
        player.OnPurchaseUnit -= SetFx;
        player.OnSellUnit -= SetFx;
        player.OnPurchaseUnit -= SetGrade;
        player.OnSellUnit -= SetGrade;
    }
}
