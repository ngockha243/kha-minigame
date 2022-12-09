using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lose : UICanvas
{
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TextMeshProUGUI yourScoreText;
    private void Start() {
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("BEST_SCORE", 0);
        yourScoreText.text = "Your Score: " + Character.instance.score;
    }
    public void Home()
    {
        GameManager.instance.Home();
    }
}
