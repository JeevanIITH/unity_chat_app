using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMessage : MonoBehaviour
{
    public string inputMessage;
    public string temp_store_message;

    public InputField inputfield;
    public Button send_button;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 10;
        
    }

    

    public void send_button_function()
    {
        inputMessage = inputfield.text;
        Debug.Log(inputMessage);
        inputfield.text = "";
    }


    

    public void getinputmessage( string s  )
    {
        inputMessage = s;
        Debug.Log("output from getinput function"+inputMessage);
    }

    public void store_temp_message( string s)
    {
        temp_store_message = s;
        
        Debug.Log("output from store temp"+temp_store_message);

    }


}
