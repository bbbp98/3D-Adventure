using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    public Condition Health { get { return uiCondition.Health; } }

    [Header("Stamina")]
    [SerializeField] float staminaForRun;
    public Condition Stamina { get { return uiCondition.Stamina; } }

    private void Update()
    {
        if (CharacterManager.Instance.Player.isRun)
        {
            UseStamina(staminaForRun * Time.deltaTime);
        }

        Heal(Health.GetPassiveValue() * Time.deltaTime);    // natural recovery
        Stamina.Increase(Stamina.GetPassiveValue() * Time.deltaTime);
    }

    public void Heal(float amount)
    {
        Health.Increase(amount);
    }

    public void UseStamina(float amount)
    {
        Stamina.Decrease(amount);
    }

    public void RecoveryStamina(float amount)
    {
        Stamina.Increase(amount);
    }

    public void Die()
    {
        Debug.Log("die");
    }

    public void TakeDamage(int damage)
    {
        Health.Decrease(damage);
    }
}
