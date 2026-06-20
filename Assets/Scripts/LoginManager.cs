using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Text.RegularExpressions;
public class LoginManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject signUpPanel;
    public GameObject gameplayRoot;
    [Header("Login")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;
    [Header("Sign Up")]
    public TMP_InputField usernameInput;
    public TMP_InputField signUpEmailInput;
    public TMP_InputField signUpPasswordInput;
    public TMP_InputField confirmPasswordInput;
    [Header("Messages")]
    public TMP_Text errorText;
    private FirebaseAuth auth;
    void Start()
    {
        ShowMessage("Start() reached");
        try
        {
            ShowMessage("Checking Firebase...");
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync()
                .ContinueWithOnMainThread(task =>
                {
                    ShowMessage("Dependency callback reached");
                    if (task.IsFaulted)
                    {
                        ShowMessage("Dependency check failed:\n" + task.Exception);
                        return;
                    }
                    var status = task.Result;
                    ShowMessage("Status: " + status);
                    if (status == Firebase.DependencyStatus.Available)
                    {
                        auth = FirebaseAuth.DefaultInstance;
                        if (auth == null)
                            ShowMessage("Auth is NULL");
                        else
                            ShowMessage("Firebase Ready");
                    }
                    else
                    {
                        ShowMessage("Firebase Error: " + status);
                    }
                });
        }
        catch (System.Exception ex)
        {
            ShowMessage("START ERROR:\n" + ex);
        }
    }
    private void ShowMessage(string message)
    {
        Debug.Log(message);
        if (errorText != null)
            errorText.text = message;
    }
    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
    private string ValidateLogin()
    {
        if (string.IsNullOrWhiteSpace(loginEmailInput.text))
            return "Please enter your email.";
        if (!IsValidEmail(loginEmailInput.text.Trim()))
            return "Please enter a valid email address.";
        if (string.IsNullOrWhiteSpace(loginPasswordInput.text))
            return "Please enter your password.";
        return null;
    }
    private string ValidateSignUp()
    {
        if (string.IsNullOrWhiteSpace(usernameInput.text))
            return "Please enter a username.";
        if (usernameInput.text.Trim().Length < 3)
            return "Username must be at least 3 characters long.";
        if (string.IsNullOrWhiteSpace(signUpEmailInput.text))
            return "Please enter an email address.";
        if (!IsValidEmail(signUpEmailInput.text.Trim()))
            return "Please enter a valid email address.";
        if (string.IsNullOrWhiteSpace(signUpPasswordInput.text))
            return "Please enter a password.";
        if (signUpPasswordInput.text.Length < 6)
            return "Password must be at least 6 characters long.";
        if (string.IsNullOrWhiteSpace(confirmPasswordInput.text))
            return "Please confirm your password.";
        if (signUpPasswordInput.text != confirmPasswordInput.text)
            return "Passwords do not match.";
        return null;
    }
    public void Login()
    {
        if (errorText != null)
            errorText.text = "";
        string validationError = ValidateLogin();
        if (validationError != null)
        {
            ShowMessage(validationError);
            return;
        }
        if (auth == null)
        {
            ShowMessage("Firebase Auth is NULL");
            return;
        }
        ShowMessage("Attempting login...");
        auth.SignInWithEmailAndPasswordAsync(
            loginEmailInput.text.Trim(),
            loginPasswordInput.text
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                ShowMessage("LOGIN CANCELED");
                return;
            }
            if (task.IsFaulted)
            {
                ShowMessage("LOGIN ERROR:\n" + task.Exception);
                return;
            }
            FirebaseUser user = task.Result.User;
            if (errorText != null)
                errorText.text = "";
            loginPanel.SetActive(false);
            signUpPanel.SetActive(false);
            gameplayRoot.SetActive(true);
            Debug.Log("Login Successful: " + user.Email);
        });
    }
    public void OpenSignUp()
    {
        if (errorText != null)
            errorText.text = "";
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }
    public void BackToLogin()
    {
        if (errorText != null)
            errorText.text = "";
        signUpPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
    public void CreateAccount()
    {
        if (errorText != null)
            errorText.text = "";
        string validationError = ValidateSignUp();
        if (validationError != null)
        {
            ShowMessage(validationError);
            return;
        }
        if (auth == null)
        {
            ShowMessage("Firebase Auth is NULL");
            return;
        }
        ShowMessage("Attempting account creation...");
        auth.CreateUserWithEmailAndPasswordAsync(
            signUpEmailInput.text.Trim(),
            signUpPasswordInput.text
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                ShowMessage("SIGNUP CANCELED");
                return;
            }
            if (task.IsFaulted)
            {
                ShowMessage("SIGNUP ERROR:\n" + task.Exception);
                return;
            }
            FirebaseUser user = task.Result.User;
            ShowMessage("Account created successfully.");
            Debug.Log("Account Created: " + user.Email);
            signUpPanel.SetActive(false);
            loginPanel.SetActive(true);
        });
    }
}