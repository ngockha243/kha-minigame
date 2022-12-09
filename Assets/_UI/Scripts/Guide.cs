using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide : UICanvas
{
    public void Play()
    {
        UIManager.instance.OpenUI<Play>();
        GameplayManager.instance.onSetting = false;
        Close();
    }
}
