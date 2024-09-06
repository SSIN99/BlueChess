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

    private WeightedRandomPicker<int> unitPool;
    private int poolCount = 0;

    private void OnEnable()
    {
        player.OnLevelUp += AddUnitPool;
        player.OnLevelUp += UpdateWeightPool;
        player.OnLevelUp += SetRatio;
    }
    private void Start()
    {
        unitPool = new WeightedRandomPicker<int>();
    }
    private void SetRatio() 
    {
        for(int i = 0; i< ratioText.Length; i++)
        {
            ratioText[i].text = $"{info.ratioData[player.Level][$"Ratio{i + 1}"]}%";
        }
    }
    private void AddUnitPool()
    {
        switch (player.Level)
        {
            case 1:
                for (int i = 0; i < int.Parse(info.ratioData[0]["Ratio1"]); i++)
                {
                    unitPool.Add(i + poolCount, 1);
                }
                break;
            case 3:
                for (int i = 0; i < int.Parse(info.ratioData[0]["Ratio2"]); i++)
                {
                    unitPool.Add(i + poolCount, 1);
                }
                poolCount = unitPool.GetCount();
                for (int i = 0; i < int.Parse(info.ratioData[0]["Ratio3"]); i++)
                {
                    unitPool.Add(i + poolCount, 1);
                }
                break;
            case 5:
                for (int i = 0; i < int.Parse(info.ratioData[0]["Ratio4"]); i++)
                {
                    unitPool.Add(i + poolCount, 1);
                }
                break;
            case 7:
                for (int i = 0; i < int.Parse(info.ratioData[0]["Ratio5"]); i++)
                {
                    unitPool.Add(i + poolCount, 1);
                }
                break;
            default:
                return;
        }
        poolCount = unitPool.GetCount();
    }
    private void UpdateWeightPool()
    {
        for (int i = 0; i < poolCount; i++)
        {
            switch (int.Parse(info.unitData[i]["Cost"]))
            {
                case 1:
                    unitPool.ModifyWeight(i, double.Parse(info.ratioData[player.Level]["Wei1"]));
                    break;
                case 2:
                    unitPool.ModifyWeight(i, double.Parse(info.ratioData[player.Level]["Wei2"]));
                    break;
                case 3:
                    unitPool.ModifyWeight(i, double.Parse(info.ratioData[player.Level]["Wei3"]));
                    break;
                case 4:
                    unitPool.ModifyWeight(i, double.Parse(info.ratioData[player.Level]["Wei4"]));
                    break;
                case 5:
                    unitPool.ModifyWeight(i, double.Parse(info.ratioData[player.Level]["Wei5"]));
                    break;
            }
        }
    }
    public void SetShopItem()
    {
        foreach(var item in itemList)
        {
            if (item.gameObject.activeSelf.Equals(false))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.ReturnUnit(); 
            }
            int no = GetRandomUnit();
            item.SetItemInfo(no);
        }
    }
    private int GetRandomUnit()
    {
        int rand = unitPool.GetRandomPick();

        while(info.unitPool[rand].Count.Equals(0))
        {
            rand = unitPool.GetRandomPick();
            if(info.unitPool[rand].Count < 0)
            {
                Debug.Log("풀 갯수 오류 발생!!!");
                break;
            }
        }
        return rand;
    }


}
