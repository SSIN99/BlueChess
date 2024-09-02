using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //À¯´Öº° Á¤º¸ csv
    private List<Dictionary<string, object>> unitData;
    //·¹º§¿¡ µû¸¥ °¡Ã­È®·ü 
    private List<Dictionary<string, object>> unitRatio;

    //À¯´Ö ÇÁ¸®Æé ¸®½ºÆ®
    public GameObject[] unitList;
    //À¯´Öº° ÃÖ´ë°¹¼ö
    private Dictionary<string, int> unitCount;
    //À¯´Ö °¡Ã­Ç® 
    private WeightedRandomPicker<int> unitPool;
    //À¯´Ö ¹öÆ°µé
    public UnitItem[] unitItems; 

    private void Start()
    {
        unitData = CSVReader.Read("UnitData");
        unitRatio = CSVReader.Read("UnitRatio");
        unitCount = new Dictionary<string, int>();
        unitPool = new WeightedRandomPicker<int>();

        InitUnitCount();
        InitUnitPool();
        SetUnitItem();
    }
    //ÃÊ±â À¯´Ö Ä«¿îÆ® ¼¼ÆÃ
    private void InitUnitCount()
    {
        for (int i = 0; i < unitData.Count; i++)
        {
            switch (unitData[i]["Cost"])
            {
                case 1:
                    unitCount.Add(unitData[i]["Name"].ToString(), (int)unitRatio[8]["Cost1"]);
                    break;
                case 2:
                    unitCount.Add(unitData[i]["Name"].ToString(), (int)unitRatio[8]["Cost2"]);
                    break;
                case 3:
                    unitCount.Add(unitData[i]["Name"].ToString(), (int)unitRatio[8]["Cost3"]);
                    break;
                case 4:
                    unitCount.Add(unitData[i]["Name"].ToString(), (int)unitRatio[8]["Cost4"]);
                    break;
                case 5:
                    unitCount.Add(unitData[i]["Name"].ToString(), (int)unitRatio[8]["Cost5"]);
                    break;
            }
        }
    }

    // ÃÊ±â À¯´ÖÇ® ¼¼ÆÃ 
    private void InitUnitPool() 
    {
        for(int i = 0; i < unitList.Length; i++)
        {
            switch (unitData[i]["Cost"])
            {
                case 1:
                    unitPool.Add(i, 100);
                    break;
                default:
                    unitPool.Add(i, 0);
                    break;
            }
        }
    }

    // ·¹º§¿¡ µû¶ó À¯´ÖÇ® È®·ü °»½Å
    private void UpdateUnitPool()
    {

    }

    public void SetUnitItem()
    {
        foreach(var i in unitItems)
        {
            int unitNo = unitPool.GetRandomPick();
            if (i.gameObject.activeSelf.Equals(false))
            {
                i.gameObject.SetActive(true);
            }
            i.SetItemData(unitList[unitNo], unitData[unitNo]);
            Debug.Log(unitNo);
        }
    }
}
