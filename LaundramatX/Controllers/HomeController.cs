using LaundramatX.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaundramatX.Models;
using LaundramatX.Controllers;

namespace LaundramatX.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Test()
        {
            return View();
        }

        public LaundromatModel LaundromatX = new LaundromatModel();

        public ActionResult Laundromats()
        {
            return View(LaundromatX.Accounts.ToList().FindAll(acc => acc.Companies.Count > 0).ToList());
        }

        public string clearDb()
        {
            var acc3 = LaundromatX.Rates.ToList();
            LaundromatX.Rates.RemoveRange(acc3);


            var acc11 = LaundromatX.Clothings.ToList();
            LaundromatX.Clothings.RemoveRange(acc11);

            var acc7 = LaundromatX.Notifications.ToList();
            LaundromatX.Notifications.RemoveRange(acc7);

            var acc2 = LaundromatX.Chats.ToList();
            LaundromatX.Chats.RemoveRange(acc2);

            var acc8 = LaundromatX.LocationXes.ToList();
            LaundromatX.LocationXes.RemoveRange(acc8);

            var acc9 = LaundromatX.Companies.ToList();
            LaundromatX.Companies.RemoveRange(acc9);

            var acc10 = LaundromatX.Comments.ToList();
            LaundromatX.Comments.RemoveRange(acc10);


            var acc6 = LaundromatX.Orders.ToList();
            LaundromatX.Orders.RemoveRange(acc6);

            var acc5 = LaundromatX.PostHelpers.ToList();
            LaundromatX.PostHelpers.RemoveRange(acc5);

            var acc4 = LaundromatX.Posts.ToList();
            LaundromatX.Posts.RemoveRange(acc4);


            var acc = LaundromatX.Accounts.ToList();
            LaundromatX.Accounts.RemoveRange(acc);

            LaundromatX.SaveChanges();
            return "done";
        }

        public ActionResult Info()
        {
            return View();
        }

        public ActionResult Index(string Errormessage = "")
        {
            ViewBag.Message = Errormessage;
            if (Convert.ToBoolean(Session[X.isActivatedX]))
            {
                ViewBag.Message = "";
            }
            return View();
        }


        public string format()
        {
            try
            {
                LaundromatX.Posts.ToList().ForEach((post) =>
                {
                    try
                    {
                        string[] DateTime = post.PostDue.Split();
                        if (DateTime.Length > 1)
                        {
                            string[] date = DateTime[0].Split('/');
                            string[] time = DateTime[1].Split(':');

                            var now = new { Year = Convert.ToInt32(date[2]), Month = Convert.ToInt32(date[1]), Day = Convert.ToInt32(date[0]), Hour = Convert.ToInt32(time[0]), Minute = Convert.ToInt32(time[1]) };
                            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
                            post.PostDue = X.FormatDateTime(d);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
                LaundromatX.SaveChanges();

            }
            catch (Exception ex)
            {
                LaundromatX.SaveChanges();
                return "Error " + ex.Message;
            }
            return "DONE";
        }


        public string Searching(string Search, string Filter, string Sort)
        {
            var search = Search.ToUpper();

            try
            {
                switch (Filter)
                {
                    case "Post":

                        var posts = LaundromatX.Posts.ToList().Where(post => (post.Account.Name.ToUpper().Contains(search) || post.PostMessage.ToUpper().Contains(search) || post.Clothings.ToList().FindAll(c => c.Name.ToUpper().Contains(search)).Count > 0)).ToList();

                        if (Sort == "Date")
                        {
                            posts.Reverse();
                        }
                        else if (Sort == "Price")
                        {
                            posts.OrderBy(o => Convert.ToInt32(o.PostPrice));
                        }
                        else if (Sort == "Views")
                        {
                            posts.OrderBy(o => o.PostViews);
                        }
                        else if (Sort == "Comments")
                        {
                            posts.OrderBy(o => o.Comments.Count);
                        }
                        else if (Sort == "Helpers")
                        {
                            posts.OrderBy(o => o.PostHelpers.Count);
                        }

                        List<string> postss = new List<string>();
                        foreach (var post in posts)
                        {
                            postss.Add(X.ConvertPost(post, Session, Url));
                        }

                        return JavaScriptX.convertString(postss.ToArray());

                    case "Laundromats":

                        var laundromats = LaundromatX.Accounts.ToList().Where(person => (person.Companies.Count > 0 && person.Name.ToUpper().Contains(search) || person.Surname.ToUpper().Contains(search) || person.Comments.ToList().FindAll(c => c.CommentMessage.ToUpper().Contains(search)).Count > 0)).ToList();

                        if (Sort == "Date")
                        {
                            laundromats.Reverse();
                        }
                        else if (Sort == "Comments")
                        {
                            laundromats.OrderBy(o => o.Comments.Count);
                        }

                        List<string> laundromatss = new List<string>();
                        foreach (var laundry in laundromats)
                        {
                            laundromatss.Add(X.ConvertAdminCard(laundry, Session, Url));
                        }

                        return JavaScriptX.convertString(laundromatss.ToArray());

                    case "Brands":

                        return JavaScriptX.convertString("No Brands for now");
                }
            }
            catch (Exception ex)
            {
                return JavaScriptX.convertString("Error " + ex);
            }
            return JavaScriptX.convertString("Done but why???");
        }

        public ActionResult Search(string SearchText = "")
        {
            ViewBag.Search = SearchText;
            return View();
        }
    }
}