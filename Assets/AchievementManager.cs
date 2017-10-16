using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour {

    public GameObject achievementClone;
    public Sprite[] generalAchievementSprites;
    public Sprite[] otherAchievementSprites;
    public AchievementButton activeButton;
    public ScrollRect scrollRect;
    public GameObject achieveMentPanel;
    public GameObject PopUpAchievement;
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

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
            EarnAchievement("B");
        }
    }
    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            GameObject achievement = Instantiate(PopUpAchievement);
            SetAchievementInfo("EarnAchievementCanvas",achievement,title);
            StartCoroutine(HideAchievement(achievement));
        }
    }

    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    public void OpenAchievementMenu()
    {
        achieveMentPanel.SetActive(true);
    }

    public void InitAchievements()
    {
        CreateAchievement("General", 0, "A", "ABCD", 15, 0);
        CreateAchievement("Other", 1, "B", "ABCD", 10, 1);

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
