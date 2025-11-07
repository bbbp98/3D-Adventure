using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    private Condition Health { get { return uiCondition.Health; } }

    private void Update()
    {
        Heal(Health.GetPassiveValue() * Time.deltaTime);    // natural recovery
    }

    public void Heal(float amount)
    {
        Health.Increase(amount);
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
