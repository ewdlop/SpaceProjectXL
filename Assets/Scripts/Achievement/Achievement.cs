using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement
{

    private string name;
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    private string description;
    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    private bool unlocked;
    public bool Unlocked
    {
        get
        {
            return unlocked;
        }

        set
        {
            unlocked = value;
        }
    }

    private int points;
    public int Points
    {
        get
        {
            return points;
        }

        set
        {
            points = value;
        }
    }
    private int categoryIndex;
    public int CategoryIndex
    {
        get
        {
            return categoryIndex;
        }

        set
        {
            categoryIndex = value;
        }
    }

    private int spriteIndex;
    public int SpriteIndex
    {
        get
        {
            return spriteIndex;
        }

        set
        {
            spriteIndex = value;
        }
    }
    private GameObject achievementReference;

    public Achievement(string name, string description, int points, int categoryIndex, int spriteIndex, GameObject achievementReference)
    {
        this.name = name;
        this.description = description;
        this.points = points;
        this.categoryIndex = categoryIndex;
        this.spriteIndex = spriteIndex;
        this.achievementReference = achievementReference;
        LoadAchievement();
    }



    public bool EarnAchievement()
    {
        if (!unlocked)
        {
            unlocked = true;
            achievementReference.GetComponent<Image>().color = AchievementManager.AchivementEarnedColor;
            return true;
        }
        return false;
    }
    public void SaveAchievement(bool value)
    {
        unlocked = true;
    }
    public void LoadAchievement()
    {
        if (unlocked)
        {
            achievementReference.GetComponent<Image>().color = AchievementManager.AchivementEarnedColor;

        }
    }
}
