using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanel : FishMonoClass
{
    TextMeshProUGUI ScoreText;
    TextMeshProUGUI ComboText;
    void Start()
    {
        ScoreText = transform.Find("ScoreValueText").GetComponent<TextMeshProUGUI>();
        ComboText = transform.Find("ComboValueText").GetComponent<TextMeshProUGUI>();

        RegisterMessage<OnScoreUpdateMessageData>(FishMessageDefine.OnScoreUpdate, OnScoreUpdate);
        RegisterMessage<OnComboUpdateMessageData>(FishMessageDefine.OnComboUpdate, OnComboUpdate);
    }

    void OnScoreUpdate(OnScoreUpdateMessageData onScoreUpdateMessageData)
    {
        ScoreText.text = onScoreUpdateMessageData.new_score.ToString();
    }

    void OnComboUpdate(OnComboUpdateMessageData comboUpdateMessageData)
    {
        ComboText.text = comboUpdateMessageData.ComboCount.ToString();
    }
}
