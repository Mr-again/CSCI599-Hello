using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Sprite lockSprite;

    public Text levelText;

    private int level = 0;

    private Button button;

    private Image image;

    void OnEnable()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

    }

    public void Setup(int level, bool isUnlock)
    {
        this.level = level;
        levelText.text = level.ToString();

        if (isUnlock)
        {
            image.sprite = null;
            button.enabled = true;
            levelText.gameObject.SetActive(true);
        }
        else
        {
            image.sprite = lockSprite;
            button.enabled = false;
            levelText.gameObject.SetActive(false);
        }
    }

    public void onClick()
    {

    }

}
