using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlowdown : MonoBehaviour
{
    public float slowdownMultiplier = 2f;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void UpdateSlowdownPhysics(int blocks_amount)
    {
        if (blocks_amount > 0 && gameManager.PhysicsRange > 0.1f)
        {
            //Slows down physics by 0.1f per block based on slowdownMultiplier being 1
            gameManager.PhysicsRange = 0.5f - (blocks_amount / 100f) * slowdownMultiplier;
        }
        Debug.Log(gameManager.PhysicsRange);
    }
}
