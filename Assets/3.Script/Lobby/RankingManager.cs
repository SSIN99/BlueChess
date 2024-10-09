using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField] RankingBarUI myRanking;
    [SerializeField] GameObject rankingBar;
    [SerializeField] Transform content;
    List<FirebaseManager.UserData> userList;

    private void Start()
    {
        userList = new List<FirebaseManager.UserData>();
        InitRanking();
    }

    private void InitRanking()
    {
        FirebaseManager.db.Child("UserData").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {;
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string email = userSnapshot.Child("email").Value.ToString();
                    Debug.Log(userSnapshot.Child("email").Value.ToString());
                    int record = int.Parse(userSnapshot.Child("record").Value.ToString());
                    Debug.Log(userSnapshot.Child("record").Value.ToString());
                    userList.Add(new FirebaseManager.UserData(email, record));
                }

                userList.Sort((x, y) => y.record.CompareTo(x.record));
                GameObject temp;
                for (int i = 0; i < userList.Count; i++)
                {
                    temp = Instantiate(rankingBar, content);
                    temp.GetComponent<RankingBarUI>().InitInfo(i + 1, userList[i].email, userList[i].record);

                    if (FirebaseManager.userData.email == userList[i].email)
                    {
                        myRanking.InitInfo(i + 1, userList[i].email, userList[i].record);
                    }
                }
            }
        });
    }
}
