using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


public class Info : MonoBehaviour
{
    //���ֺ� ���� csv
    public  List<Dictionary<string, string>> unitData;
    //Ư���� ���� csv
    public  List<Dictionary<string, string>> traitData;
    //������ ���� ��íȮ�� csv
    public  List<Dictionary<string, string>> ratioData;
    //���ֺ� �ִ밹��
    public Dictionary<int, int> unitCount;
    //���� ������ ����Ʈ
    public List<GameObject> prefabs;
    //���� �޸𸮾� ����Ʈ
    public List<Sprite> memorials;
    //Ư���� ������ ����Ʈ
    public List<Sprite> traits;

    private void Awake()
    {
        unitData = CSVReader.Read("UnitData");
        traitData = CSVReader.Read("TraitData");
        ratioData = CSVReader.Read("RatioData");

        InitCountPerUnit();
    }

    private void InitCountPerUnit()
    {
        unitCount = new Dictionary<int, int>();
        for (int i = 0; i < unitData.Count; i++)
        {
            switch (int.Parse(unitData[i]["Cost"]))
            {
                case 1:
                    unitCount.Add(i, int.Parse(ratioData[0]["Wei1"]));
                    break;
                case 2:
                    unitCount.Add(i, int.Parse(ratioData[0]["Wei2"]));
                    break;
                case 3:
                    unitCount.Add(i, int.Parse(ratioData[0]["Wei3"]));
                    break;
                case 4:
                    unitCount.Add(i, int.Parse(ratioData[0]["Wei4"]));
                    break;
                case 5:
                    unitCount.Add(i, int.Parse(ratioData[0]["Wei5"]));
                    break;
            }
        }
    } 
}
