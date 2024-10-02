using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoginManager : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private InputField loginEmail;
    [SerializeField] private InputField loginPW;
    [SerializeField] private Text loginLog;
    [Header("SignUp")]
    [SerializeField] private GameObject signUpPanel;
    [SerializeField] private InputField signUpEmail;
    [SerializeField] private InputField signUpPW;
    [SerializeField] private InputField signUpName;
    [SerializeField] private Text SignUpLog;

    public async void OnLoginBtn()
    {
        bool success = await FirebaseManager.Instance.Login(loginEmail.text, loginPW.text);
        if (success)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            loginLog.text = "error";
        }
    }

    public async void OnSignUpBtn()
    {
        bool success = await FirebaseManager.Instance.SignUp(signUpEmail.text, signUpPW.text);
        if (success)
        {
            OpenLogin();
        }
        else
        {
            loginLog.text = "error";
        }
    }
    public void OpenLogin()
    {
        signUpPanel.SetActive(false);
        loginPanel.SetActive(true);
        loginEmail.text = string.Empty;
        loginPW.text = string.Empty;
        loginLog.text = string.Empty;
    }
    public void OpenSignUp()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
        signUpEmail.text = string.Empty;
        signUpPW.text = string.Empty;
        signUpName.text = string.Empty;
        SignUpLog.text = string.Empty;
    }
    public void GameExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); 
        #endif
    }
}
