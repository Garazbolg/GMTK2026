using UnityEngine;

public abstract class EnemyFireTickResponseBehaviour : MonoBehaviour
{
    public int tickBeforeFire = 3;
    private int remainingTicks;
    
    public TMPro.TextMeshProUGUI countdownText;

    private void Start()
    {
        remainingTicks = tickBeforeFire;
        UpdateCountdownText();
    }

    private void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = remainingTicks.ToString();
        }
    }

    public int GetRemainingTicks()
    {
        return remainingTicks;
    }

    public void OnFireTick()
    {
        remainingTicks--;
        if (remainingTicks <= 0)
        {
            Fire();
            remainingTicks = tickBeforeFire;
        }
        UpdateCountdownText();
    }
    
    protected abstract void Fire();
}