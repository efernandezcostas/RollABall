using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
private Rigidbody rb; 
private int count;
private float movementX;
private float movementY;

public float speed = 15f; 
public TextMeshProUGUI countText;
public GameObject winTextObject;


   void Start()
   {
      rb = GetComponent<Rigidbody>();
      count = 0; 
      SetCountText();
      winTextObject.SetActive(false);
   }
 
   void OnMove(InputValue movementValue)
   {
      Vector2 movementVector = movementValue.Get<Vector2>();
      movementX = movementVector.x; 
      movementY = movementVector.y; 
   }
   
   void SetCountText() 
   {
      countText.text =  "Count: " + count.ToString();
      if (count >= 10)
      {
         winTextObject.SetActive(true);
         Destroy(GameObject.FindGameObjectWithTag("Enemy"));
      }
   }

   private void FixedUpdate() 
   {
      Vector3 movement = new Vector3(movementX, 0.0f, movementY);
      rb.AddForce(movement * speed, ForceMode.Acceleration);
   }

   void OnTriggerEnter(Collider other) 
   {
      if (other.gameObject.CompareTag("PickUp")) 
      {
         other.gameObject.SetActive(false);
         count = count + 1;
         SetCountText();
      }
   }

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.CompareTag("Enemy"))
      {
         // Destroy the current object
         Destroy(gameObject); 
         // Update the winText to display "You Lose!"
         winTextObject.gameObject.SetActive(true);
         winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
      }
   }
}