using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Play : UICanvas
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Update() {
        scoreText.text = "Your score: " + Character.instance.score;
    }
    public void Left()
    {
        Character.instance.MoveLeft();
    }
    public void Right()
    {
        Character.instance.MoveRight();
    }
}
