using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //���ֺ� ���� csv
    private List<Dictionary<string, string>> unitData;
    //������ ���� ��íȮ�� csv
    private List<Dictionary<string, string>> unitRatio;
    //���ֺ� �ִ밹��
    private Dictionary<int, int> unitCount;
    //���� ������ ����Ʈ
    public GameObject[] unitList;
    //���� �޸𸮾� ����Ʈ
    public Sprite[] unitMemorial;
    //���� ��ư��
    public UnitItem[] unitItems;
    //���� ��íǮ 
    private WeightedRandomPicker<int> unitPool;
    //���� ��� bool��
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
    //�ʱ� ���� ī��Ʈ ����
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

    // �ʱ� ����Ǯ ���� 
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
    // ������ ���� ����Ǯ Ȯ�� ����
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
    // ���� ��ư ������ ���� ����
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
                Debug.Log($"���ֳѹ� {unitNo} �ܿ����� {unitCount[unitNo]} ");
            }
            item.SetItemData(unitNo ,unitList[unitNo],unitMemorial[unitNo], unitData[unitNo]);
            Debug.Log($"�������� {unitNo}");
        }
    }
    // ���� ���� �̱�
    private int GetRandomUnit()
    {
        int unitNo = unitPool.GetRandomPick();
        while(unitCount[unitNo] <= 0)
        {
            unitNo = unitPool.GetRandomPick();
        }
        unitCount[unitNo]--;
        Debug.Log($"���ֳѹ� {unitNo} �ܿ����� {unitCount[unitNo]} ");
        return unitNo;
    }
    public void ToggleLock()
    {
        isLocked = !isLocked;

    }
}
