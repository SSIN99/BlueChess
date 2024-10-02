using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    private FirebaseAuth auth;
    private DatabaseReference db;

    //[SerializeField] private Text loginLog;
    //[SerializeField] private Text signUpLog;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }
    public async Task<bool> Login(string email, string password)
    {
        try
        {
            await auth.SignInWithEmailAndPasswordAsync(email, password);
            return true;
        }
        catch (Firebase.FirebaseException ex)
        {
            return false;
        }
    }
    public async Task<bool> SignUp(string email, string password)
    {
        try
        {
            await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            return true;
        }
        catch (Firebase.FirebaseException ex)
        {
            return false;
        }
    }
    public void Logout()
    {
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }
}
