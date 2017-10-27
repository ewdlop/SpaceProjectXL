using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour {

    public GameObject achievementClone;
    public Sprite[] generalAchievementSprites;
    public Sprite[] bossAchievementSprites;
    public Sprite[] powerUpAchievementSprites;
    public Sprite[] shipAchievementSprites;
    public Sprite[] weaponAchievementSprites;
    public Sprite[] otherAchievementSprites;
    public AchievementButton activeButton;
    public ScrollRect scrollRect;
    public GameObject achieveMentPanel;
    public GameObject achievementPopUp;
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();
    public Text achievementScoreText;
    private static Color achivementEarnedColor = new Color(0f,255f,12f);

    public static Color AchivementEarnedColor
    {
        get
        {
            return achivementEarnedColor;
        }
    }

    void Start() {

        InitAchievements();
        GameObject generalCategory = GameObject.Find("GeneralCategory");
        generalCategory.GetComponent<Button>().Select();
        activeButton = generalCategory.GetComponent<AchievementButton>();

        foreach (GameObject achievementList in GameObject.FindGameObjectsWithTag("AchievementList"))
        {
            achievementList.SetActive(false);
        }

        activeButton.Click();
        achieveMentPanel.SetActive(false);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.I))
        {
            achieveMentPanel.SetActive(!achieveMentPanel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            EarnAchievement("A");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            EarnAchievement("B");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            EarnAchievement("C");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            EarnAchievement("D");
        }
    }
    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            GameObject achievement = Instantiate(achievementPopUp);
            achievementPopUp.GetComponent<Image>().color = achivementEarnedColor;
            SetAchievementInfo("EarnAchievementCanvas",achievement,title);
            StartCoroutine(HideAchievement(achievement));
        }
    }
    public IEnumerator HideAchievement(GameObject achievementPopUp)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievementPopUp);
    }

    public void InitAchievements()
    {
        CreateAchievement("General", 0, "A", "Press Z", 15, 0);
        CreateAchievement("General", 0, "B", "Press X", 15, 1);
        CreateAchievement("Boss", 1, "C", "Press C", 15, 0);
        CreateAchievement("PowerUp", 2, "D", "Press C", 15, 0);
        CreateAchievement("Ship", 3, "E", "Press C", 15, 0);
        CreateAchievement("Weapon", 4, "F", "Press C", 15, 0);
        CreateAchievement("Other", 5, "G", "Press D", 10, 0);

    }
    public void CreateAchievement(string category, int categoryIndex, string title, string description ,int points, int spriteIndex)
    {
        GameObject achievement = Instantiate(achievementClone);
        Achievement newAchievement = new Achievement(name, description,points,categoryIndex,spriteIndex,achievement);
        achievements.Add(title,newAchievement);
        SetAchievementInfo(category, achievement, title);
    }
    public void SetAchievementInfo(string category, GameObject achievement, string title)
    {
        achievement.transform.SetParent(GameObject.Find(category).transform);
        achievement.transform.localScale = new Vector3(1f, 1f, 1f);
        achievement.transform.GetChild(3).GetComponent<Text>().text = title;
        achievement.transform.GetChild(4).GetComponent<Text>().text = achievements[title].Description;
        achievement.transform.GetChild(5).GetComponent<Text>().text = achievements[title].Points.ToString();
        switch (achievements[title].CategoryIndex)
        {
            case 0:
                achievement.transform.GetChild(2).GetComponent<Image>().sprite = generalAchievementSprites[achievements[title].SpriteIndex];
                break;
            case 1:
                achievement.transform.GetChild(2).GetComponent<Image>().sprite = bossAchievementSprites[achievements[title].SpriteIndex];
                break;
            case 2:
                achievement.transform.GetChild(2).GetComponent<Image>().sprite = powerUpAchievementSprites[achievements[title].SpriteIndex];
                break;
            case 3:
                achievement.transform.GetChild(2).GetComponent<Image>().sprite = shipAchievementSprites[achievements[title].SpriteIndex];
                break;
            case 4:
                achievement.transform.GetChild(2).GetComponent<Image>().sprite = weaponAchievementSprites[achievements[title].SpriteIndex];
                break;
            case 5:
                achievement.transform.GetChild(2).GetComponent<Image>().sprite = otherAchievementSprites[achievements[title].SpriteIndex];
                break;
            default:
                break;
        }
    }
    public void ChangeCategory(GameObject button)
    {
        AchievementButton achievementButton = button.GetComponent<AchievementButton>();
        scrollRect.content = achievementButton.achievementList.GetComponent<RectTransform>();
        achievementButton.Click();
        activeButton.Click();
        activeButton = achievementButton;
    }
}
