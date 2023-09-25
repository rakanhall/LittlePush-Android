using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PowerUpUIHandler[] powerUpHandlers; // Assign all your PowerUpUIHandlers here in the inspector.

    public void Start()
    {
        foreach (PowerUpUIHandler puih in powerUpHandlers)
        {
            puih.UpdateUI();
        }
    }
}


