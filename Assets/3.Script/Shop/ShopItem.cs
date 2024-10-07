using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Image border;
    [SerializeField] private Image memorial;
    [SerializeField] private Image originIcon;
    [SerializeField] private Text originText;
    [SerializeField] private Image classIcon;
    [SerializeField] private Text classText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Material fx;
    [SerializeField] private Image grade;
    private Color[] colors =
    {
        new Color(1f, 1f, 1f),
        new Color(0.35f, 1f, 0.7f),
        new Color(0.45f, 1f, 1f),
        new Color(1f, 0.35f, 1f),
        new Color(1f, 0.81f, 0.25f)
    };


    [SerializeField] private Info info;
    [SerializeField] private Player player;
    [SerializeField] private UnitManager unitManager;
    private Dictionary<string, string> data;
    public int no;
    public int cost;

    private void Start()
    {
        unitManager.OnGetUnit += SetVFX;
        unitManager.OnSellUnit += SetVFX;
        unitManager.OnBenchChanged += SetBtnState;
        player.OnGoldChanged += SetBtnState;
    }
    public void InitInfo(int n)
    {
        info.unitCount[n] -= 1;
        no = n;
        data = info.Units[n];
        border.color = colors[int.Parse(data["Cost"]) - 1];
        memorial.sprite = info.memorials[n];
        originIcon.sprite = info.traitIcon[int.Parse(data["Origin"])];
        originText.text = info.Traits[int.Parse(data["Origin"])]["Name"];
        classIcon.sprite = info.traitIcon[int.Parse(data["Class"])];
        classText.text = info.Traits[int.Parse(data["Class"])]["Name"];
        nameText.text = data["Name"];
        cost = int.Parse(data["Cost"]);
        costText.text = cost.ToString(); 

        SetVFX();
        SetBtnState();
    }
    public void SetVFX()
    {
        memorial.material = unitManager.CheckHaveUnit(no) ? fx : null;
        switch (unitManager.CheckShopItemVFX(no))
        {
            case 0:
                grade.gameObject.SetActive(false);
                break;
            case 1:
                grade.gameObject.SetActive(true);
                grade.sprite = info.gradeIcon[1];
                break;
            case 2:
                grade.gameObject.SetActive(true);
                grade.sprite = info.gradeIcon[2];
                break;
        }
    }
    public void SetBtnState()
    {
        btn.interactable = player.Gold < cost || unitManager.isFullBench ? false : true;
    }
    public void OnClicked()
    {
        player.UpdateGold(-cost);
        unitManager.GetUnit(no);
        gameObject.SetActive(false);
    }
}
