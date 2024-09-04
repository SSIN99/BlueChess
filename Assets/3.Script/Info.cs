using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


public class Info : MonoBehaviour
{
    //유닛별 정보 csv
    public List<Dictionary<string, string>> dataPerUnit;
    //특성별 정보 csv
    public List<Dictionary<string, string>> dataPerTrait;
    //레벨에 따른 가챠확률 csv
    public List<Dictionary<string, string>> ratioPerLevel;
    //유닛별 최대갯수
    public Dictionary<int, int> countPerUnit;
    //유닛 프리펩 리스트
    public List<GameObject> unitPrefabs;
    //유닛 메모리얼 리스트
    public List<Sprite> unitMemorials;
    //특성별 아이콘 리스트
    public List<Sprite> traitIcons;

    public string memorialLabel = "Memorial";
    private string traitLabel = "Trait";

    private void Start()
    {
        dataPerUnit = CSVReader.Read("UnitData");
        dataPerTrait = CSVReader.Read("TraitData");
        ratioPerLevel = CSVReader.Read("UnitRatio");

        InitCountPerUnit();
        //InitResourceList();
    }

    private void InitCountPerUnit()
    {
        countPerUnit = new Dictionary<int, int>();
        for (int i = 0; i < dataPerUnit.Count; i++)
        {
            switch (int.Parse(dataPerUnit[i]["Cost"]))
            {
                case 1:
                    countPerUnit.Add(i, int.Parse(ratioPerLevel[0]["Cost1"]));
                    break;
                case 2:
                    countPerUnit.Add(i, int.Parse(ratioPerLevel[0]["Cost2"]));
                    break;
                case 3:
                    countPerUnit.Add(i, int.Parse(ratioPerLevel[0]["Cost3"]));
                    break;
                case 4:
                    countPerUnit.Add(i, int.Parse(ratioPerLevel[0]["Cost4"]));
                    break;
                case 5:
                    countPerUnit.Add(i, int.Parse(ratioPerLevel[0]["Cost5"]));
                    break;
            }
        }
    }
    
    private void InitResourceList()
    {
        unitPrefabs = new List<GameObject>();
        unitMemorials = new List<Sprite>();
        traitIcons = new List<Sprite>();

        AsyncOperationHandle<IList<Sprite>> handle = Addressables.LoadAssetsAsync<Sprite>(memorialLabel, null);

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (Sprite s in handle.Result)
            {
                unitMemorials.Add(s);
            }

            Debug.Log($"Loaded {unitMemorials.Count} sprites with label: {memorialLabel}");
        }
        else
        {
            Debug.LogError($"Failed to load sprites with label: {memorialLabel}");
        }

    }
}
