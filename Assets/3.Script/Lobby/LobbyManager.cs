using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Text nickname;

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
        SceneManager.LoadScene("Chess");
    }
}
