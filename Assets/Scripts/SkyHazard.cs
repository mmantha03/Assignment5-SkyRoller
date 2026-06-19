using UnityEngine;

public enum SkyHazardType
{
    Spikes,
    SlowPad,
    PushLeft,
    PushRight,
    SpeedPad
}

public class SkyHazard : MonoBehaviour
{
    public SkyHazardType hazardType;
    public float slowSpeed = 0.65f;
    public float slowControl = 0.75f;
    public float effectTime = 1.25f;
    public float pushSpeed = 4.5f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (hazardType == SkyHazardType.Spikes)
        {
            if (SurvivalGameManager.Instance != null)
            {
                SurvivalGameManager.Instance.LoseGame("Hit a danger pad!");
            }
        }
        else if (hazardType == SkyHazardType.SlowPad && player != null)
        {
            player.ApplySlow(
                Mathf.Max(slowSpeed, 0.65f),
                Mathf.Max(slowControl, 0.75f),
                Mathf.Min(effectTime, 1.5f)
            );
        }
        else if (hazardType == SkyHazardType.SpeedPad && player != null)
        {
            player.ApplySlow(1.2f, 0.9f, 1f);
        }
        else if (player != null)
        {
            float direction = hazardType == SkyHazardType.PushLeft ? -1f : 1f;
            player.ApplyPush(direction * Mathf.Clamp(pushSpeed, 0f, 4.5f), 0.35f);
        }
    }
}
