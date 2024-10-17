using System;
using System.Text;

public static class PasswordUtility
{
    public static string DecodeBase64Password(string base64EncodedPassword)
    {
        byte[] passwordBytes = Convert.FromBase64String(base64EncodedPassword);
        string clearTextPassword = Encoding.UTF8.GetString(passwordBytes);
        return clearTextPassword;
    }
}