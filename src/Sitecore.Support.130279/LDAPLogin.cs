namespace Sitecore.Support
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.Web.Security;
  using System.Web.UI;
  using System.Web.UI.WebControls;

  using Sitecore;
  using Sitecore.Configuration;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;
  using Sitecore.Pipelines.LoggingIn;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Sitecore.Security.Domains;
  using Sitecore.Sites;
  using Sitecore.Text;
  using Sitecore.Web;

  public class LDAPLogin : Page
  {
    protected System.Web.UI.HtmlControls.HtmlForm form1;

    /// <summary>
    /// The is debug.
    /// </summary>
    private bool isDebug;

    /// <summary>
    /// The login error.
    /// </summary>
    private string loginError;

    /// <summary>
    /// The user to login.
    /// </summary>
    private string userToLogin;

    /// <summary>
    /// The page_ load.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(e, "e");
      this.loginError = string.Empty;
      this.isDebug = LightLDAP.Configurations.Settings.Debug
                     || (!string.IsNullOrEmpty(Request.QueryString["debug"]) && Request.QueryString["debug"] == "true");

      string logonUsername = Request.ServerVariables["LOGON_USER"];
      if (!string.IsNullOrEmpty(logonUsername))
      {
        string domainPrefix = logonUsername.Substring(0, logonUsername.IndexOf("\\"));
        string username = logonUsername.Substring(logonUsername.IndexOf("\\") + 1);

        this.userToLogin = this.GetValidUsername(domainPrefix, username);
        if (!string.IsNullOrEmpty(this.userToLogin))
        {
          if (this.isDebug)
          {
            this.RenderLoginInfo(this.GetFullMembership(this.userToLogin));
            Log.Info(string.Format("The user {0} has been logged in!", logonUsername), this);
          }

          this.DoLogin(this.userToLogin);
        }
      }
      else
      {
        this.loginError =
          "Error logging in using Single Sign-on: User information is not available <br><br>This error can happen either if you are not logged on to the Active Directory domain, or if anonymous access to this page has not been disabled in the Internet Information Server. Make sure you are logged in to the domain, and contact your system administrator if this problem persists.";
      }

      if (!string.IsNullOrEmpty(this.loginError))
      {
        var loginErrorLabel = new Label { Text = this.loginError, ForeColor = Color.Red };
        loginErrorLabel.Font.Bold = true;

        Form.Controls.Add(loginErrorLabel);
      }
    }

    /// <summary>
    /// Gets full user membership.
    /// </summary>
    /// <param name="userName">The user to login.</param>
    /// <returns>
    /// The list of user roles.
    /// </returns>
    [NotNull]
    private string[] GetFullMembership([NotNull] string userName)
    {
      Assert.ArgumentNotNull(userName, "userName");
      var roles = new List<string>();

      string[] directRoles = Roles.GetRolesForUser(userName);
      if (directRoles == null || directRoles.Length == 0)
      {
        return roles.ToArray();
      }

      roles.AddRange(directRoles);

      if (RolesInRolesManager.RolesInRolesSupported)
      {
        var indirectRoles =
          RolesInRolesManager.GetRolesForUser(Sitecore.Security.Accounts.User.FromName(userName, false), true) as
          RoleList;
        if (indirectRoles != null && indirectRoles.Count > 0)
        {
          foreach (Role role in indirectRoles)
          {
            if (!roles.Contains(role.Name))
            {
              roles.Add(string.Format("<{0}>", role.Name));
            }
          }
        }
      }

      return roles.ToArray();
    }

    /// <summary>
    /// The render login info.
    /// </summary>
    /// <param name="rolesForUser">The roles for user.</param>
    private void RenderLoginInfo([NotNull] string[] rolesForUser)
    {
      Assert.ArgumentNotNull(rolesForUser, "rolesForUser");
      var button = new Button { Text = @"Log me in" };
      button.Click += (sender, args) => this.DoLogin(this.userToLogin);

      Form.Controls.Add(button);

      var rolesList = new BulletedList();
      foreach (var role in rolesForUser)
      {
        var roleItem = new ListItem(role);
        rolesList.Items.Add(roleItem);
      }

      Form.Controls.Add(rolesList);
    }

    // TODO: [CMSCopyPaste]
    // Source: https://svn2dk1.dk.sitecore.net/svn/cms/Sitecore/tags/Version 6.6.0/CMS 6.6 RTM rev.120918/code/Sitecore.Client/sitecore/login/default.aspx.cs
    // Method: Login_LoggedIn

    /// <summary>
    /// Logs in user.
    /// </summary>
    /// <param name="username">The username.</param>
    private void DoLogin([NotNull] string username)
    {
      Assert.ArgumentNotNull(username, "username");

      var loggingInArgs = new LoggingInArgs
      {
        Username = username,
        StartUrl = string.Empty
      };
      using (new SiteContextSwitcher(Factory.GetSite("shell")))
      {
        Pipeline.Start("loggingin", loggingInArgs);
      }

      if (AuthenticationManager.Login(username, false, true))
      {
        var user = Sitecore.Security.Accounts.User.FromName(username, true);

        var url = this.GetStartUrl(user);
        var language = StringUtil.GetString(user.Profile.ClientLanguage, Settings.ClientLanguage);

        string startPage;
        if (!string.IsNullOrEmpty(loggingInArgs.StartUrl))
        {
          var startUrlString = new UrlString(loggingInArgs.StartUrl);
          startUrlString["sc_lang"] = language;
          startPage = startUrlString.ToString();
        }
        else
        {
          var startUrl = new UrlString(url);
          startUrl["sc_lang"] = language;

          var usersPageUrl = new UrlString(WebUtil.GetQueryString())
          {
            Path = "/sitecore/shell/Applications/Login/Users/Users.aspx"
          };

          usersPageUrl["su"] = startUrl.ToString();
          startPage = usersPageUrl.ToString();
        }

        Log.Info(string.Format("The user {0} was successfully logged in!", username), this);

        WebUtil.Redirect(startPage);
      }
      else
      {
        this.loginError = string.Format("Can't login a user '{0}'", username);
        Log.Warn(this.loginError, this);
      }
    }

    // TODO: [CMSCopyPaste]
    // Source: https://svn2dk1.dk.sitecore.net/svn/cms/Sitecore/tags/Version 6.6.0/CMS 6.6 RTM rev.120918/code/Sitecore.Client/sitecore/login/default.aspx.cs
    // Method: GetStartUrl

    /// <summary>
    /// Gets the user start url.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>
    /// The user start url.
    /// </returns>
    [NotNull]
    private string GetStartUrl([CanBeNull]User user)
    {
      var formValue = WebUtil.GetCookieValue("sitecore_starturl");
      if (user != null)
      {
        formValue = StringUtil.GetString(new[] { user.Profile.StartUrl, formValue });
      }

      return StringUtil.GetString(new[] { formValue, "/sitecore/shell/applications/clientusesoswindows.aspx" });
    }

    /// <summary>
    /// Checks whether the domain exists.
    /// </summary>
    /// <param name="domainName">The domain name.</param>
    /// <returns>
    /// <c>true</c> if domain exits; otherwise - <c>false</c>.
    /// </returns>
    private bool DomainExists([NotNull] string domainName)
    {
      Assert.ArgumentNotNull(domainName, "domainName");
      return Domain.GetDomain(domainName) != null;
    }

    /// <summary>
    /// Gets the valid user name.
    /// </summary>
    /// <param name="domainPrefix">The domain prefix.</param>
    /// <param name="username">The user name.</param>
    /// <returns>
    /// The user full name.
    /// </returns>
    [CanBeNull]
    private string GetValidUsername([NotNull] string domainPrefix, [NotNull] string username)
    {
      Assert.ArgumentNotNull(domainPrefix, "domainPrefix");
      Assert.ArgumentNotNull(username, "username");
      if (this.DomainExists(domainPrefix))
      {
        // if a valid domain is specified, try to find a user in this domain
        var fullName = string.Format(@"{0}\{1}", domainPrefix, username);
        var user = Membership.GetUser(fullName, false);
        if (user != null)
        {
          if (this.IsAllowedToLogin(fullName))
          {
            return fullName;
          }
        }
        else
        {
          this.loginError =
            string.Format(
              "Cannot log in: The system failed to find a user named '{0}' in the '{1}' domain",
              username,
              domainPrefix);
          Log.Warn(this.loginError, this);
        }
      }
      else
      {
        // if an invalid domain is specified, try to search the same user in one of the other domains
        var userExists = false;
        foreach (var domainName in Factory.GetDomainNames())
        {
          var proposedUsername = string.Format(@"{0}\{1}", domainName, username);
          var user = Membership.GetUser(proposedUsername, false);
          if (user != null)
          {
            if (this.IsAllowedToLogin(proposedUsername))
            {
              return proposedUsername;
            }

            userExists = true;
          }
        }

        if (!userExists)
        {
          this.loginError = string.Format(
            "Cannot log in: The system failed to find a user named '{0}' in any domain",
            username);
          Log.Warn(this.loginError, this);
        }
      }

      return null;
    }

    /// <summary>
    /// Checks whether the user is allowed to login.
    /// </summary>
    /// <param name="fullName">The full name.</param>
    /// <returns>
    /// <c>true</c> if user is allowed to login; otherwise - <c>false</c>
    /// </returns>
    private bool IsAllowedToLogin([NotNull] string fullName)
    {
      Assert.ArgumentNotNull(fullName, "fullName");
      var user = Sitecore.Security.Accounts.User.FromName(fullName, false);
      if (user.IsAdministrator)
      {
        this.loginError = string.Empty;
        return true;
      }

      if (Roles.IsUserInRole(fullName, Sitecore.Constants.SitecoreClientUsersRole))
      {
        this.loginError = string.Empty;
        return true;
      }

      if (RolesInRolesManager.RolesInRolesSupported
          && RolesInRolesManager.IsUserInRole(
            Sitecore.Security.Accounts.User.FromName(fullName, false),
            Role.FromName(Sitecore.Constants.SitecoreClientUsersRole),
            true))
      {
        this.loginError = string.Empty;
        return true;
      }

      this.loginError = string.Format(
        "Cannot log in: The user '{0}' is not a member of the '{1}' role",
        fullName,
        Sitecore.Constants.SitecoreClientUsersRole);
      Log.Warn(this.loginError, this);
      return false;
    }
  }
}