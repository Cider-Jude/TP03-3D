using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private Vector3 healthBarInitialScale;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar == null) return;

        healthBar.transform.localScale = new Vector3(
            currentHealth/maxHealth,
            healthBar.transform.localScale.y,
            healthBar.transform.localScale.x
            );
    }

    void Die()
    {
        Debug.Log("Le joueur est mort");
    }
}