using System;
using System.Configuration;
using System.Text.RegularExpressions;
using Utility.IO;
using Utility.Web;

namespace IPAddressNotifier
{
  class Program
  {
    public static void Main(string[] args)
    {
      const string cCheckIPURL = @"http://checkip.dyndns.com/";
      const string cIPRegexStr = @"\b(?:\d{1,3}\.){3}\d{1,3}\b";
      const string cSubjectStr = @"Server IP Address";
      const string cContentStr = @"Current IP Address:";
      
      string username = ConfigurationSettings.AppSettings["username"];
      string password = ConfigurationSettings.AppSettings["password"];
      string existingAddressFilePath = ConfigurationSettings.AppSettings["filepath"];
      string checkURL = ConfigurationSettings.AppSettings["checkurl"];
      if (string.IsNullOrEmpty(checkURL)) { checkURL = cCheckIPURL; }
      string ipRegexStr = ConfigurationSettings.AppSettings["ipregex"];
      if (string.IsNullOrEmpty(ipRegexStr)) { ipRegexStr = cIPRegexStr; }
      string toAddress = ConfigurationSettings.AppSettings["toaddress"];
      string subject = ConfigurationSettings.AppSettings["subject"];
      if (string.IsNullOrEmpty(subject)) { subject = cSubjectStr; }
      string content = ConfigurationSettings.AppSettings["content"];
      if (string.IsNullOrEmpty(content)) { content = cContentStr; }
      
      string currentIPAddress = ParseIP(HttpContent.GetQuickContent(checkURL), ipRegexStr);
      Console.WriteLine(currentIPAddress);
      
      // if username and password provided, send IP Address notification
      // if saving path provided, add duplication check before send notification 
      if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
      {
        string existingIPAddress = File.ReadContent(existingAddressFilePath).Trim();
        if (existingIPAddress != currentIPAddress)
        {
          // Save temp file for duplication check
          if(!string.IsNullOrEmpty(existingAddressFilePath)) 
          { 
            File.WriteContent(existingAddressFilePath, currentIPAddress, false); 
            Console.WriteLine(string.Format("Cache file saved under {0} with value {1}", existingAddressFilePath, currentIPAddress));
          }
          
          // Send email
          GMail mail = new GMail(username, password);
          if(string.IsNullOrEmpty(toAddress)) { mail.ToAddress = username; } else { mail.ToAddress = toAddress; }
          mail.Subject = subject;
          mail.Content = string.Format("{0}{1}{2}", content, System.Environment.NewLine, currentIPAddress);
          bool sendSuccess = mail.SendMail();
          if (sendSuccess) { Console.WriteLine(string.Format("Send email to {0}", mail.ToAddress)); }
        }
      }
    }
    
    private static string ParseIP(string rawData, string regexStr)
    {
        Regex r = new Regex(regexStr);
        Match m = r.Match(rawData);
        if(m.Success)
        {
          return m.Value.Trim();
        }
        return string.Empty;
    }
  }
}
