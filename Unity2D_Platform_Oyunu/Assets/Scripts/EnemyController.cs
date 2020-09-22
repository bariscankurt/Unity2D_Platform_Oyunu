using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] bool onGround;
    private float width;
    private Rigidbody2D myBody;
    [SerializeField] LayerMask engel;
    [SerializeField] float speed;
    private static int totalEnemyNumber;
    // Start is called before the first frame update
    void Start()
    {
        totalEnemyNumber++;
        width = GetComponent<SpriteRenderer>().bounds.extents.x;
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //objenin sınırının altındaki zeminden çıkıp çıkmadığını tespit etme
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right * width/2), Vector2.down, 2f,engel);
        if (hit.collider != null)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        Dondurme();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 playerRealPosition = transform.position + (transform.right * width / 2);
        Gizmos.DrawLine(playerRealPosition, playerRealPosition + new Vector3(0,-2f,0));
    }
    void Dondurme()
    {
        if (onGround == false)
        {
            //objeyi döndürme
            transform.eulerAngles += new Vector3(0, 180f, 0);
        }
        myBody.velocity = new Vector2(transform.right.x * speed, 0f);
    }
}
