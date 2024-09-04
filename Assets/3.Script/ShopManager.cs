using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopItem[] itemList;
    [SerializeField] private Text[] ratio;
    [SerializeField] private Info info;
    [SerializeField] private PlayerControl player;

    private WeightedRandomPicker<int> pickUpPool;
    private bool isLocked = false;

    private void Start()
    {
        pickUpPool = new WeightedRandomPicker<int>();
        SetRatio(player.Level);
        AddUnitPool(1);
        UpdateWeightPool(player.Level);
        SetShopItem();
    }
    
    private void SetRatio(int level) 
    {
        for(int i = 0; i< ratio.Length; i++)
        {
            ratio[i].text = $"{info.ratioPerLevel[level][$"Wei{i + 1}"]}%";
        }
    }
    private void AddUnitPool(int cost)
    {
        for (int i = 0; i < info.dataPerUnit.Count; i++)
        {
            if (int.Parse(info.dataPerUnit[i]["Cost"]).Equals(cost))
            {
                pickUpPool.Add(i, 1);
            }
        }
    }
    private void UpdateWeightPool(int level)
    {
        for (int i = 0; i < pickUpPool.GetCount(); i++)
        {
            switch (int.Parse(info.dataPerUnit[i]["Cost"]))
            {
                case 1:
                    pickUpPool.ModifyWeight(i, double.Parse(info.ratioPerLevel[level]["Cost1"]));
                    break;
                case 2:
                    pickUpPool.ModifyWeight(i, double.Parse(info.ratioPerLevel[level]["Cost2"]));
                    break;
                case 3:
                    pickUpPool.ModifyWeight(i, double.Parse(info.ratioPerLevel[level]["Cost3"]));
                    break;
                case 4:
                    pickUpPool.ModifyWeight(i, double.Parse(info.ratioPerLevel[level]["Cost4"]));
                    break;
                case 5:
                    pickUpPool.ModifyWeight(i, double.Parse(info.ratioPerLevel[level]["Cost5"]));
                    break;
            }
        }
    }
    public void SetShopItem()
    {
        if (isLocked) return;

        foreach(var item in itemList)
        {
            if (item.gameObject.activeSelf.Equals(false))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                info.countPerUnit[item.unitNo]++;
                Debug.Log($"À¯´ÖÈ¸¼ö {info.dataPerUnit[item.unitNo]["Name"]} ÀÜ¿©°¹¼ö { info.countPerUnit[item.unitNo]} ");
            }
            int no = GetRandomUnit();
            item.SetItemInfo(no);
        }
    }
    private int GetRandomUnit()
    {
        int rand = pickUpPool.GetRandomPick();

        while(info.countPerUnit[rand] <= 0)
        {
            rand = pickUpPool.GetRandomPick();
        }
        info.countPerUnit[rand]--;
        Debug.Log($"»ÌÀºÀ¯´Ö {info.dataPerUnit[rand]["Name"]} ÀÜ¿©°¹¼ö { info.countPerUnit[rand]} ");
        return rand;
    }
    public void ToggleLock()
    {
        isLocked = !isLocked;

    }
}
