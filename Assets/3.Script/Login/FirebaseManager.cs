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
    public static UserData userData;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }
    public static void Logout()
    {
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }

    public static void SaveUserData(FirebaseUser user)
    {
        if (user != null)
        {
            string userId = user.UserId;
            string email = user.Email;

            // ���� ������ ����
            UserData userData = new UserData(email, 0);

            // �����ͺ��̽��� ����
            db.Child("UserData").Child(userId).SetRawJsonValueAsync(JsonUtility.ToJson(userData))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log("����ID�� ����� ����Ǿ����ϴ�.");
                    }
                    else
                    {
                        Debug.LogError("������ ���� �� ���� �߻�: " + task.Exception);
                    }
                });
        }
        else
        {
            Debug.LogError("������ �����ϴ�.");
        }
    }
    public static void LoadUserData(string userID)
    {
        db.Child("UserData").Child(userID).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("�����͸� �ҷ����� �� ���� �߻�: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string email = snapshot.Child("email").Value.ToString();
                    int record = int.Parse(snapshot.Child("record").Value.ToString());
                    userData = new UserData(email, record);
                }
                else
                {
                    Debug.Log("���� �����͸� ã�� �� �����ϴ�.");
                }
            }
        });
    }
    public static void UpdateUserRecord(string userId, int newRecord)
    {
        string path = $"UserData/{userId}/record";
        db.Child(path).SetValueAsync(newRecord).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Record updated successfully!");
            }
            else
            {
                Debug.LogError("Failed to update record: " + task.Exception);
            }
        });
    }
    [System.Serializable]
    public class UserData
    {
        public string email;
        public int record;

        public UserData(string email, int record)
        {
            this.email = email;
            this.record = record;
        }
    }
}
