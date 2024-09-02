using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public class Test : MonoBehaviour
{
    public string path;
    public Image image;

    private void Start()
    {
        Addressables.LoadAssetAsync<Sprite>(path).Completed += OnImgaeLoaded;
        
    }
    private void OnImgaeLoaded(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            // 에셋 인스턴스화
            image.sprite = obj.Result;
            //Debug.Log("Asset Loaded and Instantiated: " +.name);
        }
        else
        {
            Debug.LogError("Failed to load Addressable Asset: " + path);
        }
    }
}
