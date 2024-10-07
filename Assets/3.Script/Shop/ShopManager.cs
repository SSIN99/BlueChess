using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Player player;
    [SerializeField] private ShopItem[] itemList;
    [SerializeField] private Text[] ratioText;
    [SerializeField] private Button expBtn;
    [SerializeField] private Button rerollBtn;
    [SerializeField] private Image lockBtn;
    [SerializeField] private Sprite[] lockSprite;
    public bool isLocked;
    private WeightedRandomPicker<int> unitPool;

    private void AddUnitPool()
    {
        int count = unitPool.GetCount();
        switch (player.Level)
        {
            case 1:
                for (int i = 0; i < int.Parse(info.Ratios[0]["Ratio1"]); i++)
                {
                    unitPool.Add(i, 1);
                }
                break;
            case 3:
                for (int i = 0; i < int.Parse(info.Ratios[0]["Ratio2"]); i++)
                {
                    unitPool.Add(i + count, 1);
                }
                count = unitPool.GetCount();
                for (int i = 0; i < int.Parse(info.Ratios[0]["Ratio3"]); i++)
                {
                    unitPool.Add(i + count, 1);
                }
                break;
            case 5:
                for (int i = 0; i < int.Parse(info.Ratios[0]["Ratio4"]); i++)
                {
                    unitPool.Add(i + count, 1);
                }
                break;
            case 7:
                for (int i = 0; i < int.Parse(info.Ratios[0]["Ratio5"]); i++)
                {
                    unitPool.Add(i + count, 1);
                }
                break;
            default:
                return;
        }
    }
    private void UpdateWeightPool()
    {
        int unitCount = unitPool.GetCount(); 
        for (int i = 0; i < unitCount; i++)
        {
            switch (int.Parse(info.Units[i]["Cost"]))
            {
                case 1:
                    unitPool.ModifyWeight(i, double.Parse(info.Ratios[player.Level]["Wei1"]));
                    break;
                case 2:
                    unitPool.ModifyWeight(i, double.Parse(info.Ratios[player.Level]["Wei2"]));
                    break;
                case 3:
                    unitPool.ModifyWeight(i, double.Parse(info.Ratios[player.Level]["Wei3"]));
                    break;
                case 4:
                    unitPool.ModifyWeight(i, double.Parse(info.Ratios[player.Level]["Wei4"]));
                    break;
                case 5:
                    unitPool.ModifyWeight(i, double.Parse(info.Ratios[player.Level]["Wei5"]));
                    break;
            }
        }
    }
    private void SetRatio() 
    {
        for(int i = 0; i< ratioText.Length; i++)
        {
            ratioText[i].text = $"{info.Ratios[player.Level][$"Ratio{i + 1}"]}%";
        }
    }
    public void PurchaseExp()
    {
        player.UpdateGold(-4);
        player.GetExp(4);
    }
    public void RerollShop()
    {
        player.UpdateGold(-2);
        if (isLocked)
        {
            SetLockState();
        }
        foreach(var item in itemList)
        {
            if (item.gameObject.activeSelf.Equals(false))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                info.unitCount[item.no] += 1;
            }
            int no = GetRandomUnit();
            item.InitInfo(no);
        }
    }
    public void RoundReroll()
    {
        if (isLocked) return;
 
        foreach (var item in itemList)
        {
            if (item.gameObject.activeSelf.Equals(false))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                info.unitCount[item.no] += 1; 
            }
            int no = GetRandomUnit();
            item.InitInfo(no);
        }
    }
    public void SetLockState()
    {
        isLocked = !isLocked;
        lockBtn.sprite = isLocked ? lockSprite[0] : lockSprite[1];
    }
    private int GetRandomUnit()
    {
        int rand = unitPool.GetRandomPick();

        while(info.unitCount[rand].Equals(0))
        {
            rand = unitPool.GetRandomPick();
        }
        return rand;
    }
    private void SetButtonState()
    {
        if(player.Gold < 2)
        {
            expBtn.interactable = false;
            rerollBtn.interactable = false;
        }
        else if(player.Gold < 4)
        {
            expBtn.interactable = false;
            rerollBtn.interactable = true;
        }
        else
        {
            expBtn.interactable = true;
            rerollBtn.interactable = true;
        }
        if(player.Level == 9)
        {
            expBtn.interactable = false;
        }
    }
    private void OnEnable()
    {
        unitPool = new WeightedRandomPicker<int>();
        player.OnLevelChanged += AddUnitPool;
        player.OnLevelChanged += UpdateWeightPool;
        player.OnLevelChanged += SetRatio;
        player.OnGoldChanged += SetButtonState;
    }
    private void OnDisable()
    {
        player.OnLevelChanged -= AddUnitPool;
        player.OnLevelChanged -= UpdateWeightPool;
        player.OnLevelChanged -= SetRatio;
        player.OnGoldChanged -= SetButtonState;
    }
}
