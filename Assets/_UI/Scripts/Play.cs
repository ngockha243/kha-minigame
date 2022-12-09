using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Play : UICanvas
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject combo;
    [SerializeField] private GameObject increasePoint;
    private void FixedUpdate() {
        scoreText.text = "Your score: " + Character.instance.score;
        float maxTime = Character.instance.GetMaxTime() - Character.instance.difficultyTime;
        float timeShow = Mathf.Clamp(Character.instance.timerForFall, 0f, maxTime);
        timeSlider.value = (maxTime - timeShow)/maxTime;

        int numCombo = Character.instance.combo;
        if(numCombo >= 10)
        {
            comboText.text = numCombo.ToString();
            combo.SetActive(true);
        }
        else
        {
            combo.SetActive(false);
        }
        if(Character.instance.increasePoint > 1)
        {
            increasePoint.SetActive(true);
        }
        else
        {
            increasePoint.SetActive(false);
        }
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
