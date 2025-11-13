using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI timerText;
    public Image cooldownImage;

    private float remaingTime;
    private float duration;

    public void Initialize(Sprite iconSprite, float duration)
    {
        icon.sprite = iconSprite;
        this.duration = duration;
        remaingTime = duration;
        cooldownImage.fillAmount = 1.0f;
        timerText.text = $"{(int)remaingTime}s";
    }

    public bool UpdateTimer(float deltaTime)
    {
        remaingTime -= deltaTime;

        timerText.text = $"{(int)Mathf.Max(remaingTime, 0)}s";
        float ratio = Mathf.Clamp01(remaingTime / duration);
        cooldownImage.fillAmount = ratio;

        return remaingTime <= 0;
    }
}
