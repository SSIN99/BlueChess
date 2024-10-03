using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


public class Info : MonoBehaviour
{
    //유닛별 정보 csv
    public  List<Dictionary<string, string>> Units;
    //특성별 정보 csv
    public  List<Dictionary<string, string>> Traits;
    //레벨에 따른 가챠확률 csv
    public  List<Dictionary<string, string>> Ratios;
    //몬스터 정보 csv 
    public List<Dictionary<string, string>> Enemies;
    //아이템 정보 csv
    public List<Dictionary<string, string>> Items;
    //라운드 정보 csv
    public List<Dictionary<string, string>> Rounds;
    //유닛 풀 갯수 딕셔너리
    public Dictionary<int, int> unitCount;
    //유닛 프리펩 리스트
    public List<GameObject> prefabs;
    //유닛 메모리얼 리스트
    public List<Sprite> memorials;
    //유닛 아이콘 리스트
    public List<Sprite> portraits;
    //에너미 아이콘 리스트
    public List<Sprite> enemyPortraits;
    //유닛 스킬 아이콘 리스트
    public List<Sprite> skillIcon;
    //특성별 아이콘 리스트
    public List<Sprite> traitIcon;
    //아이템 아이콘 리스트 
    public List<Sprite> itemIcon;
    //유닛 등급 아이콘 리스트
    public List<Sprite> gradeIcon;

    private void Awake()
    {
        Units = CSVReader.Read("UnitData");
        Traits = CSVReader.Read("TraitData");
        Ratios = CSVReader.Read("RatioData");
        Enemies = CSVReader.Read("EnemyData");
        Items = CSVReader.Read("ItemData");
        Rounds = CSVReader.Read("RoundData");
        InitUnitPool();
    }
    
    private void InitUnitPool()
    {
        int count = 0;
        unitCount = new Dictionary<int, int>();
        for(int i = 0; i < Units.Count; i++)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            switch (int.Parse(Units[i]["Cost"]))
            {
                case 1:
                    count = int.Parse(Ratios[0]["Wei1"]);
                    break;
                case 2:
                    count = int.Parse(Ratios[0]["Wei2"]);
                    break;
                case 3:
                    count = int.Parse(Ratios[0]["Wei3"]);
                    break;
                case 4:
                    count = int.Parse(Ratios[0]["Wei4"]);
                    break;
                case 5:
                    count = int.Parse(Ratios[0]["Wei5"]);
                    break;
            }
            unitCount.Add(i, count);
        }
    }
}
