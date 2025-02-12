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
   private bool isGrounded;

   public float speed = 15f; 
   public float jumpForce = 10f; // Fuerza del salto
   public TextMeshProUGUI countText;
   public GameObject winTextObject;
   public float rampBoostMultiplier = 5f; // Multiplicador de velocidad al tocar la cuesta

   private enum PlayerState { moving, jumping, invulnerable }
   private PlayerState currentState;

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

   void OnJump(InputValue jumpValue)
   {
      if (isGrounded)
      {
         rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
         isGrounded = false;
      }
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
      rb.AddForce(movement * speed);
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
      else if (collision.gameObject.CompareTag("Ramp"))
      {
            // Aplica un impulso adicional al jugador al tocar la cuesta
            Vector3 boost = new Vector3(movementX, 0.0f, movementY) * rampBoostMultiplier;
            rb.AddForce(boost, ForceMode.Impulse);
      }
      else if (collision.gameObject.CompareTag("Ground"))
      {
         isGrounded = true;
      }
   }
}