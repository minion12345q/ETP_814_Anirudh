using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interaction : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public float pushForce;
    public Movement Player;
    public float pushDirection;

    public GameObject VFX;
    private Transform spawnVFX;
    // Start is called before the first frame update
    void Awake()
    {
        playerRB = Player.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Player.spriteRenderer.flipX)
        {
            pushDirection = 1;
        }
        else 
        {
            pushDirection = -1;
        }

        spawnVFX = Player.transform;

        
    }
    public void InteractionExectuted()
    {
        Debug.Log("InteractionExectuted");
        playerRB.AddForce(new Vector2(pushForce*pushDirection, 0), ForceMode2D.Impulse);
        Instantiate(VFX, spawnVFX.position, Quaternion.identity);

    }
}
