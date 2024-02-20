using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanel : FishMonoClass
{
    TextMeshProUGUI ScoreText;
    TextMeshProUGUI ComboText;
    TextMeshProUGUI MaxScoreText;

    void Start()
    {
        ScoreText = transform.Find("ScoreValueText").GetComponent<TextMeshProUGUI>();
        ComboText = transform.Find("ComboValueText").GetComponent<TextMeshProUGUI>();
        MaxScoreText = transform.Find("MaxScoreText").GetComponent<TextMeshProUGUI>();

        RegisterMessage<OnScoreUpdateMessageData>(FishMessageDefine.OnScoreUpdate, OnScoreUpdate);
        RegisterMessage<OnComboUpdateMessageData>(FishMessageDefine.OnComboUpdate, OnComboUpdate);

        MaxScoreText.text = ScoreData.GetInstnace.GetPlayerMaxScore().ToString();
    }

    void OnScoreUpdate(OnScoreUpdateMessageData onScoreUpdateMessageData)
    {
        ScoreText.text = onScoreUpdateMessageData.new_score.ToString();
        MaxScoreText.text = ScoreData.GetInstnace.GetPlayerMaxScore().ToString();
    }

    void OnComboUpdate(OnComboUpdateMessageData comboUpdateMessageData)
    {
        ComboText.text = comboUpdateMessageData.ComboCount.ToString();
    }
}
