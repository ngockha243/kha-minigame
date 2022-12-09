using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : UICanvas
{
    [SerializeField] TextMeshProUGUI bestScoreText;
    private void Start() {
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("BEST_SCORE", 0);
    }
    public void Play()
    {
        UIManager.instance.OpenUI<Guide>();
        Close();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
