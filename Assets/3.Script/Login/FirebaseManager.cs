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
    public static FirebaseAuth auth;
    public static DatabaseReference db;
    public static FirebaseUser user;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }
    public void Logout()
    {
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }
}
