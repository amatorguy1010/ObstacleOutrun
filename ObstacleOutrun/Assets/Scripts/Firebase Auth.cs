using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
//using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class FirebaseAuth : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, profilePanel, forgetPasswordPanel,notificationPanel;

    public TMP_InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUserName, forgetPassEmail;

    public TMP_Text notif_Title_Text, notif_Message_Text,profileUserName_Text,profileUserEmail_Text;

    public Toggle rememberMe;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    bool IsSignIn = false;

    private const string adminEmail = "admin123@yahoo.com";
    private const string adminPassword = "admin123";

    public GameObject MainAdminPage, AdminView;
    DatabaseReference databaseReference;


    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://obstacleoutrun-default-rtdb.firebaseio.com/");

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        MainAdminPage.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
        AdminView.SetActive(false);

    }
    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        MainAdminPage.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
        AdminView.SetActive(false);


    }
    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        MainAdminPage.SetActive(false);
        forgetPasswordPanel.SetActive(false);
        AdminView.SetActive(false);


    }

    public void OpenMainAdminPage()
    {
        if (user != null && user.Email == adminEmail)
        {
            loginPanel.SetActive(false);
            signupPanel.SetActive(false);
            profilePanel.SetActive(false);
            MainAdminPage.SetActive(true);
            AdminView.SetActive(false);
            forgetPasswordPanel.SetActive(false);

        }
        else
        {
            Debug.LogError("Only admins can access the main admin page.");
        }
    }


    public void OpenForgetPassPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        MainAdminPage.SetActive(false);
        forgetPasswordPanel.SetActive(true);


    }

    public void LoginUser()
    {
        if(string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
        {
            showNotificationMessage("Error", "Fields Are Empty");
            return;
        }

        SignInUser(loginEmail.text, loginPassword.text);

        // Do Login
        //OpenProfilePanel();

       
    }

    public void SignUpUser()
    {
        if(string.IsNullOrEmpty(signupEmail.text) &&  string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPassword.text) && string.IsNullOrEmpty(signupUserName.text))
        {
            showNotificationMessage("Error", "Fields Are Empty");
            return;
        }

        CreateUser(signupEmail.text, signupPassword.text ,signupUserName.text);

        // Do SignUp
    }

    public void forgetPass()
    {
        if (string.IsNullOrEmpty(forgetPassEmail.text))
        {
            showNotificationMessage("Error", "Forget Email Empty");
            return;
        }
        forgetPasswordSubmit(forgetPassEmail.text);
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
        auth.SignOut();
        OpenLoginPanel();
        profileUserEmail_Text.text = "";
        profileUserName_Text.text = "";
        MainAdminPage.SetActive(false);

    }

    void CreateUser( string email , string password ,string Username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        showNotificationMessage("Error", GetErrorMessage(errorCode));
                    }
                }
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            UpdateUserProfile(Username);

            AddUserToDatabase(result.User.UserId, email, Username);
        });
    }

    public void SignInUser(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("SignInUser: Email or password is empty.");
            showNotificationMessage("Error", "Email or password is empty");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                showNotificationMessage("Error", "Login canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                if (task.Exception.InnerException != null)
                {
                    Debug.LogError("Inner Exception: " + task.Exception.InnerException.Message);
                }
                showNotificationMessage("Error", "Failed to log in: " + task.Exception.GetBaseException().Message);
                

                /*foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        showNotificationMessage("Error", GetErrorMessage(errorCode));
                    }
                }*/

                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            if (email == adminEmail)
            {
                OpenMainAdminPage();
            }
            else
            {
                profileUserName_Text.text = "" + result.User.DisplayName;
                profileUserEmail_Text.text = "" + result.User.Email;
                OpenProfilePanel();
            }
        });
    }
    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "Account Not Exist";
                break;
            case AuthError.MissingPassword:
                message = "Missing Password";
                break;
            case AuthError.WeakPassword:
                message = "Password so Weak";
                break;
            case AuthError.WrongPassword:
                message = "Wrong Password";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Your Email Already in Use";
                break;
            case AuthError.InvalidEmail:
                message = "Your Email is Invalid";
                break;
            case AuthError.MissingEmail:
                message = "Email Missing";
                break;
            default:
                message = "Invalid Error";
                break;
        }
        return message;
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                IsSignIn = true;
            }
        }
    }




    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    void UpdateUserProfile(string UserName)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = UserName,
                PhotoUrl = new System.Uri("https://dummyimage.com/600x400/000/fff&text=sfsfs"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");

                showNotificationMessage("Alert", "Account Successfully Created");
            });
        }
    }
    bool IsSigned = false;

    void Update()
    {
        if (IsSignIn && !IsSigned)
        {
            if (user != null && profileUserName_Text != null && profileUserEmail_Text != null)
            {
                IsSigned = true;
                profileUserName_Text.text = "" + user.DisplayName;
                profileUserEmail_Text.text = "" + user.Email; 
                OpenProfilePanel();
            }
           // else
            {
                //Debug.LogError("Update: One or more required fields are null.");
            }
        }
    }

    void forgetPasswordSubmit(string forgetPasswordEmail)
    {
        auth.SendPasswordResetEmailAsync(forgetPasswordEmail).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
            }
            if (task.IsFaulted)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        showNotificationMessage("Error", GetErrorMessage(errorCode));
                    }
                }
            }

            showNotificationMessage("Alert", "Successfully Send Email for Reset Password.");
        });
    }



    public void usersPage()
    {
        if (user != null && user.Email == adminEmail)
        {
            MainAdminPage.SetActive(false);
            AdminView.SetActive(true);
            // Aici poți adăuga cod pentru a încărca și afișa utilizatorii pe panoul admin, folosind prefab-ul AdminView sau orice alte componente UI ai definit
        }
        else
        {
            Debug.LogError("Only admins can access the users page.");
        }
    }


    void AddUserToDatabase(string userId, string userEmail, string userName)
    {
        // Creează un nou obiect pentru datele utilizatorului
        Dictionary<string, object> user = new Dictionary<string, object>
    {
        { "email", userEmail },
        { "username", userName }
    };

        // Adaugă datele utilizatorului în baza de date la calea corespunzătoare (de exemplu, "users/{userId}")
        databaseReference.Child("users").Child(userId).SetValueAsync(user).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("AddUserToDatabase encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("User data added to database successfully.");
        });
    }

}
