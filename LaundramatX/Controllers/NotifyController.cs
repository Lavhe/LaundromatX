using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaundramatX.Models;
using LaundramatX.Classes;

namespace LaundramatX.Controllers
{
    public class NotifyController : Controller
    {
        private LaundromatModel LaundromatX = new LaundromatModel();
        // GET: Notify
        public ActionResult Notifications()
        {
            if (Convert.ToBoolean(Session[X.isActivatedX]))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string ClearN() {
            var n = LaundromatX.Notifications.ToList();
            LaundromatX.Notifications.RemoveRange(n);
            LaundromatX.SaveChanges();
            return "done";
        }

        public ActionResult b(int UserID, int ID, string Type)
        {
            try
            {
                if (X.GetUser(Session).AccountID != UserID)
                {
                    throw new Exception();
                }

                string answer = "";
                if (Type.StartsWith("post"))
                {
                    var p = LaundromatX.Posts.Where(post => post.PostID == ID).First();
                    answer = X.ConvertPost(p, Session, Url);
                }
                else if (Type.StartsWith("help"))
                {
                    var p = LaundromatX.PostHelpers.Where(help => help.ID == ID).First();
                    answer = X.ConvertOrder(p, Url, Session);
                }

                ViewBag.Answer = answer;
                ViewBag.User = X.GetUser(Session);

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult c()
        {

            try
            {
                List<string> answer = new List<string>();
                int UserID = X.GetUser(Session).AccountID;

                var helpers = LaundromatX.PostHelpers.Where(Help => Help.HelperAccepted == "true" && Help.Post.Status.StartsWith("Work In Progress") && Help.Post.UserID == UserID).ToList();

                return View(helpers);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string LoadOrder(int helpID)
        {
            try
            {
                var help = LaundromatX.PostHelpers.Where(h => h.ID == helpID).First();
                return X.ConvertOrder(help, Url, Session);
            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
            }

        }

        public string MoveStatus(int PostHelpID, int OrderID)
        {
            try
            {
                bool isComplete = true;
                LaundromatX.Orders.Where(o => o.PostHelperID == PostHelpID).ToList().ForEach((order) =>
                {
                    if (order.OrderID < OrderID)
                    {
                        order.Ready = "true";
                        if (order.EventDateTime == "Nothing")
                        {
                            order.EventDateTime = X.FormatDateTime(DateTime.Now);
                        }
                    }
                    else if (order.OrderID == OrderID)
                    {
                        order.Ready = "true";
                        order.EventDateTime = X.FormatDateTime(DateTime.Now);

                        LaundromatX.Notifications.Add(new Notification() { From = "Staff", NotificationTime = X.FormatDateTime(DateTime.Now), Seen = "false", UserID = Convert.ToInt32(order.PostHelper.Post.UserID), Message = $"{order.PostHelper.Account.Name} {order.PostHelper.Account.Surname} is done with '{order.Status}'", RefID = order.PostHelperID, RefName = "help" ,Icon="Success"});
                    }
                    else
                    {
                        isComplete = false;
                        order.Ready = "false";
                        order.EventDateTime = "Nothing";
                    }
                });

                //This means that the worker is done
                if (isComplete)
                {
                    var p = LaundromatX.PostHelpers.Where(help => help.ID == PostHelpID).First().Post;
                    if (p.Status != "Done working")
                    {
                        p.Status = "Done working";
                    }
                }

                LaundromatX.SaveChanges();
                return "done done done";
            }
            catch (Exception ex)
            {
                return "Error " + ex;
            }
        }

        public string GetNotifications(int UserID, bool All = false, bool inDetail = false)
        {
            try
            {
                List<string> Notifications = new List<string>();

                if (All)
                {
                    LaundromatX.Notifications.Where(notify => notify.UserID == UserID).ToList().ForEach((e) =>
                    {
                        if (e.From != "Staff")
                        {
                            try
                            {
                                var commenter = Convert.ToInt32(e.From);
                                var victim = LaundromatX.Accounts.Where(acc => acc.AccountID == commenter).First();
                                Notifications.Add(X.ConvertNotification(e, victim, inDetail));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            Notifications.Add(X.ConvertNotification(e, inDetail: inDetail));
                        }
                    });
                }
                else
                {
                    LaundromatX.Notifications.Where(notify => notify.UserID == UserID && notify.Seen == "false").ToList().ForEach((e) =>
                    {
                        if (e.From != "Staff")
                        {
                            try
                            {
                                var commenter = Convert.ToInt32(e.From);
                                var victim = LaundromatX.Accounts.Where(acc => acc.AccountID == commenter).First();
                                Notifications.Add(X.ConvertNotification(e, victim));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            Notifications.Add(X.ConvertNotification(e));
                        }
                    });
                }
                Notifications.Reverse();
                return JavaScriptX.convertString(Notifications.ToArray());

            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
            }
        }

        public int NotificationsCount(int UserID)
        {
            var total = LaundromatX.Notifications.Where(notify => notify.Account.AccountID == UserID && notify.Seen == "false").ToList().Count;
            return total;
        }

        public string SendMessage(int HelpID, int SenderID, int ReceiverID, string msg)
        {
            LaundromatX.Chats.Add(new Chat() { HelpID = HelpID, ReceiverID = ReceiverID, SenderID = SenderID, Message = msg, SendTime = X.FormatDateTime(DateTime.Now), Seen = "false" });
            LaundromatX.SaveChanges();
            new System.Threading.Thread(() => {
                LaundromatX.Notifications.Add(new Notification() {UserID = ReceiverID,From = SenderID.ToString(),Message=msg,NotificationTime=X.FormatDateTime(DateTime.Now),Seen="false",RefID=HelpID,RefName="help"});
                LaundromatX.SaveChanges();
            }).Start();
            return "done";
        }

        //This just creates a thread to perform a seen for a notification
        public string NotificationSeen(int NotifyID)
        {

            new System.Threading.Thread((e) =>
            {
                try
                {
                    LaundromatX.Notifications.Where(notify => notify.NotifyID == NotifyID).First().Seen = "true";
                    LaundromatX.SaveChanges();
                }
                catch (Exception ex) { }
            }).Start();

            return "done";
        }
    }
}