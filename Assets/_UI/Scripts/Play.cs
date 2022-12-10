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
    [SerializeField] private Slider increaseSlider;
    [SerializeField] private GameObject increase;
    private void FixedUpdate() {
        // Update score text
        scoreText.text = "Your score: " + Character.instance.score;

        // Show fall time
        // max time = time fall - difficulty time
        float maxTime = Character.instance.GetMaxTime() - Character.instance.difficultyTime;
        float timeShow = Mathf.Clamp(Character.instance.timerForFall, 0f, maxTime);

        timeSlider.value = (maxTime - timeShow)/maxTime;

        // Show combo: if combo >= 10 --> show combo
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

        // Show time increase point: if increase point != 1 --> Show timer and show label
        if(Character.instance.increasePoint > 1)
        {
            // show timer
            float timeIncrease = Character.instance.timerIncreasePoint;
            float maxTimeIncrease = Character.instance.GetTimeIncrease();
            increaseSlider.value = timeIncrease / maxTimeIncrease;

            increasePoint.SetActive(true);
            increase.SetActive(true);
        }
        else
        {
            increasePoint.SetActive(false);
            increase.SetActive(false);
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
