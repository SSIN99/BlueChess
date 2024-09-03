using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //À¯´Öº° Á¤º¸ csv
    private List<Dictionary<string, string>> unitData;
    //·¹º§¿¡ µû¸¥ °¡Ã­È®·ü csv
    private List<Dictionary<string, string>> unitRatio;
    //À¯´Öº° ÃÖ´ë°¹¼ö
    private Dictionary<int, int> unitCount;
    //À¯´Ö ÇÁ¸®Æé ¸®½ºÆ®
    public GameObject[] unitList;
    //À¯´Ö ¸Þ¸ð¸®¾ó ¸®½ºÆ®
    public Sprite[] unitMemorial;
    //À¯´Ö ¹öÆ°µé
    public UnitItem[] unitItems;
    //À¯´Ö °¡Ã­Ç® 
    private WeightedRandomPicker<int> unitPool;
    //»óÁ¡ Àá±Ý bool°ª
    private bool isLocked = false;

    private void Start()
    {
        unitData = CSVReader.Read("UnitData");
        unitRatio = CSVReader.Read("UnitRatio");
        unitCount = new Dictionary<int, int>();
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
            switch (int.Parse(unitData[i]["Cost"]))
            {
                case 1:
                    unitCount.Add(i, int.Parse(unitRatio[0]["Cost1"]));
                    break;
                case 2:
                    unitCount.Add(i, int.Parse(unitRatio[0]["Cost2"]));
                    break;
                case 3:
                    unitCount.Add(i, int.Parse(unitRatio[0]["Cost3"]));
                    break;
                case 4:
                    unitCount.Add(i, int.Parse(unitRatio[0]["Cost4"]));
                    break;
                case 5:
                    unitCount.Add(i, int.Parse(unitRatio[0]["Cost5"]));
                    break;
            }
        }
    }

    // ÃÊ±â À¯´ÖÇ® ¼¼ÆÃ 
    private void InitUnitPool() 
    {
        for(int i = 0; i < unitData.Count; i++)
        {
            if (unitData[i]["Cost"].Equals(1))
            {
                unitPool.Add(i, 100);        
            }
            else
            {
                unitPool.Add(i, 0);
            }
        }
    }
    // ·¹º§¿¡ µû¶ó À¯´ÖÇ® È®·ü °»½Å
    public void UpdateUnitPool(int level)
    {
        for (int i = 0; i < unitData.Count; i++)
        {
            switch (int.Parse(unitData[i]["Cost"]))
            {
                case 1:
                    unitPool.ModifyWeight(i, int.Parse(unitRatio[0]["Cost1"]));
                    break;
                case 2:
                    unitPool.ModifyWeight(i, int.Parse(unitRatio[0]["Cost2"]));
                    break;
                case 3:
                    unitPool.ModifyWeight(i, int.Parse(unitRatio[0]["Cost3"]));
                    break;
                case 4:
                    unitPool.ModifyWeight(i, int.Parse(unitRatio[0]["Cost4"]));
                    break;
                case 5:
                    unitPool.ModifyWeight(i, int.Parse(unitRatio[0]["Cost5"]));
                    break;
            }
        }
    }
    // À¯´Ö ¹öÆ° ¾ÆÀÌÅÛ Á¤º¸ ¼¼ÆÃ
    public void SetUnitItem()
    {
        if (isLocked) return;

        foreach(var item in unitItems)
        {
            int unitNo = GetRandomUnit();
            if (item.gameObject.activeSelf.Equals(false))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                unitCount[item.unitNo]++;
                Debug.Log($"À¯´Ö³Ñ¹ö {unitNo} ÀÜ¿©°¹¼ö {unitCount[unitNo]} ");
            }
            item.SetItemData(unitNo ,unitList[unitNo],unitMemorial[unitNo], unitData[unitNo]);
            Debug.Log($"»ÌÈùÀ¯´Ö {unitNo}");
        }
    }
    // ·£´ý À¯´Ö »Ì±â
    private int GetRandomUnit()
    {
        int unitNo = unitPool.GetRandomPick();
        while(unitCount[unitNo] <= 0)
        {
            unitNo = unitPool.GetRandomPick();
        }
        unitCount[unitNo]--;
        Debug.Log($"À¯´Ö³Ñ¹ö {unitNo} ÀÜ¿©°¹¼ö {unitCount[unitNo]} ");
        return unitNo;
    }
    public void ToggleLock()
    {
        isLocked = !isLocked;

    }
}
