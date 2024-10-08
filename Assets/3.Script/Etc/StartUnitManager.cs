using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUnitManager : MonoBehaviour
{
    [SerializeField] UnitManager unitManager;
    [SerializeField] GameObject[] modelPrefabs;
    [SerializeField] Transform[] modelPosition;
    [SerializeField] StartUnitInfoUI[] startUnitInfos;
    List<int> randomUnitList;

    private void Awake()
    {
        randomUnitList = new List<int>();
    }
    private void Start()
    {
        GetRandomModel();
    }

    public void GetRandomModel()
    {
        GameObject model = null;
        int rand = 0;

        for(int i = 0; i < modelPosition.Length; i++)
        {
            rand = Random.Range(0, 13);
            model = Instantiate(modelPrefabs[rand], modelPosition[i].position, Quaternion.Euler(0, -180, 0), transform);
            startUnitInfos[i].InitInfo(rand);
            randomUnitList.Add(rand);
        }
    }
    public void GetSelectedUnit()
    {
        for (int i = 0; i < randomUnitList.Count; i++)
        {
            unitManager.GetUnit(randomUnitList[i]);
        }
        Destroy(gameObject);
    }
}
