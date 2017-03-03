namespace Sitecore.Support
{
    using RazorGenerator.Mvc;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;

    public static class RazorGeneratorMvcStart
    {
        public static void Start()
        {
            PrecompiledMvcEngine item = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly)
            {
                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
            };
            ViewEngines.Engines.Insert(0, item);
            VirtualPathFactoryManager.RegisterVirtualPathFactory(item);
        }
    }
}
