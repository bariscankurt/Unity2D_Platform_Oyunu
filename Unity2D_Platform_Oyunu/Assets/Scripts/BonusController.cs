using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusController : MonoBehaviour
{
    
    [SerializeField] Text puanSayisi;
    private void Update()
    {
        transform.Rotate(new Vector3(0f, 3f, 0f));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int puan = int.Parse(puanSayisi.text);
            puan += 50;
            puanSayisi.text = puan.ToString();
            Destroy(gameObject);
        }
    }
}
