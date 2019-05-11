#pragma warning disable 0649 //Disable warnings for private serialized fields
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Core;

public class BtnLogin : MonoBehaviour
{

    [SerializeField]
    private InputField username;
    [SerializeField]
    private InputField password;

    public void LoginAccount()
    {
       
        try
        {
            ClientTCP.PACKAGE_Login(username.text, password.text);
        }
        catch (Exception)
        {
            Debug.Log("Trying to initialize connection");
            try
            {
                ClientTCP.clientManager.InitializeClientManager(); //Try to reinitialize ClientManager Again
                ClientTCP.PACKAGE_Login(username.text, password.text);
            }
            catch (Exception) //In case if there is no Connection and client Manager was not initialized
            {
                Debug.Log("No connection to the server");
            }
        }
       
    }
}
