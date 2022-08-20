using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Account_script : MonoBehaviour
{

    public static string Name;

    public  InputField enter_name_field;

    public  Button continue_button;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 10;
    }

    public void continue_button_fuction()
    {
        Name = enter_name_field.text;

        // write code to load next scene Great job !

    }
}
