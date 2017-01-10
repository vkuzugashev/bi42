using Bi42.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace bi42
{

    public class SharedLib
    {

        static public Image ResizeImg(Image b, int nWidth, int nHeight, InterpolationMode mode = InterpolationMode.HighQualityBicubic)
        {
            //ужать пропорционально если выходит за требуемые рамки
            if (nWidth > 0 && nHeight == 0 && b.Size.Width > nWidth)
                nHeight = Convert.ToInt32(Math.Round((b.Size.Height * 1.0 / b.Size.Width) * nWidth));
            else if (nWidth > 0 && nHeight == 0 && b.Size.Width <= nWidth)
            {
                nWidth = b.Size.Width;
                nHeight = b.Size.Height;
            }
            if (nWidth == 0 && nHeight > 0 && b.Size.Height > nHeight)
                nWidth = Convert.ToInt32(Math.Round((b.Size.Height * 1.0 / b.Size.Width) / nHeight));
            else if (nWidth == 0 && nHeight > 0 && b.Size.Height <= nHeight)
            {
                nWidth = b.Size.Width;
                nHeight = b.Size.Height;
            }

            Image result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.InterpolationMode = mode;
                g.DrawImage(b, 0, 0, nWidth, nHeight);
                g.Dispose();
            }
            return result;
        }

        static public void UploadFile(string dir, string fileName, bool? delFile, HttpPostedFileBase file)
        {

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            if (delFile == true)
            {
                var path = Path.Combine(dir, fileName);
                if (File.Exists(path))
                    File.Delete(path);
            }

            if (file != null && file.ContentLength > 0)
            {
                var path = Path.Combine(dir, fileName);
                if (File.Exists(path))
                    File.Delete(path);
                file.SaveAs(path);
            }
        }

    }


    public class NotEqual : IRouteConstraint
    {
        private string _match = String.Empty;

        public NotEqual(string match)
        {
            _match = match;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return String.Compare(values[parameterName].ToString(), _match, true) != 0;
        }
    }

    public class NotStartsWith : IRouteConstraint
    {
        private string _match = String.Empty;

        public NotStartsWith(string match)
        {
            _match = match;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return !values[parameterName].ToString().StartsWith(_match);
        }
    }

    public static class UserLastAccessEx
    {

        public static UserLastAccess GetLastAccess(this DbModel db, string key, string userID)
        {
            var row = db.UserLastAccess.Find(userID, key);
            if (row != null)
                return row;
            else
                return null;
        }

        public static void SetLastAccess(this DbModel db, string key, string userID)
        {
            var row = db.UserLastAccess.Find(userID, key );
            if (row == null)
                db.UserLastAccess.Add(new UserLastAccess { UserID = userID, Key = key, LastAccess = DateTime.Now, LastEmail = null, Count = 0 });
            else
            {
                row.LastAccess = DateTime.Now;
                row.LastEmail = null;
                row.Count = 0;
            }
            db.SaveChanges();
        }

        public static void SetLastEmail(this DbModel db, string key, string userID, int count)
        {
            var row = db.UserLastAccess.Find(userID, key);
            if (row == null)
                db.UserLastAccess.Add(new UserLastAccess { UserID = userID, Key = key, LastAccess = DateTime.Now, LastEmail = DateTime.Now, Count = count });
            else
            {
                row.LastEmail = DateTime.Now;
                row.Count = count;
            }
            db.SaveChanges();
        }

    }




}