using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //���ֺ� ���� csv
    private List<Dictionary<string, object>> unitData;
    //������ ���� ��íȮ�� 
    private List<Dictionary<string, object>> unitRatio;

    //���� ������ ����Ʈ
    public GameObject[] unitList;
    //���ֺ� �ִ밹��
    private Dictionary<string, int> unitCount;
    //���� ��íǮ 
    private WeightedRandomPicker<int> unitPool;
    //���� ��ư��
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
    //�ʱ� ���� ī��Ʈ ����
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

    // �ʱ� ����Ǯ ���� 
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

    // ������ ���� ����Ǯ Ȯ�� ����
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
