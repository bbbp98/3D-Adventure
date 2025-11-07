using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private float curValue;
    [SerializeField] private float startValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float passiveValue;

    [Header("UI")]
    public Image uiBar;
    public float duration = 0.4f;

    #region Event
    public Action onValueChanged;
    #endregion

    private void Awake()
    {
        curValue = startValue;
        onValueChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        onValueChanged -= UpdateUI;
    }

    #region Handle Value
    private float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Increase(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
        onValueChanged?.Invoke();
    }

    public void Decrease(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0);
        onValueChanged?.Invoke();
    }
    #endregion

    #region UI
    public void UpdateUI()
    {
        StartCoroutine(AnimateUIBar(GetPercentage()));
    }

    private IEnumerator AnimateUIBar(float targetValue)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            uiBar.fillAmount = Mathf.Lerp(uiBar.fillAmount, targetValue, t);
        yield return null;
        }

        uiBar.fillAmount = targetValue;
    }
    #endregion
}