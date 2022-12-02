using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    public Slider m_Slider;
    public float speed = 5;
    
    private CharacterController characterController;
    private Vector3 movec = Vector3.zero;
    private float gravity = -20;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!characterController.isGrounded)
        {
            movec.y += gravity * Time.deltaTime;
        }

        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            movec.x = (float)(-speed - m_Slider.value * 0.02);
            movec.z = 0;
        }
        else
        {
            movec.z = (float)(speed + m_Slider.value * 0.02);
            movec.x = 0;
        }

        characterController.Move(movec * Time.deltaTime);
    }
}
