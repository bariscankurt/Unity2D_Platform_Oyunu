using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    private Vector2 screenBounds;
    private float mySpeed;
    private Rigidbody2D myBody;
    private Animator myAnimator;
    private Vector3 defaultLocalScale;
    private Vector3 defaultArrowLocalScale;
    [SerializeField] float jumpPower;
    [SerializeField] float speed;
    public bool onGround;

    private bool canDoubleJump;
    private int puan;
    [SerializeField] float currentAttackTimer;
    [SerializeField] float defaultAttackTimer;
    
    [SerializeField] bool attacked;
    [SerializeField] GameObject arrow;
    [SerializeField] int arrowNumber;
    [SerializeField] Text okSayisiDegeri;
    [SerializeField] AudioClip olumSesi;

    [SerializeField] GameObject winPanel, losePanel;
    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Time.timeScale = 1;
        attacked = false;
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        defaultLocalScale = transform.localScale;
        okSayisiDegeri.text = arrowNumber.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        mySpeed = Input.GetAxis("Horizontal");
        myBody.velocity = new Vector2(mySpeed * speed, myBody.velocity.y);
        Debug.Log("Frame sayisi:");
        myAnimator.SetFloat("Speed", Mathf.Abs(mySpeed));
        
        #region playerin sağ ve sol hareket yönüne göre yüzünü dönmesi ve zıplaması
        if (mySpeed > 0)
        {
            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        else if (mySpeed < 0)
        {
            transform.localScale = new Vector3(-defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        //playerın zıplamasının kontrolü
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround == true)
            {
                
                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                canDoubleJump = true;
                myAnimator.SetTrigger("Jump");
            }
            else if (canDoubleJump == true)
            {
                
                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                myAnimator.SetTrigger("Jump");
                canDoubleJump = false;
            }

        }
        #endregion

        #region playerın ok atmasının kontrolü

        if (Input.GetMouseButtonDown(0) && arrowNumber > 0)
        {
            if (attacked == false)
            {
                attacked = true;
                myAnimator.SetTrigger("Attack");
                Invoke("Fire", 0.5f);
                
                
            }
            

        }
        if (attacked == true)
        {
            currentAttackTimer -= Time.deltaTime;
            
        }
        else
        {
            currentAttackTimer = defaultAttackTimer;
        }

        if (currentAttackTimer <= 0)
        {
            attacked = false;
        }

        

        #endregion

    }
    
    void Fire()
    {
        GameObject okumuz = Instantiate(arrow, transform.position, Quaternion.identity);
        okumuz.transform.parent = GameObject.Find("Arrows").transform;

        if (transform.localScale.x > 0)
        {
            okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);

        }
        else
        {
            Vector3 okumuzScale = okumuz.transform.localScale;
            okumuz.transform.localScale = new Vector3(-okumuzScale.x, okumuzScale.y, okumuzScale.z);
            okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);
        }
        arrowNumber--;
        okSayisiDegeri.text = arrowNumber.ToString();
    }
    //playerımızın çarpışma kontrolleri
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Diken"))
        {
            GetComponent<TimeControl>().enabled = false;
            Die();
        }
        else if (collision.gameObject.CompareTag("Bitir"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(Wait(true));
            /*winPanel.active = true;
            Time.timeScale = 0;*/
        }
        
    }

    //playerımızın ölüm kontrolleri
    
    public void Die()
    {
        GameObject.Find("SoundController").GetComponent<AudioSource>().clip = null;
        GameObject.Find("SoundController").GetComponent<AudioSource>().PlayOneShot(olumSesi);
        myAnimator.SetTrigger("Die");
        myAnimator.SetFloat("Speed", 0);
        //myBody.constraints = RigidbodyConstraints2D.FreezePosition;
        myBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = false;
        StartCoroutine(Wait(false));
    }

    IEnumerator Wait(bool win)
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0;
        if(win == true)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
        
    }
}
