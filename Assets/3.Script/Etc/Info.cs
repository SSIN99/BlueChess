using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


public class Info : MonoBehaviour
{
    //���ֺ� ���� csv
    public  List<Dictionary<string, string>> Units;
    //Ư���� ���� csv
    public  List<Dictionary<string, string>> Traits;
    //������ ���� ��íȮ�� csv
    public  List<Dictionary<string, string>> Ratios;
    //���� ���� csv 
    public List<Dictionary<string, string>> Enemies;
    //������ ���� csv
    public List<Dictionary<string, string>> Items;
    //���� ���� csv
    public List<Dictionary<string, string>> Rounds;
    //���� Ǯ ���� ��ųʸ�
    public Dictionary<int, int> unitCount;
    //���� ������ ����Ʈ
    public List<GameObject> prefabs;
    //���� �޸𸮾� ����Ʈ
    public List<Sprite> memorials;
    //���� ������ ����Ʈ
    public List<Sprite> portraits;
    //���ʹ� ������ ����Ʈ
    public List<Sprite> enemyPortraits;
    //���� ��ų ������ ����Ʈ
    public List<Sprite> skillIcon;
    //Ư���� ������ ����Ʈ
    public List<Sprite> traitIcon;
    //������ ������ ����Ʈ 
    public List<Sprite> itemIcon;
    //���� ��� ������ ����Ʈ
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
