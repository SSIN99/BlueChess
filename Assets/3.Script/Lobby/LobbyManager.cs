using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Text nickname;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;
    private void Start()
    {
        SetUserInfo();
    }

    private void SetUserInfo()
    {
        if(FirebaseManager.user != null)
        {
            nickname.text = FirebaseManager.user.Email;
        }
        else
        {
            nickname.text = "Error";
        }
    }

    public void GameStart()
    {
        StartCoroutine(LoadSceneAsync("Chess"));
    }
    public void Logout()
    {
        FirebaseManager.Logout();
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}
