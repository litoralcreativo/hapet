using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerScript : MonoBehaviour
{
    public bool pointing;
    public bool pointingCI;
    public bool pointingDP;
    public bool ableToPoint;
    private Image img;
    public GameObject CI;
    public GameObject DP;

    public bool smallStep;

    public ElementPlacingScript eps;

    private Vector2 _cursPos;
    private bool listeningCursor = false;

    private void Start()
    {
        img = GetComponent<Image>();
    }
    private void Update()
    {
        if (eps.active)
            ableToPoint = true;
        else
            ableToPoint = false;


        if (ableToPoint)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _cursPos = Input.mousePosition;
                smallStep = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                smallStep = false;
            }

            if (Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.LeftControl))
            {
                pointing = true;
                pointingDP = true;
                pointingCI = false;
                if (Cursor.visible == true)
                {
                    Cursor.visible = false;
                }

                if (DP.activeSelf == false)
                {
                    DP.SetActive(true);
                    CI.SetActive(false);
                }
                /*
                if (listeningCursor)
                {
                    _cursPos = Input.mousePosition;
                    listeningCursor = false;
                }
                */
                if (!smallStep)
                {
                    Vector2 cursPos = Input.mousePosition;
                    transform.position = cursPos;
                } else
                {
                    Vector2 cursPos = new Vector2(_cursPos.x - (_cursPos.x - Input.mousePosition.x) / 5, _cursPos.y - (_cursPos.y - Input.mousePosition.y) / 5);
                    transform.position = cursPos;
                }
                    
                
            }
            else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftAlt))
            {
                pointing = true;
                pointingCI = true;
                pointingDP = false;
                if (Cursor.visible == true)
                {
                    Cursor.visible = false;
                }

                if (CI.activeSelf == false)
                {
                    CI.SetActive(true);
                    DP.SetActive(false);
                }

                if (!smallStep)
                {
                    Vector2 cursPos = Input.mousePosition;
                    transform.position = cursPos;
                }
                else
                {
                    Vector2 cursPos = new Vector2(_cursPos.x - (_cursPos.x - Input.mousePosition.x) / 5, _cursPos.y - (_cursPos.y - Input.mousePosition.y) / 5);
                    transform.position = cursPos;
                }
            }
            else
            {
                pointing = false;

                if (Cursor.visible == false)
                {
                    Cursor.visible = true;
                }
                if (DP.activeSelf == true || CI.activeSelf == true)
                {
                    CI.SetActive(false);
                    DP.SetActive(false);
                }
            }
        }
        else
        {
            pointing = false;
            
            if (Cursor.visible == false)
            {
                Cursor.visible = true;
                
            }
            if (DP.activeSelf == true || CI.activeSelf == true)
            {
                CI.SetActive(false);
                DP.SetActive(false);
            }
        }

        if (!pointing)
        {
            smallStep = false;
            listeningCursor = false;
        }


    }

}
