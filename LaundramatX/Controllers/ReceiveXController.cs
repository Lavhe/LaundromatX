using LaundramatX.Classes;
using LaundramatX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaundramatX.Controllers
{
    public class ReceiveXController : Controller
    {
        public LaundromatModel LaundromatX = new LaundromatModel();
        public const int MaxPosts = 8;

        // GET: ReceiveX
        public ActionResult Receive()
        {
            return View();
        }

        private bool SearchPerPost(Post post, string search)
        {
            search = search.ToUpper();
            if (post.PostMessage.ToUpper().Contains(search) || post.Account.Name.ToUpper().Contains(search) || post.PostDue.ToUpper().Contains(search))
            {
                return true;
            }

            foreach (var cloth in post.Clothings)
            {
                if (cloth.Name.ToUpper().Contains(search))
                {
                    return true;
                }
            }

            foreach (var comment in post.Comments)
            {
                if (comment.CommentMessage.ToUpper().Contains(search))
                {
                    return true;
                }
            }

            return false;
        }

        public string SearchForPosts(string search)
        {
            try
            {
                //Read all the posts from the database for us to search fine
                var _p = LaundromatX.Posts.Where(p => p.Status.Contains("Waiting for help")).ToList();

                List<string> card = new List<string>();

                foreach (var post in _p)
                {
                    if (SearchPerPost(post, search))
                    {
                        card.Add(X.ConvertPost(post, Session, Url));
                    }
                }

                if (card.Count <= 0)
                {
                    throw new Exception();
                }
                card.Reverse();

                return JavaScriptX.convertString(card.ToArray());

            }
            catch (Exception ex)
            {
                return JavaScriptX.convertString("No more Posts");
            }

        }

        public string getMorePosts(int index, int maxPosts = MaxPosts)
        {
            List<Post> posts = LaundromatX.Posts.Where(p => p.Status.Contains("Waiting for help")).ToList();
            posts.Reverse();

            try
            {
                posts = posts.GetRange(index, maxPosts);
            }
            catch (Exception ex)
            {

                if ((maxPosts + index > posts.Count) && maxPosts > 0)
                {
                    return getMorePosts(index, maxPosts - 1);
                }
            }

            if (maxPosts == 0)
            {
                return JavaScriptX.convertString("No more Posts");
            }

            List<string> card = new List<string>();

            foreach (var p in posts)
            {
                card.Add(X.ConvertPost(p, Session, Url));
            }

            return JavaScriptX.convertString(card.ToArray());

        }

        //The Functionality to add a comment to a post
        public string PerformComment(string UID, string PID, string Message)
        {
            Comment comment = new Comment();

            try
            {
                if (Message == "")
                {
                    throw new Exception("Error");
                }

                var PostID = Convert.ToInt32(PID);
                var UserID = Convert.ToInt32(UID);

                comment.CommentTime = X.FormatDateTime(DateTime.Now);
                comment.PostID = PostID;
                comment.UserID = UserID;
                comment.CommentMessage = Message;
                comment.Account = X.GetUser(Session);

                new System.Threading.Thread(() =>
                {
                    comment.Account = null;
                    comment.CommentTime = X.FormatDateTime(DateTime.Now);
                    LaundromatX.Comments.Add(comment);
                    LaundromatX.SaveChanges();

                    //Notify the owner of the post that someone commented on it
                    var PostOwner = LaundromatX.Posts.Where(p => p.PostID == PostID).First().Account.AccountID;
                    var Commenter = comment.UserID;
                    if (PostOwner != Commenter)
                    {
                        //Check if the owner is the one commenting
                        LaundromatX.Notifications.Add(new Notification() { UserID = PostOwner, Message = "Commented on your post", NotificationTime = X.FormatDateTime(DateTime.Now), From = Commenter.ToString(), RefID=comment.PostID,RefName="post", Seen = "false" });
                        LaundromatX.SaveChanges();
                    }

                }).Start();

                Comment c = comment;

                return X.ConvertComment(c);
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public string GetAllComments(int PostID)
        {
            //Create a variable to hold all the comments
            List<string> comments = new List<string>();

            //Take the comments convert them and store them in the above array
            LaundromatX.Comments.Where(comment => comment.PostID == PostID).ToList().ForEach((e) =>
            {
                comments.Add(X.ConvertComment(e));
            });

            //Check if we have any comments
            if (comments.Count == 0)
            {
                return "No comments";
            }

            return JavaScriptX.convertString(comments.ToArray());
        }

        //This is used to increment the views after we get a new viewer
        public string PerformView(string PostID, string currentViews)
        {
            HttpCookie cookie = Request.Cookies.Get("ViewedPosts");

            if (cookie == null)
            {
                var newCookie = new HttpCookie("ViewedPosts");
                newCookie.Expires.AddYears(5);
                Response.Cookies.Add(newCookie);
                cookie = Request.Cookies.Get("ViewedPosts");
            }

            if (!cookie.Values.AllKeys.Contains(PostID))
            {
                try
                {
                    new System.Threading.Thread(() =>
                    {
                        int postid = Convert.ToInt32(PostID);
                        LaundromatX.Posts.Where(post => post.PostID == postid).ToList()[0].PostViews++;
                        LaundromatX.SaveChanges();

                    }).Start();

                    cookie.Values.Add(PostID, PostID);

                    Response.Cookies.Get("ViewedPosts").Values.Add(cookie.Values);
                    //Values.Add(PostID, PostID);
                    int ans = Convert.ToInt32(currentViews);
                    return (ans++).ToString();

                }
                catch (Exception ex)
                {
                    return currentViews;
                }
            }
            else
            {
                return currentViews;
            }
        }

        //This use to add the helper in the helper's table
        public string AddWasher(string PostID, string washerID, string washerMessage)
        {

            int postID = Convert.ToInt32(PostID);
            int HelperID = Convert.ToInt32(washerID);

            try
            {
                //Check if this guy has already wanted to wash this clothing
                if (LaundromatX.Posts.Where(post => (post.PostID == postID)).First().PostHelpers.ToList().FindAll(washer => (washer.HelperID == HelperID)).Count > 0)
                {
                    return "Sorry but you are already in the queue to wash this";
                }

                if (LaundromatX.Posts.Where(p => p.Account.AccountID == HelperID && p.PostID == postID).ToList().Count > 0)
                {
                    return "Sorry you can not help yourself";
                }

            }
            catch (Exception ex)
            {
            }

            PostHelper helper = new PostHelper();
            helper.HelperID = HelperID;
            helper.PostID = postID;
            helper.HelperTime = X.FormatDateTime(DateTime.Now);
            helper.HelperAccepted = "false";

            new System.Threading.Thread(() =>
            {
                LaundromatX.PostHelpers.Add(helper);
                LaundromatX.SaveChanges();

                //Notify the owner of the post that someone wanna help on it
                var PostOwner = LaundromatX.Posts.Where(p => p.PostID == postID).First().Account.AccountID;
                var Commenter = helper.HelperID;
                if (PostOwner != Commenter)
                {
                    //Check if the owner is the one commenting
                    LaundromatX.Notifications.Add(new Notification() { UserID = PostOwner, Message = "Wants to help", NotificationTime = X.FormatDateTime(DateTime.Now), From = Commenter.ToString(), RefID = helper.PostID, RefName = "post", Seen = "false" });
                    LaundromatX.SaveChanges();
                }

            }).Start();

            return "Done";

        }

        public string GetAllClothes(int PostID)
        {
            List<string> clothes = new List<string>();

            LaundromatX.Posts.Where(post => post.PostID == PostID).First().Clothings.ToList().ForEach((e) =>
            {
                clothes.Add(X.ConvertClothing(e));
            });

            return JavaScriptX.convertString(clothes.ToArray());
        }

        public string GetAllHelpers(int PostID)
        {
            List<string> helpers = new List<string>();

            LaundromatX.PostHelpers.Where(helper => helper.PostID == PostID).ToList().ForEach((e) =>
            {
                helpers.Add(X.ConvertHelper(e, Session));
            });

            if (helpers.Count < 1)
            {
                return "No Helpers";
            }

            return JavaScriptX.convertString(helpers.ToArray());

        }

        public string RemoveHelper(int helperID)
        {
            try
            {
                var h = LaundromatX.PostHelpers.Where(helper => helper.ID == helperID).First();
                LaundromatX.PostHelpers.Remove(h);
                LaundromatX.SaveChanges();
            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
            }

            return "done";
        }
        public string AcceptHelper(int helperID)
        {
            try
            {
                PostHelper helper = null;
                try
                {
                    helper = LaundromatX.PostHelpers.Where(h => h.ID == helperID && h.HelperAccepted == "false").First();
                }
                catch
                {
                    throw new Exception("Helper already added");
                }

                if (helper.Post.PostHelpers.Where(p => p.HelperAccepted == "true").ToList().Count > 0)
                {
                    throw new Exception("Someone is already helping here");
                }

                //Thread the entire process coz the accepter does not have to wait for it all
                new System.Threading.Thread(() =>
                {
                    //1.Accept this guy ,,, 
                    helper.HelperAccepted = "true";

                    //2.Change status of the Post
                    helper.Post.Status = "Work In Progress";

                    //3.Start the transaction and give access to victims
                    string[] properties = helper.Post.PostMessage.Split(',');
                    foreach (var prop in properties)
                    {
                        if (prop.Length > 2)
                        {
                            LaundromatX.Orders.Add(new Order() { Ready = "false", PostHelperID = helperID, Status = prop, EventDateTime = "Nothing" });
                        }
                    }

                    LaundromatX.PostHelpers.Where(h => h.ID == helperID).ToList()[0] = helper;

                    //4.Notify the victims of this ,,, 
                    LaundromatX.Notifications.Add(new Notification() { From = "Staff", NotificationTime = X.FormatDateTime(DateTime.Now), Seen = "false", UserID = Convert.ToInt32(helper.HelperID), Message = $"{helper.Account.Name} {helper.Account.Surname} needs your help", RefID = helper.ID, RefName = "help" , Icon = "Success" });

                    LaundromatX.SaveChanges();
                }).Start();

            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
            }

            return "done";
        }
    }
}