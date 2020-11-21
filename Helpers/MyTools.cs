using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HienlthOnline.Helpers
{
    public class MyTools
    {

        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.HttpContext.Request.GetTypedHeaders().Referer == null ||
         filterContext.HttpContext.Request.GetTypedHeaders().Host.Host.ToString() != filterContext.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString())
                {
                    filterContext.HttpContext.Response.Redirect("/");
                }
            }
        }


        public static string FullPathFolderImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh");

        public static string NoImage = "no-image-available.png";
        public static string ImageToBase64(string fileName, string folder)
        {
            var fullPath = Path.Combine(FullPathFolderImage, folder, fileName);
            if (File.Exists(fullPath))
            {
                byte[] data = File.ReadAllBytes(fullPath);
                return Convert.ToBase64String(data);
            }

            return null;
        }

        public static string NoImage = "no-image-available.png";
        public static string CheckImageExist(string fileName, string folder)
        {
            if (File.Exists(Path.Combine(FullPathFolderImage, folder, fileName)))
            {
                return $"{folder}/{fileName}";
            }
            return NoImage;
        }

        public static string GetRandom(int length = 5)
        {
            var pattern = @"1234567890qazwsxedcrfvtgbyhn@#$%";
            var rd = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append(pattern[rd.Next(0, pattern.Length)]);

            return sb.ToString();
        }

    }
}