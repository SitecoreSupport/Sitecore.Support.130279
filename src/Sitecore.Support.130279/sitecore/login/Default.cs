using System;
using System.Reflection;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel.Cryptography;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.Authentication;

namespace Sitecore.Support.sitecore.login
{
    public class Default : Sitecore.sitecore.login.Default
    {
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            }
            catch (PlatformNotSupportedException exception)
            {
                Log.Error("Setting response headers is not supported.", exception, this);
            }
            if (Sitecore.Context.User.IsAuthenticated)
            {
                if (WebUtil.GetQueryString("inv") == "1")
                {
                    Boost.Invalidate();
                }

                #region Custom code
                Uri urlReferrer = base.Request.UrlReferrer;
                if (urlReferrer != null)
                {
                    if (!urlReferrer.AbsolutePath.ToLower().Contains("/sitecore/client/applications/licenseoptions") && !DomainAccessGuard.GetAccess())
                    {
                        this.LogMaxEditorsExceeded();
                        base.Response.Redirect(WebUtil.GetFullUrl("/sitecore/client/Applications/LicenseOptions/StartPage"));
                        return;
                    }
                }
                #endregion Custom code

                else if (!DomainAccessGuard.GetAccess())
                {
                    this.LogMaxEditorsExceeded();
                    base.Response.Redirect(WebUtil.GetFullUrl("/sitecore/client/Applications/LicenseOptions/StartPage"));
                    return;
                }
            }
            this.DataBind();
            if (Sitecore.Configuration.Settings.Login.DisableRememberMe)
            {
                this.LoginForm.Attributes.Add("autocomplete", "off");
            }
            if (!base.IsPostBack && Sitecore.Configuration.Settings.Login.RememberLastLoggedInUserName)
            {
                string cookieValue = WebUtil.GetCookieValue(WebUtil.GetLoginCookieName());
                if (!string.IsNullOrEmpty(cookieValue))
                {
                    MachineKeyEncryption.TryDecode(cookieValue, out cookieValue);
                    this.UserName.Text = cookieValue;
                    this.UserNameForgot.Text = cookieValue;
                }
            }
            try
            {
                base.Response.Headers.Add("SC-Login", "true");
            }
            catch (PlatformNotSupportedException exception2)
            {
                Log.Error("Setting response headers is not supported.", exception2, this);
            }
            this.RenderSdnInfoPage();
            base.OnInit(e);
        }
        private void LogMaxEditorsExceeded()
        {
            string format = "The maximum number of simultaneously active (logged-in) editors exceeded. The User {0} cannot be logged in to the system. The maximum of editors allowed by license is {1}.";

            var fullUserName = typeof(Sitecore.sitecore.login.Default).GetField("fullUserName",BindingFlags.GetField|BindingFlags.NonPublic|BindingFlags.Instance).GetValue(this);

            Log.Warn(string.Format(format, fullUserName, DomainAccessGuard.MaximumSessions), this);
        }

        private void RenderSdnInfoPage()
        {
            string sitecoreUrl = Settings.Login.SitecoreUrl;
            if (base.Request.IsSecureConnection)
            {
                sitecoreUrl = sitecoreUrl.Replace("http:", "https:");
            }
            UrlString str2 = new UrlString(sitecoreUrl);
            str2["id"] = Sitecore.SecurityModel.License.License.LicenseID;
            str2["host"] = WebUtil.GetHostName();
            str2["licensee"] = Sitecore.SecurityModel.License.License.Licensee;
            str2["iisname"] = WebUtil.GetIISName();
            str2["st"] = WebUtil.GetCookieValue("sitecore_starttab", string.Empty);
            str2["sc_lang"] = Sitecore.Context.Language.Name;
            str2["v"] = About.GetVersionNumber(true);
            this.StartPage.Attributes["src"] = str2.ToString();
            this.StartPage.Attributes["onload"] = "javascript:this.style.display='block'";
        }


    }

}
