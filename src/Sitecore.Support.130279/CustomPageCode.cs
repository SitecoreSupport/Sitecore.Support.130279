namespace Sitecore.Support
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Sitecore;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.Extensions.StringExtensions;
    using Sitecore.IO;
    using Sitecore.Mvc;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Reflection;
    using Sitecore.Shell;
    using Sitecore.Web.PageCodes;
    using Sitecore.Web.UI.Controls.Common.UserControls;
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;

    [PageVirtualPath("~/sitecore/shell/client/Speak/Layouts/Renderings/Common/PageCodes/CustomPageCode.cshtml"), GeneratedCode("RazorGenerator", "2.0.0.0")]
    internal class CustomPageCode : WebViewPage<RenderingModel>
    {
        private const string RootGlobalizeFolder = "~/sitecore/shell/client/Speak/Assets/lib/globalize/main/";
        private const string RootGlobalizeSupplemental = "~/sitecore/shell/client/Speak/Assets/lib/globalize/supplemental/";

        public override void Execute()
        {
            UserControl userControl = base.Html.Sitecore().Controls().GetUserControl(base.Model.Rendering);
            string requireJsWaitSeconds = SpeakSettings.Html.RequireJsWaitSeconds;
            string requireJsMainFile = SpeakSettings.Html.RequireJsMainFile;
            string requireJsConfigFile = SpeakSettings.Html.RequireJsConfigFile;
            string bindingPluginFile = SpeakSettings.Html.BindingPluginFile;
            string str5 = userControl.GetString("SpeakCoreVersion");
            string str6 = userControl.GetString("AssetsLoadingType");
            bool flag = string.IsNullOrEmpty(str5) || (str5 == "{C1B05B3A-689F-474A-918B-5FD50F12849C}");
            string str7 = "{3A0CC262-E84F-4F0F-98E9-BCA5F6BEE829}";
            string str8 = "{84F1BE3E-02A3-42F8-AEB2-E73768790E45}";
            string str9 = "{5A598A7A-CBE9-470F-AD2B-4FD7E0561307}";
            string name = ClientHost.Context.Language.Name;
            string str11 = ClientHost.Context.Culture.Name;
            string threeLetterISOLanguageName = ClientHost.Context.Culture.ThreeLetterISOLanguageName;
            string twoLetterISOLanguageName = ClientHost.Context.Culture.TwoLetterISOLanguageName;
            string str14 = Sitecore.Context.ContentDatabase.Name;
            string str15 = Sitecore.Context.Database.Name;
            if (!ClientHost.Context.Request.Path.ToLower().StartsWith("/sitecore/client/applications/licenseoptions") && !userControl.GetBool("IsLicenceCheckDisabled", false))
            {
                new Sitecore.Shell.Security().CheckUser();
            }
            if (flag)
            {
                requireJsMainFile = SpeakSettings.Html.RequireJSMainFile71;
                if ((str7 == str6) || SpeakSettings.Html.MinifyScripts)
                {
                    requireJsMainFile = "/-/speak/v1/assets/main.min.js";
                }
                else if (str8 == str6)
                {
                    requireJsMainFile = "/-/speak/v1/assets/main.cdn.min.js";
                }
                else if (str9 == str6)
                {
                    requireJsMainFile = "/-/speak/v1/assets/main.bundled.min.js";
                }
            }
            else if ((str7 == str6) || SpeakSettings.Html.MinifyScripts)
            {
                requireJsMainFile = SpeakSettings.Html.RequireJsMainFileMinified;
                bindingPluginFile = SpeakSettings.Html.BindingPluginFileMinified;
            }
            else if (str8 == str6)
            {
                requireJsMainFile = "/-/speak/v1/assets/main-2.0.cdn.min.js";
                bindingPluginFile = SpeakSettings.Html.BindingPluginFileMinified;
            }
            else if (str9 == str6)
            {
                requireJsMainFile = "/-/speak/v1/assets/main-2.0.bundled.min.js";
                bindingPluginFile = SpeakSettings.Html.BindingPluginFileMinified;
            }
            bool debugging = Sitecore.Context.Diagnostics.Debugging;
            HtmlString str16 = base.Html.Sitecore().Placeholder("Page.Title");
            if (string.IsNullOrEmpty(str16.ToString()))
            {
                str16 = new HtmlString(base.Model.PageItem["BrowserTitle"]);
            }
            string str17 = userControl.GetString("PageCodeTypeName");
            string str18 = userControl.GetString("Behaviors") ?? string.Empty;
            if (!string.IsNullOrEmpty(str18))
            {
                str18 = "data-sc-behaviors=\"" + str18 + "\"";
            }
            if (!string.IsNullOrEmpty(str17))
            {
                IPageCode code1 = ReflectionUtil.CreateObject(str17) as IPageCode;
                if (code1 == null)
                {
                    throw new InvalidOperationException("Page Code object not found: " + str17);
                }
                code1.Initialize(RenderingContext.Current);
            }
            string str19 = userControl.GetString("PageCodeScriptFileName");
            if (string.IsNullOrEmpty(str19))
            {
                string[] textArray1 = new string[] { "/sitecore/shell/client/", base.Model.PageItem.Paths.Path.Mid(0x11), "/", base.Model.PageItem.Name, ".js" };
                str19 = string.Concat(textArray1);
                if (!FileUtil.FileExists(str19))
                {
                    str19 = null;
                }
            }
            this.WriteLiteral("\r\n<meta");
            this.WriteLiteral(" data-sc-name=\"sitecoreLanguage\"");
            this.WriteLiteral(" data-sc-content=\"");
            this.Write(name);
            this.WriteLiteral("\"");
            this.WriteLiteral("/>\r\n<meta");
            this.WriteLiteral(" data-sc-name=\"sitecoreContentDatabase\"");
            this.WriteLiteral(" data-sc-content=\"");
            this.Write(str14);
            this.WriteLiteral("\"");
            this.WriteLiteral("/>\r\n<meta");
            this.WriteLiteral(" data-sc-name=\"sitecoreDatabase\"");
            this.WriteLiteral(" data-sc-content=\"");
            this.Write(str15);
            this.WriteLiteral("\"");
            this.WriteLiteral(" />\r\n<meta");
            this.WriteLiteral(" data-sc-name=\"sitecoreCultureName\"");
            this.WriteLiteral(" data-sc-content=\"");
            this.Write(str11);
            this.WriteLiteral("\"");
            this.WriteLiteral(" />\r\n<meta");
            this.WriteLiteral(" data-sc-name=\"sitecoreCultureTwoLetterIsoCode\"");
            this.WriteLiteral(" data-sc-content=\"");
            this.Write(twoLetterISOLanguageName);
            this.WriteLiteral("\"");
            this.WriteLiteral(" />\r\n<meta");
            this.WriteLiteral(" data-sc-name=\"sitecoreCultureThreeLetterIsoCode\"");
            this.WriteLiteral(" data-sc-content=\"");
            this.Write(threeLetterISOLanguageName);
            this.WriteLiteral("\"");
            this.WriteLiteral(" />\r\n\r\n<title>");
            this.Write(str16);
            this.WriteLiteral("</title>\r\n<link");
            this.WriteLiteral(" rel=\"shortcut icon\"");
            this.WriteLiteral(" href=\"/sitecore/images/favicon.ico\"");
            this.WriteLiteral(" />\r\n");
            this.Write(base.Html.Sitecore().PageStylesheets(flag ? "1" : "2"));
            this.WriteLiteral("\r\n\r\n");
            if (debugging)
            {
                this.WriteLiteral("  <script");
                this.WriteLiteral(" type=\"text/javascript\"");
                this.WriteLiteral("> window.__SITECOREDEBUG = true; </script>\r\n");
            }
            this.WriteLiteral("<script>\r\n    var __speak_config_culture = __speak_config_culture || [];\r\n</script>\r\n");
            string[] cultureNames = new string[] { str11, twoLetterISOLanguageName, "en/" };
            string str20 = this.ResolveCultureFolder(cultureNames);
            string jsonFileContent = this.GetJsonFileContent(str20 + "/ca-gregorian.json");
            string str22 = this.GetJsonFileContent(str20 + "/numbers.json");
            string str23 = this.GetJsonFileContent(str20 + "/timeZoneNames.json");
            string str24 = this.GetJsonFileContent(str20 + "/currencies.json");
            string str25 = this.GetJsonFileContent(this.Server.MapPath("~/sitecore/shell/client/Speak/Assets/lib/globalize/supplemental/likelySubtags.json"));
            string str26 = this.GetJsonFileContent(this.Server.MapPath("~/sitecore/shell/client/Speak/Assets/lib/globalize/supplemental/timeData.json"));
            string str27 = this.GetJsonFileContent(this.Server.MapPath("~/sitecore/shell/client/Speak/Assets/lib/globalize/supplemental/weekData.json"));
            string str28 = this.GetJsonFileContent(this.Server.MapPath("~/sitecore/shell/client/Speak/Assets/lib/globalize/supplemental/numberingSystems.json"));
            string str29 = this.GetJsonFileContent(this.Server.MapPath("~/sitecore/shell/client/Speak/Assets/lib/globalize/supplemental/currencyData.json"));
            string str30 = this.GetJsonFileContent(this.Server.MapPath("~/sitecore/shell/client/Speak/Assets/lib/globalize/supplemental/plurals.json"));
            this.WriteLiteral("\r\n\r\n");
            if (((((!string.IsNullOrEmpty(jsonFileContent) && !string.IsNullOrEmpty(str22)) && (!string.IsNullOrEmpty(str23) && !string.IsNullOrEmpty(str24))) && ((!string.IsNullOrEmpty(str25) && !string.IsNullOrEmpty(str26)) && (!string.IsNullOrEmpty(str27) && !string.IsNullOrEmpty(str28)))) && !string.IsNullOrEmpty(str29)) && !string.IsNullOrEmpty(str30))
            {
                JObject obj2 = JObject.Parse(jsonFileContent);
                JObject obj3 = JObject.Parse(str22);
                JObject obj4 = JObject.Parse(str23);
                JObject obj5 = JObject.Parse(str24);
                JObject obj6 = JObject.Parse(str25);
                JObject obj7 = JObject.Parse(str26);
                JObject obj8 = JObject.Parse(str27);
                JObject obj9 = JObject.Parse(str28);
                JObject obj10 = JObject.Parse(str29);
                JObject obj11 = JObject.Parse(str30);
                this.WriteLiteral("  <script>\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj2.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj3.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj4.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj5.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj6.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj7.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj8.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj9.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj10.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  __speak_config_culture.push(");
                this.Write(base.Html.Raw(obj11.ToString(Formatting.None, new JsonConverter[0])));
                this.WriteLiteral(");\r\n  </script>\r\n");
            }
            this.WriteLiteral("\r\n");
            if (str9 == str6)
            {
                this.WriteLiteral("    <script>\r\n        var __speak_config = __speak_config || {};\r\n        __speak_config.useBundle = true;\r\n        __speak_config.deferred = true;\r\n    </script>\r\n");
                if (flag)
                {
                    this.WriteLiteral("        <script");
                    AttributeValue[] values = new AttributeValue[] { Tuple.Create<Tuple<string, int>, Tuple<object, int>, bool>(Tuple.Create<string, int>("", 0x2547), Tuple.Create<object, int>(this.Href("~/sitecore/shell/client/Speak/Assets/lib/core/1.1/sitecore-1.0.2.packed.min.js", new object[0]), 0x2547), false) };
                    this.WriteAttribute("src", Tuple.Create<string, int>(" src=\"", 0x2541), Tuple.Create<string, int>("\"", 0x2595), values);
                    this.WriteLiteral("></script>\r\n");
                }
                else
                {
                    this.WriteLiteral("      <script");
                    AttributeValue[] valueArray2 = new AttributeValue[] { Tuple.Create<Tuple<string, int>, Tuple<object, int>, bool>(Tuple.Create<string, int>("", 0x25cd), Tuple.Create<object, int>(this.Href("~/sitecore/shell/client/Speak/Assets/lib/core/2.0/sitecore.packed.js", new object[0]), 0x25cd), false) };
                    this.WriteAttribute("src", Tuple.Create<string, int>(" src=\"", 0x25c7), Tuple.Create<string, int>("\"", 0x2611), valueArray2);
                    this.WriteLiteral("></script>\r\n");
                    requireJsConfigFile = requireJsConfigFile + "?amdBundle=1";
                }
            }
            this.WriteLiteral("\r\n");
            if (flag && !string.IsNullOrEmpty(str19))
            {
                this.WriteLiteral("  <script");
                this.WriteLiteral(" data-sc-require=\"");
                this.Write(str19);
                this.WriteLiteral("\"");
                this.WriteLiteral(" type=\"text/x-sitecore-pagecode\"");
                this.WriteLiteral(" ");
                this.Write(str18);
                this.WriteLiteral(">\r\n  </script>\r\n");
            }
            this.WriteLiteral("\r\n");
            if (!flag && !string.IsNullOrEmpty(str19))
            {
                this.WriteLiteral("  <script");
                AttributeValue[] valueArray3 = new AttributeValue[] { Tuple.Create<Tuple<string, int>, Tuple<object, int>, bool>(Tuple.Create<string, int>("", 0x2748), Tuple.Create<object, int>(str19, 0x2748), false) };
                this.WriteAttribute("src", Tuple.Create<string, int>(" src=\"", 0x2742), Tuple.Create<string, int>("\"", 0x2757), valueArray3);
                this.WriteLiteral(" type=\"text/x-sitecore-pagecode\"");
                this.WriteLiteral(">\r\n  </script>\r\n");
            }
            this.WriteLiteral("\r\n");
            if (!flag)
            {
                this.WriteLiteral("  <script");
                this.WriteLiteral(" type='text/x-sitecore-pluginscript'");
                this.WriteLiteral(" data-sc-pluginid=\"bindingjsPlugin\"");
                this.WriteLiteral(" data-sc-require=\"");
                this.Write(bindingPluginFile);
                this.WriteLiteral("\"");
                this.WriteLiteral("></script>\r\n");
                this.WriteLiteral("  <script");
                this.WriteLiteral(" type=\"text/javascript\"");
                AttributeValue[] valueArray4 = new AttributeValue[] { Tuple.Create<Tuple<string, int>, Tuple<object, int>, bool>(Tuple.Create<string, int>("", 0x2844), Tuple.Create<object, int>(requireJsConfigFile, 0x2844), false) };
                this.WriteAttribute("src", Tuple.Create<string, int>(" src=\"", 0x283e), Tuple.Create<string, int>("\"", 0x2853), valueArray4);
                this.WriteLiteral("></script>\r\n");
            }
            this.WriteLiteral("\r\n<script");
            this.WriteLiteral(" src=\"/sitecore/shell/client/Speak/Assets/lib/core/2.0/deps/require.js\"");
            this.WriteLiteral(" data-main=\"");
            this.Write(requireJsMainFile);
            this.WriteLiteral("\"");
            this.WriteLiteral(" type=\"text/javascript\"");
            this.WriteLiteral("></script>\r\n\r\n<script");
            this.WriteLiteral(" type=\"text/javascript\"");
            this.WriteLiteral(">require.config({ waitSeconds: ");
            this.Write(requireJsWaitSeconds);
            this.WriteLiteral(" });</script>\r\n");
            this.Write(base.Html.Sitecore().PageScripts());
            this.WriteLiteral("\r\n");
            this.Write(base.Html.Sitecore().Overlays());
        }

        private string GetJsonFileContent(string path)
        {
            Assert.ArgumentNotNull(path, "path");
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            Log.Error($"Culture globalization file {path} not found", this);
            return string.Empty;
        }

        private string ResolveCultureFolder(params string[] cultureNames)
        {
            Assert.ArgumentNotNull(cultureNames, "cultureNames");
            foreach (string str in cultureNames)
            {
                string path = this.Server.MapPath("~/sitecore/shell/client/Speak/Assets/lib/globalize/main/" + str);
                if (Directory.Exists(path))
                {
                    return path;
                }
            }
            throw new Exception("Globalization folders cannot be found");
        }
    }
}
