using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement {

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
    }



    public bool EarnAchievement()
    {
        if (!unlocked)
        {
            unlocked = true;
            return true;
        }
        return false;
    }

}
