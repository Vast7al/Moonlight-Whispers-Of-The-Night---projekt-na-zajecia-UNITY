using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int melonCount = 0;
    [SerializeField] private Text melonText;
    [SerializeField] private AudioSource collectionSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Melon")) {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject);
            melonCount++;
            melonText.text = "Melons: " + melonCount; 
        }
    }
}