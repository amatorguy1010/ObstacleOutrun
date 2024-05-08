using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuth : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, profilePanel, forgetPasswordPanel,notificationPanel;

    public TMP_InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUserName, forgetPassEmail;

    public Text notif_Title_Text, notif_Message_Text,profileUserName_Text,profileUserEmail_Text;

    public Toggle rememberMe;

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);

    }
    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);


    }
    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);


    }

    public void OpenForgetPassPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);


    }

    public void LoginUser()
    {
        if(string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
        {
            showNotificationMessage("Error", "Fields Are Empty");
            return;
        }

        // Do Login
    }

    public void SignUpUser()
    {
        if(string.IsNullOrEmpty(signupEmail.text) &&  string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPassword.text) && string.IsNullOrEmpty(signupUserName.text))
        {
            showNotificationMessage("Error", "Fields Are Empty");
            return;
        }

        // Do SignUp
    }

    public void forgetPass()
    {
        if (string.IsNullOrEmpty(forgetPassEmail.text))
        {
            showNotificationMessage("Error", "Forget Email Empty");
            return;
        }
    }

    private void showNotificationMessage(string title, string message)
    {
        notif_Title_Text.text ="" + title;
        notif_Message_Text.text = "" + message;
        notificationPanel.SetActive(true);
    }

    public void CloseNotif_Panel()
    {
        notif_Title_Text.text = "" ;
        notif_Message_Text.text = "";
        notificationPanel.SetActive(false);
    }

    public void LogOut()
    {
        profilePanel.SetActive(false);
        profileUserEmail_Text.text = "";
        profileUserName_Text.text = "";
        OpenLoginPanel();
    }
}
