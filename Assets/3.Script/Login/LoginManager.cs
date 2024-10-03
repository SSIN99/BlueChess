using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using Firebase.Auth;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private Text title; 
    [SerializeField] private InputField emailInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private Text log;
    
    [SerializeField] private GameObject loginBtn; 
    [SerializeField] private GameObject registerMove; 
    [SerializeField] private GameObject registerBtn; 
    [SerializeField] private GameObject loginMove;
    [SerializeField] private GameObject loading;


    public void OnLogin()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        FirebaseManager.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread((task) =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Login canceled."); 
                log.color = Color.red;
                log.text = "로그인 취소됨";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Login error: " + task.Exception);
                log.color = Color.red;
                log.text = "로그인 오류";
                return;
            }
            Debug.Log("User signed in successfully");
            FirebaseManager.user = task.Result.User;
            log.color = Color.blue;
            log.text = "로그인 성공";
            loading.SetActive(true);
            SceneManager.LoadScene("Lobby");
        });
    }
    public void OnRegister()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        FirebaseManager.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("Signup canceled.");
                log.color = Color.red;
                log.text = "회원가입 취소됨";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Signup error: " + task.Exception);
                log.color = Color.red;
                log.text = "회원가입 오류";
                return;
            }
            Debug.Log("User signed up successfully");
            log.color = Color.blue;
            log.text = "회원가입 성공";
        });
    }
    public void LoginPage()
    {
        title.text = "LOGIN";
        emailInput.text = string.Empty;
        passwordInput.text = string.Empty;
        log.text = string.Empty;
        
        loginBtn.SetActive(true);
        registerMove.SetActive(true);
        registerBtn.SetActive(false);
        loginMove.SetActive(false);
    }
    public void RegisterPage()
    {
        title.text = "REGISTER";
        emailInput.text = string.Empty;
        passwordInput.text = string.Empty;
        log.text = string.Empty;

        registerBtn.SetActive(true);
        loginMove.SetActive(true);
        loginBtn.SetActive(false);
        registerMove.SetActive(false);
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
