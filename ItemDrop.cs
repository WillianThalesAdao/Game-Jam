using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

//esse script vc tem que add ele nos itens que vão ser dropados para no jogador
public class ItemDrop : MonoBehaviour
{
    public ItemEffect effect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.SetItemEffect(effect);
            Destroy(gameObject);
        }
    }
}
