using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardElement : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text survivalTimeText;

    public void NewScoreElement(string _username, float _survivalTime)
    {
        usernameText.text = _username;

        int min = Mathf.FloorToInt(_survivalTime / 60);
        int sec = Mathf.FloorToInt(_survivalTime % 60);
        survivalTimeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
