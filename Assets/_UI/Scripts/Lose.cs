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
        
        // play sound
        SoundManager.instance.PlaySound(SoundType.lose);

        bestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("BEST_SCORE", 0);
        yourScoreText.text = "Your Score: " + Character.instance.score;
    }
    public void Home()
    {
        // play sound
        SoundManager.instance.PlaySound(SoundType.click);

        GameManager.instance.Home();
    }
}
