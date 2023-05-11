using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey= "797a4cdd39792f18e6e4ea5cc3f8078f7da15bcf52502028a9279523bcbb9eeb";

    public void GetLeaderboard() {
        GetLeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>{
            for (int i = 0; i < names.Count; i++){
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString;
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score){
        GetLeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score,((msj) => {
            GetLeaderboard();

        }));
    }
} 
