using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UnitItem : MonoBehaviour
{
    public Image unitImage;
    public Image originIcon;
    public Image classIcon;
    public Text nameText;
    public Text costText;
    public Text originText;
    public Text classText;

    private GameObject unitPrefab;

    public void SetItemData(GameObject unit, Dictionary<string, object> data)
    {
        unitPrefab = unit;
        nameText.text = data["Name"].ToString();
        costText.text = data["Cost"].ToString();
        originText.text = data["Origin"].ToString();
        classText.text = data["Class"].ToString();

        Addressables.LoadAssetAsync<Sprite>(data["Memorial"]).Completed += OnImgaeLoaded;
    }

    public void OnClicked()
    {
        GameObject newUnit = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);
        newUnit.SetActive(false);
        gameObject.SetActive(false);
    }
    private void OnImgaeLoaded(AsyncOperationHandle<Sprite> image)
    {
        if (image.Status == AsyncOperationStatus.Succeeded)
        {
            unitImage.sprite = image.Result;
        }
    }
}
