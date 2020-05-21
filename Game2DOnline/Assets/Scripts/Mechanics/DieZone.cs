using GameOnline.Mechanics;
using UnityEngine;

public class DieZone : MonoBehaviour
{
    protected PlayerController player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            int dame = player.maxHealth;
            player.playerTakeDamage(dame);
        }
    }
}
