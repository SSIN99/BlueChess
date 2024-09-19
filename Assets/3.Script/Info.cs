using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


public class Info : MonoBehaviour
{
    //유닛별 정보 csv
    public  List<Dictionary<string, string>> unitData;
    //특성별 정보 csv
    public  List<Dictionary<string, string>> traitData;
    //레벨에 따른 가챠확률 csv
    public  List<Dictionary<string, string>> ratioData;
    //유닛 오브젝트 풀링 큐 리스트
    public List<Queue<GameObject>> unitPool;
    //유닛 프리펩 리스트
    public List<GameObject> prefabs;
    //유닛 메모리얼 리스트
    public List<Sprite> memorials;
    //특성별 아이콘 리스트
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
        Debug.Log("유닛풀 생성");
    }
}
