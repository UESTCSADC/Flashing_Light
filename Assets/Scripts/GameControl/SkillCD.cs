using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillCD : MonoBehaviour {
    public float coldTime = 5;//技能的冷却时间  
    private float currentTime = 0;//当前冷却时间  

    public Image Mask;
    public Button skillBtn;
    private static SkillCD instance;
    public static SkillCD Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }
    public bool canUse = false;
    void Awake()
    {
        if (instance == null)
            instance = this;
        Mask.fillAmount = 0;
        skillBtn.onClick.RemoveAllListeners();
        skillBtn.onClick.AddListener(UseSkill);
    }
    public void SetSkill(Sprite skillSprite)
    {
        this.GetComponent<Image>().sprite = skillSprite;

    }
    void Update()
    {
        SkillTimeCalculator();
    }

    public void UseSkill()
    {
        currentTime = coldTime;
        Mask.fillAmount = 1;
        skillBtn.enabled = false;
        canUse = false;
    }

    private void SkillTimeCalculator()
    {
        if (currentTime <= 0)
        {
            Mask.fillAmount = 0;
            canUse = true;
            skillBtn.enabled = true;
            return;
        }
        else
        {
            currentTime -= Time.deltaTime;
            var value = currentTime / coldTime;
            Mask.fillAmount = value;
        }
    }
}
