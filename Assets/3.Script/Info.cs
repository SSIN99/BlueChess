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
    //���� ������Ʈ Ǯ�� ť ����Ʈ
    public List<Queue<GameObject>> unitPool;
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

        InitUnitPool();
    }
    
    private void InitUnitPool()
    {
        int count = 0;
        unitPool = new List<Queue<GameObject>>();
        for(int i = 0; i < unitData.Count; i++)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            switch (int.Parse(unitData[i]["Cost"]))
            {
                case 1:
                    count = int.Parse(ratioData[0]["Wei1"]);
                    break;
                case 2:
                    count = int.Parse(ratioData[0]["Wei2"]);
                    break;
                case 3:
                    count = int.Parse(ratioData[0]["Wei3"]);
                    break;
                case 4:
                    count = int.Parse(ratioData[0]["Wei4"]);
                    break;
                case 5:
                    count = int.Parse(ratioData[0]["Wei5"]);
                    break;
            }
            for (int j =0; j < count; j++)
            {
                GameObject unit = Instantiate(prefabs[i]);
                unit.SetActive(false);
                unit.transform.parent = transform;
                pool.Enqueue(unit);
            }
            unitPool.Add(pool);
        }
        Debug.Log("����Ǯ ����");
    }
}
