IP Address Notifier
===================

Tiny console app to get current machine's external IP address.
* Support Email notification
* Ideal for auto-notification in non-static IP environment

Setup & Installation
====================

1. **Simple Mode**
  
  This mode will just display current machine's external IP address.

  No need to modify app.config. Just run it. 

2. **Email Notification Mode**

  This mode will display IP address and send an email notification to specified address as well. (If 'to address' has not been specified, it will use provided username address as 'to address'.)
  
  Modify app.config with your Gmail account setting:
  
  ```
  <add key="username" value="#YOURUSERNAME#" />
  <add key="password" value="#YOURPASSWORD#" />
  <add key="toaddress" value="#EMAILYOUWANTTORECEIVETHEIPADDRESS#" />
  ```

3. **Auto-Email Notification Mode**

  This mode will run automatically to get IP address and send notification email. It also has duplication check which prevent you from receiving same email all day.
  
  Modify app.config with your Gmail account setting:
  
  ```
  <add key="username" value="#YOURUSERNAME#" />
  <add key="password" value="#YOURPASSWORD#" />
  <add key="toaddress" value="#EMAILYOUWANTTORECEIVETHEIPADDRESS#" />
  <add key="filepath" value="#TEMPFILEPATHTOSTOREEXISTINGIPADDRESS#" />
  ```

  Setup a windows task scheduler to run the program. i.e. Every 15 mins. 
  
Important Notes
===============

* **Reference**

  Make sure add following <a href="https://github.com/leonyuzhao/Utility-CodeSnippet-" target="_blank">references</a> into your project before compile.
  
  ```
  using Utility.IO;
  using Utility.Web;
  ```

