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
    //��ų ���� csv
    public List<Dictionary<string, string>> Skills;
    //���� Ǯ ���� ��ųʸ�
    public Dictionary<int, int> unitCount;
    //���� ������ ����Ʈ
    public GameObject[] prefabs;
    //���� �޸𸮾� ����Ʈ
    public Sprite[] memorials;
    //���� ������ ����Ʈ
    public Sprite[] portraits;
    //���ʹ� ������ ����Ʈ
    public Sprite[] enemyPortraits;
    //���� ��ų ������ ����Ʈ
    public Sprite[] skillIcon;
    //Ư���� ������ ����Ʈ
    public Sprite[] traitIcon;
    //������ ������ ����Ʈ 
    public Sprite[] itemIcon;
    //���� ��� ������ ����Ʈ
    public Sprite[] gradeIcon;

    private void Awake()
    {
        Units = CSVReader.Read("UnitData");
        Traits = CSVReader.Read("TraitData");
        Ratios = CSVReader.Read("RatioData");
        Enemies = CSVReader.Read("EnemyData");
        Items = CSVReader.Read("ItemData");
        Rounds = CSVReader.Read("RoundData");
        Skills = CSVReader.Read("SkillData");
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
