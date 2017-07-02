using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LaundramatX.Models;
using static LaundramatX.Classes.HomeX;

namespace LaundramatX.Classes
{
    public class HomeX
    {
        public class Link
        {
            public Link(string Action, string Controller)
            {
                this.Action = Action;
                this.Controller = Controller;
            }

            public Link(string Action, string Controller, object Routes)
            {
                this.Action = Action;
                this.Controller = Controller;
                this.Routes = Routes;
            }

            public string Action;
            public string Controller;
            public object Routes = new object();
        }

        public class Page
        {
            public Page(string name, string icon, Link link)
            {
                this.name = name;
                this.icon = icon;
                this.link = link;
            }
            public string name;
            public string icon;
            public Link link;
        };
    }


    public static class X
    {
        //This method is used to format the dateTime in the way i want (*_*) very safe
        public static string FormatDateTime(DateTime datetime)
        {

            string answer = "";
            string year = datetime.Year.ToString();
            string month = datetime.Month.ToString();
            string day = datetime.Day.ToString();
            string hour = datetime.Hour.ToString();
            string minutes = datetime.Minute.ToString();

            answer = year + "_" + month + "_" + day + "_" + hour + "_" + minutes;

            return answer;
        }

        private static string CalcRate(List<int> Rates)
        {
            //We are avoiding to divide by ZERO
            if (Rates.Count == 0)
            {
                return "0";
            }

            int total = 0;

            foreach (var value in Rates)
            {
                total += value;
            }

            int Average = total / Rates.Count;

            return Average.ToString();
        }
        public static string ConvertPost(Post post, HttpSessionStateBase session, UrlHelper Url)
        {
            string CommentSection = "";
            string PriceBtn = "";
            string AcceptLaundry = "";
            string ModalTrigger = "";
            string MapTrigger = "";
            string MapIcon = "";
            string PlaceIcon = "";
            string[] properties = post.PostMessage.Split(',');

            post.PostTime = TimeAge(DateTime.Now, post.PostTime);
            post.PostDue = TimeLeft(DateTime.Now, post.PostDue);

            Account User = null;

            string ps = "";
            if (post.PostType == 0)
            {
                PlaceIcon = "<i class='material-icons'>local_laundry_service</i>";

                foreach (var property in X.LaundryProperties)
                {
                    if (properties.Contains(property))
                    {
                        ps +=
                 "<li class='collection-item'>" +
                          "<sub><i class='material-icons green-text'>done</i></sub>" +
                          "<sup><label>  " + property + "</label></sup>" +
                 "</li>";
                    }
                    else
                    {
                        ps +=
                 "<li class='collection-item'>" +
                          "<sub><i class='material-icons red-text'>close</i></sub>" +
                          "<sup><label>  " + property + "</label></sup>" +
                 "</li>";
                    }

                }
            }
            else
            {
                PlaceIcon = "<i class='material-icons'>home</i>";

                foreach (var property in X.HomeProperties)
                {
                    if (properties.Contains(property))
                    {
                        ps +=
                 "<li class='collection-item'>" +
                          "<sub><i class='material-icons green-text'>done</i></sub>" +
                          "<sup><label>  " + property + "</label></sup>" +
                 "</li>";
                    }
                    else
                    {
                        ps +=
                 "<li class='collection-item'>" +
                          "<sub><i class='material-icons red-text'>close</i></sub>" +
                          "<sup><label>  " + property + "</label></sup>" +
                 "</li>";
                    }

                }

            }

            post.PostMessage = "<div class='PropertyListDiv'>" +
                                  "<ul class='collection'>" +
                                     ps +
                                  "</ul>" +
                               "</div>";

            //Get the current user if he/she is activated
            if (Convert.ToBoolean(session[X.isActivatedX]))
            {
                User = (Account)session[X.UserX];

                CommentSection = "<div class='collection-item center'>" +
                 $"<img style='width:50px;height:50px' src='{User.ProfilePic}' alt='' class='circle profile-image prefix'>" +
                 "<input class='CommentOnPostInput' type='text' style='width:50%'>" +
                 "<a class='CommentOnPost btn btn-floating waves-effect waves-circle' data-userID='" + User.AccountID + "' data-postID='" + post.PostID + "' data-link='" + Url.Action("PerformComment", "ReceiveX") + "'><i class='material-icons'>send</i></a>" +
                 "</div>";

                PriceBtn = "<a class='btn-floating btn-large waves-effect PriceBtn RedBg activator' onclick=\"GetAllClothes('" + post.PostID + "')\"><sup>R</sup>" + post.PostPrice + "</a>";

                AcceptLaundry = "<div class=\"center recieveWashButton\">" +
                        "<a class=\"circle btn btn-floating teal\" onclick=\"ConfirmWannaWash('" + post.PostID + "','" + User.AccountID + "')\"><i class=\"material-icons\">done</i></a>" +
                    "</div>";

                ModalTrigger = "modal-trigger";

            }
            else
            {
                CommentSection = "<div class='collection-item center'><a class='center btn btn-flat waves-effect center'>Login / Register</a></div>";

                PriceBtn = "<a class='btn-floating btn-large waves-effect PriceBtn RedBg tooltipped' data-position='left' data-tooltip='Log in to use this'><sup>R</sup>" + post.PostPrice + "</a>";

            }

            if (post.LocationX == null)
            {
                MapIcon = "<i class='material-icons'>location_off</i>";
            }
            else
            {
                MapIcon = "<i class='material-icons'>location_on</i>";
                MapTrigger = "onclick=\"toggleMapDesc(this,'" + post.LocationX.LocationLat + "','" + post.LocationX.LocationLon + "', 'POSTmap_" + post.PostID + "' )\" data-PostID='" + post.PostID + "' data-Toggled='false' data-MapLoaded='false'";
            }

            try
            {
                return "<div class='ThePost blog' Seen='false' PostID='" + post.PostID + "' Link='" + Url.Action("PerformView", "ReceiveX") + "'>" +
            "<div class=''>" +
            "<div class='product-card'>" +
                "<div class='card hoverable'>" +
                    "<div class='row RecievePostHeader'>" +
                        "<div class='col s2 left text-left'>" +
                            $"<i class='left'><img data-caption='{post.Account.Description}' class='circle z-depth-2 materialboxed' src='{post.Account.ProfilePic}' alt='Profile Pic'></i>" +
                        "</div><div class='col s6 center text-center'>" +
                            "<a class='card-title PosterName' onclick=\"RedirectAction('" + Url.Action("Index", "Account", new { userID = post.Account.AccountID }) + "')\">" + post.Account.Name + "</a>" +
                        "</div><div class='col s4 right text-right'>" +
                            "<span class='TimePosted small'>" +
                                    "<sub class='TimeIcon'><i class='material-icons'>access_time</i></sub>" +
                                    "<sup>" + post.PostTime + "</sup>" +
                                    "<sup class='TimeLeft'>" + post.PostDue + "</sup>" +
                            "</span>" +
                        "</div>" +
                    "</div>" +
                    "<div class='divider grey lighten-1'></div>" +
                    "<div class='card-image waves-effect' style='height:200px;width:100%' >" +
                        "<div style='z-index:-1' id='DivPostMap_" + post.PostID + "' hidden>" +
                                    "<div style='height:100%;width:100%;z-index:-1' id='POSTmap_" + post.PostID + "'>" +
                                        "<div class='loadingDivFather'>" +
                                            "<div class='loading loadingDiv'>" +
                                                "<div class='bullet'>L</div>" +
                                                "<div class='bullet'>o</div>" +
                                                "<div class='bullet'>a</div>" +
                                                "<div class='bullet'>d</div>" +
                                                "<div class='bullet'>i</div>" +
                                                "<div class='bullet'>n</div>" +
                                                "<div class='bullet'>g</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                        "</div>" +
                        "<div style='z-index:-1' id='DivPostDesc_" + post.PostID + "'>" +
                            post.PostMessage +
                        "</div>" +
                    "</div>" +
                    "<ul class='card-action-buttons'> " +
                        "<li>" +
                        PriceBtn +
                        "</li>" +
                    "</ul>" +
                    "<div class='card-content row'>" +
                        "<ul class='PostActions'>" +
                            $"<li class='left small'><a role='button' {MapTrigger} class='waves-effect waves-circle'><span class='black-text'><sub>{MapIcon}</sub></span></a></li>" +
                            $"<li class='left small'><span class='black-text'><sub>{PlaceIcon}</sub></span></li>" +
                            $"<li class='right small'><a class='{ModalTrigger} waves-effect waves-circle' onclick=\"getAllHelpers('{post.PostID}','{post.PostHelpers.Count}')\" href='#ModalHelpers_{post.PostID}'><span class='black-text'><sub><i class='material-icons'>timer</i></sub><sup id='nHelp_{post.PostID}'> {post.PostHelpers.Count}</sup></span></a></li>" +
                            $"<li class='right small'><a><span class='black-text'><sub><i class='material-icons'>visibility</i></sub><sup class='PostViews'>{post.PostViews}</sup></span></a></li>" +
                            $"<li class='right small'><a class='PostComments modal-trigger waves-effect waves-circle' onclick=\"getAllComments('{post.PostID}','{post.Comments.Count}')\" href='#Modal_{post.PostID}'><span class='black-text'><sub><i class='material-icons'>chat</i></sub><sup id='nComments_{post.PostID}'> {post.Comments.Count}</sup></span></a></li>" +
                        "</ul>" +
                        "<div class='modal' id='ModalHelpers_" + post.PostID + "'>" +
                            "<div class='modal-header'>" +
                                "<div class='modal-close circle btn btn-floating red' style='float:right'><i class='material-icons'>close</i></div>" +
                                "</div>" +
                            "<div class='modal-content'>" +
                            "<div class='row'>" +
                                    "<div class='col s12'>" +
                                        "<div class='loadingDivFather hidden ModalHelpersLoading'>" +
                                            "<div class='loading loadingDiv'>" +
                                                "<div class='bullet'>L</div>" +
                                                "<div class='bullet'>o</div>" +
                                                "<div class='bullet'>a</div>" +
                                                "<div class='bullet'>d</div>" +
                                                "<div class='bullet'>i</div>" +
                                                "<div class='bullet'>n</div>" +
                                                "<div class='bullet'>g</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                "<ul class='collection' helpers id='Helpers_" + post.PostID + "'></ul>" +
                            "</div>" +
                        "</div>" +
                        "<div class='modal modal-fixed-footer' id='Modal_" + post.PostID + "'>" +
                            "<div class='modal-header'>" +
                                "<div class='modal-close circle btn btn-floating red' style='float:right'><i class='material-icons'>close</i></div>" +
                            "</div>" +
                            "<div class='modal-content'>" +
                                "<div class='row'>" +
                                    "<div class='col s12'>" +
                                        "<div class='loadingDivFather hidden ModalLoading'>" +
                                            "<div class='loading loadingDiv'>" +
                                                "<div class='bullet'>L</div>" +
                                                "<div class='bullet'>o</div>" +
                                                "<div class='bullet'>a</div>" +
                                                "<div class='bullet'>d</div>" +
                                                "<div class='bullet'>i</div>" +
                                                "<div class='bullet'>n</div>" +
                                                "<div class='bullet'>g</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                "<ul class='collection' comments id='Comments_" + post.PostID + "'></ul>" +
                            "</div>" +
                            "<div class='modal-actions modal-footer teal lighten-4 left'>" +
                                CommentSection +
                "</div>" +
            "</div>" +
        "</div>" +
        "<div class='card-reveal' id='reveal_" + post.PostID + "'>" +
            "<div class='row center'>" +
                "<a class='btn btn-floating red waves-effect center' onclick=\"CloseRevealModal('#reveal_" + post.PostID + "')\"><i class='material-icons'>close</i></a>" +
            "</div>" +
            "<div class='ClothingListDiv'>" +
                "<ul class='collection' id='ClothingList_" + post.PostID + "'></ul>" +
            "</div>" +
         "<div class='row center'>" +
                "<div class='center recieveWashText'>" +
                    "<label>Accept laundry?</label>" +
                "</div>" +
                AcceptLaundry +
            "</div>" +
           "</div>" +
          "</div>" +
         "</div>" +
        "</div>" +
       "</div>";

            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
            }
        }

        public static string ConvertAdminCard(Account person, HttpSessionStateBase Session, UrlHelper Url)
        {
            string Stars = "";
            string location = "";
            if (person.LocationX != null)
            {
                location = "<p><i class='material-icons'>location_on</i> " + person.LocationX.LocationTownCity + "</p>";
            }

            if (Convert.ToBoolean(Session[X.isActivatedX]))
            {
                for (int i = 5; i >= 1; i--)
                {
                    Stars += "<input id='" + person.AccountID + " rating-" + i + "' name='rating_" + person.AccountID + "' class='rateUser_" + person.AccountID + "' type='radio' value='" + i + "' onclick=\"RateUser('" + person.AccountID + "','" + i + "','" + X.GetUserID(Session[X.UserX]) + "','" + Url.Action("RateUser", "Admin") + "','.rateUser_" + person.AccountID + "')\"/>" +
                        "<label class='col right RatingStar' for='" + person.AccountID + " rating-" + i + "' data-value='" + i + "'>" +
                             "<span class='rating-star'>" +
                                  "<i class='material-icons'>star</i>" +
                              "</span>" +
                          "</label>";
                }
            }
            else
            {
                for (int i = 5; i >= 1; i--)
                {
                    Stars += "<input id='" + person.AccountID + " rating-" + i + "' name='rating_" + person.AccountID + "' class='rateUser_" + person.AccountID + "' type='radio' value='" + i + "' disabled' />" +
                        "<label class='col right RatingStar' for='" + person.AccountID + " rating-" + i + "' data-value='" + i + "'>" +
                             "<span class='rating-star'>" +
                                  "<i class='material-icons'>star</i>" +
                              "</span>" +
                          "</label>";
                }
            }

            return "<div class='blog'>" +
            "<div class='card'>" +
                "<h6 class='card-title center text-center grey-text text-darken-4'>" + person.Name + "</h6>" +
                "<div class='card-image waves-effect LaundromatProfile'>" +
                    $"<img src='{person.ProfilePic}'>" +
                "</div>" +
                "<div class='card-content'>" +
                    "<div class='row rating-form form-group'>" +
                        "<div class='col s12 form-item center row center' style='margin-left:0;width:100%'>" +
                            Stars +
                        "</div>" +
                    "</div>" +
                    "<p><i class='material-icons'>person_pin</i> " + person.Surname + " " + person.Name + "</p>" +
                       location +
                    "<p><i class='material-icons'>phone</i> 0" + person.Contact + "</p>" +
                    "<p><i class='material-icons'>email</i> " + person.Email + "</p>" +
                    "<p><i class='material-icons'>person</i> " + person.Gender + "</p>" +
                    "<div class='center'>" +
                        "<a onclick=\"RedirectAction('" + Url.Action("Index", "Account", new { userID = person.AccountID }) + "')\" class='btn RedBg white-text waves-effect waves-orange'>View Profile</a>" +
                      "</div>" +
                "</div>" +
            "</div>" +
        "</div>";

        }

        public static string ConvertClothing(Clothing cloth)
        {
            return "<li class='collection-item'>" + cloth.Name +
                "<span class='teal white-text badge'>" + cloth.Quantity + "</span></li>";
        }

        //Take the helper and convert it to an HTML list
        public static string ConvertHelper(PostHelper helper, HttpSessionStateBase Session)
        {
            string PostResponse = "";
            string Stars = "";

            var rate = Convert.ToInt32(CalcRate(helper.Account.Rates1.Select(p => p.RateValue).ToList()));

            for (int i = 5; i >= 1; i--)
            {
                if (rate == i)
                {
                    Stars += "<input id='" + helper.Account.AccountID + " rating-" + i + "' name='rating_" + helper.Account.AccountID + "' checked='checked' class='rateUser_" + helper.Account.AccountID + "' type='radio' value='" + i + "' disabled />" +
                    "<label class='col right RatingStar' for='" + helper.Account.AccountID + " rating-" + i + "' data-value='" + i + "'>" +
                         "<span class='rating-star'>" +
                              "<i class='material-icons'>star</i>" +
                          "</span>" +
                      "</label>";
                }
                else
                {
                    Stars += "<input id='" + helper.Account.AccountID + " rating-" + i + "' name='rating_" + helper.Account.AccountID + "' class='rateUser_" + helper.Account.AccountID + "' type='radio' value='" + i + "' disabled />" +
                    "<label class='col right RatingStar' for='" + helper.Account.AccountID + " rating-" + i + "' data-value='" + i + "'>" +
                         "<span class='rating-star'>" +
                              "<i class='material-icons'>star</i>" +
                          "</span>" +
                      "</label>";
                }
            }

            if (GetUser(Session) != null)
            {
                var userID = GetUser(Session).AccountID;
                if (userID == helper.Post.Account.AccountID)
                {
                    //This is the post owner
                    PostResponse = $"<a class='secondary-content HelperAction' role='button' onclick=\"ConfirmToast('Are you sure?','AcceptHelper({helper.ID })')\"><p class='grey-text ultra-small'><i class='material-icons teal-text'>done</i></p></a>";
                }
                else if (userID == helper.HelperID)
                {
                    //This is in the queue of helpers
                    PostResponse = "<a class='secondary-content HelperAction' role='button' onclick=\"ConfirmToast('Are you sure?','RemoveHelper(" + helper.ID + ")')\"><p class='grey-text ultra-small'><i class='material-icons red-text'>delete</i></p></a>";
                }
            }

            helper.HelperTime = X.TimeAge(DateTime.Now, helper.HelperTime);
            return $"<li id='PostHelperLI_{helper.ID}' class='collection-item avatar selected PostHelpersLI' role='button' data-postID='{helper.PostID}' data-helperID='{helper.ID}'><img src='{helper.Account.ProfilePic}' class='circle' />" +
                              "<a href='/Account/Index?userID=" + helper.Account.AccountID + "'><span class='email-title'>" + helper.Account.Name + "</span></a>" +
                              "<p class='truncate grey-text ultra-small'>" +
                              "<div class='row rating-form form-group HelperRating'>" +
                                  "<div class='col s12 form-item center row' style='width:100%'>" +
                                     Stars +
                                  "</div>" +
                              "</div>" +
                              "</p>" +
                              "<a class='secondary-content email-time'><span class='grey-text ultra-small'>" +
                              helper.HelperTime + "</span></a>" +
                               PostResponse + "</li>";
        }

        //Take a comment and convert it to an HTML list
        public static string ConvertComment(Comment comment)
        {
            comment.CommentTime = TimeAge(DateTime.Now, comment.CommentTime);
            return $"<li class='collection-item avatar selected PostCommentsLI'><img src='{comment.Account.ProfilePic}' class='circle'/>" +
                                            "<a href='/Account/Index?userID=" + comment.Account.AccountID + "'><span class='email-title'> " + comment.Account.Name + "</span></a>" +
                                            "<p class='truncate grey-text ultra-small'>" + comment.CommentMessage + "</p>" +
                                            "<a class='secondary-content email-time'><i class='mdi-editor-attach-file attach-file'>" +
                                            "</i><span class='grey-text ultra-small'>" + comment.CommentTime + "</span></a></li>";
        }

        //Convert one notification to the perfect format
        public static string ConvertNotification(Notification notify, Account commenter = null, bool inDetail = false)
        {

            string NotifyIcon = "";
            string NotifyFrom = "";
            string Message = "";
            string classes = "";
            string result = "";

            if (commenter == null)
            {
                if (notify.Icon == "Warning")
                {
                    NotifyIcon = "<i class='material-icons text-red'>error</i>";
                }
                else if (notify.Icon == "Success")
                {
                    NotifyIcon = "<i class='material-icons text-teal'>done</i>";
                }
                else if (notify.Icon == "Warning")
                {
                    NotifyIcon = "<i class='material-icons text-orange'>warning</i>";
                }
                Message = notify.Message;
            }
            else
            {
                if (inDetail)
                {
                    NotifyFrom = $"<a href='/Account/Index?UserID={commenter.AccountID}'><span class='blue-text truncate'><u>{commenter.Name} {commenter.Surname}</u></span></a>";
                    NotifyIcon = $"<img src='{commenter.ProfilePic}' class='circle' style='width:60px;height:60px'/>";
                }
                else
                {
                    NotifyFrom = $"<span class='blue-text truncate'><u>{commenter.Name} {commenter.Surname}</u></span>";
                    NotifyIcon = $"<img src='{commenter.ProfilePic}' class='circle' style='width:40px;height:40px'/>";
                }

                Message = commenter.Name + " " + notify.Message;
            }


            if (inDetail)
            {
                if (notify.Seen == "false")
                {
                    classes = "_hoverable";
                }
                else
                {
                    classes = "transparent";
                }

                result = $"<div class='card {classes} col s12 row DetailNotifyLI' id='NotificationLi_{notify.NotifyID}' role='button'  data-ID='{notify.RefID}' data-RefName='{notify.RefName}' onclick=\"PerformNotifcationSeen('{notify.NotifyID}',this)\">" +
                            "<div class='col s2 card-image DetailNotifyIcon'>" +
                                  NotifyIcon +
                            "</div>" +
                            "<div class='col s10 row DetailNotifyMsg'>" +
                                "<div class='col s6 card-title'>" +
                                   NotifyFrom +
                                "</div>" +
                                "<div class='col s6'>" +
                                   $"<span class='blue-text ultra-small secondary-content email-time'>{X.TimeAge(DateTime.Now, notify.NotificationTime)}</span>" +
                                "</div>" +
                                "<div class='col s12 card-content'>" +
                                   $"<span class='truncate black-text small'>{Message}</span>" +
                                "</div>" +
                            "</div>" +
                            "<div class='divider white'></div>" +
                          "</div>";
            }
            else
            {
                classes = "transparent NotificationLi _hoverable";
                result = $"<div class='{classes} col s12 row NotifyItem' id='NotificationLi_{notify.NotifyID}' role='button' data-ID='{notify.RefID}' data-RefName='{notify.RefName}' onclick=\"PerformNotifcationSeen('{notify.NotifyID}',this)\">" +
                            "<div class='col s2 NotifyIcon black-text'>" +
                                     NotifyIcon +
                            "</div>" +
                            "<div class='col s10 NotifyMsg'>" +
                                  $"<span class='truncate black-text small'>{Message}</span>" +
                                  $"<span class='blue-text ultra-small secondary-content email-time'>{X.TimeAge(DateTime.Now, notify.NotificationTime)}</span>" +
                            "</div>" +
                         "</div>";
            }

            return result;
        }

        public static string ConvertOrder(PostHelper help, UrlHelper Url, HttpSessionStateBase Session)
        {

            string Chats = "";
            string SeenColour = "";
            string Orders = "";
            string Ready = "";
            string OnGoing = "";
            string PendingStatus = "";
            int SenderID = 0;
            int RecieverID = 0;

            if (help.Post.Status.StartsWith("Done working"))
            {
                PendingStatus = "transparent";
            }

            if (help.HelperID == GetUser(Session).AccountID)
            {
                SenderID = (int)help.HelperID;
                RecieverID = (int)help.Post.UserID;
            }
            else
            {
                SenderID = (int)help.Post.UserID;
                RecieverID = (int)help.HelperID;
            }

            foreach (var line in help.Orders)
            {
                Ready = "";
                if (line.Ready != "false")
                {
                    Ready = "complete";
                    OnGoing = $"<span class='date'>{X.TimeAge(DateTime.Now, line.EventDateTime)}</span>";
                }
                else
                {
                    OnGoing = "<span class='date'>......</span>";
                }

                Orders += $"<li class='li {Ready} WorkLI' data-OrderID='{line.OrderID}'>" +
                      "<div class='timestamp'>" +
                          OnGoing +
                      "</div>" +
                      "<div class='status'>" +
                        $"<p>{line.Status}</p>" +
                       "</div>" +
                  "</li>";
            }


            foreach (var msg in help.Chats)
            {
                if (msg.Seen == "false")
                {
                    SeenColour = "<i class='material-icons black-text'>done</i>";
                }
                else
                {
                    SeenColour = "<i class='material-icons teal-text'>done</i>";
                }

                if (msg.ReceiverID == X.GetUser(Session).AccountID)
                {
                    Chats += "<div class='card-panel LeftChat col s7 grey lighten-2'>" +
                        "<p>" +
                            msg.Message +
                        "</p>" +
                        $"<span class='small blue-text'>{X.TimeAge(DateTime.Now, msg.SendTime)}</span>" +
                    "</div>";
                }
                else
                {
                    Chats += "<div class='card-panel RightChat col s7 right grey lighten-4'>" +
                            "<p>" +
                                msg.Message +
                            "</p>" +
                            $"<span class='small blue-text'>{X.TimeAge(DateTime.Now, msg.SendTime)}</span>" +
                            "<span class='text-right right small'>" +
                    SeenColour +
                    "</span>" +
                "</div>";
                }
            }

            return $"<div class='{PendingStatus} row'>" +
                       "<div class='card col s10 offset-s1 m6 offset-m3'>" +
                          "<div class='card-title'>" +
                            "<h6 class='text-center'>" +
                                $"<a href='{Url.Action("Index", "Account", new { userID = help.Account.AccountID })}'>{help.Account.Name} {help.Account.Surname}</a>" +
                                "<span style='font-size:xx-small;padding-right:5px' class='text-right right small'>" +
                                    TimeAge(DateTime.Now, help.HelperTime) +
                                    "<br />" +
                                    TimeLeft(DateTime.Now, help.Post.PostDue) +
                                "</span>" +
                            "</h6>" +
                          "</div>" +
                          "<div class='card-content'>" +
                            "<div class='card-block'>" +
                                "<div class='row OrderProgress'>" +
                                    "<div class='card-panel'>" +
                                        $"<ul class='timeline' data-helpID='{help.ID}' id='OrderUl_{help.ID}'>" +
                                            Orders +
                                        "</ul> " +
                                    "</div> " +
                                "</div> " +
                            "</div> " +
                        "</div> " +
                    "</div>" +
                    "<div class='col s12 row OrderChat transparent'>" +
                                    "<div class='card-panel'>" +
                                        $"<span class='left RedText'>{help.Account.Name} {help.Account.Surname}</span>" +
                                        $"<span class='right GreyText'>{GetUser(Session).Name} {GetUser(Session).Surname}</span>" +
                                    "</div>" +
                                    Chats  +
                                "</div>" +
                                    "<div class='col s12 row center text-center'>" +
                                        "<textarea class='materialize-textarea col s9 ChatTextBox' placeholder='Type your message here'></textarea>" +
                                        $"<a class='btn teal col s3 waves-effect' onclick=\"Chat_SendMessage(this,'{help.ID}','{RecieverID}','{SenderID}')\">SEND</a>" +
                                    "</div>" +
                                "</div>";
        }

        public static string TimeLeft(DateTime then, string posted)
        {
            List<int> datetime = new List<int>();

            posted.Split('_').ToList().ForEach((e) =>
            {
                datetime.Add(Convert.ToInt32(e));
            });

            var now = new { Year = datetime[0], Month = datetime[1], Day = datetime[2], Hour = datetime[3], Minute = datetime[4] };

            if ((now.Year - then.Year) > 1)
            {
                int years = (now.Year - then.Year);
                return years + " years left";
            }
            else if ((now.Year - then.Year) == 1)
            {
                return "1 year left";
            }
            else if ((now.Year - then.Year) == 0)
            {
                if ((now.Month - then.Month) > 1)
                {
                    int months = (now.Month - then.Month);
                    return months + " months left";
                }
                else if ((now.Month - then.Month) == 1)
                {
                    return "1 month left";
                }
                else if ((now.Month - then.Month) == 0)
                {
                    if (((now.Day - then.Day) / 7) > 1.0)
                    {
                        int weeks = (now.Day - then.Day) / 7;
                        return weeks + " weeks left";
                    }
                    else if (((now.Day - then.Day) / 7) == 1.0)
                    {
                        return "1 week left";
                    }
                    else if (((now.Day - then.Day) / 7) > 0.1)
                    {

                        if (((now.Day - then.Day) > 1))
                        {
                            int days = (now.Day - then.Day);
                            return days + " days left";
                        }
                        else if ((now.Day - then.Day) == 1)
                        {
                            return "tomorrow";
                        }
                        else if ((now.Day - then.Day) == 0)
                        {
                            if (((now.Hour - then.Hour) > 1))
                            {
                                int hours = (now.Hour - then.Hour);
                                return hours + " hrs left";
                            }
                            else if ((now.Hour - then.Hour) == 1)
                            {
                                return "1 hour left";
                            }
                            else if ((now.Hour - then.Hour) == 0)
                            {
                                if (((now.Minute - then.Minute) > 1))
                                {
                                    int mins = (now.Minute - then.Minute);
                                    return mins + " minutes left";
                                }
                                else if ((now.Minute - then.Minute) == 1)
                                {
                                    return "1 minute left";
                                }
                                else if ((now.Minute - then.Minute) == 0)
                                {
                                    return "Due now";
                                }
                            }
                        }
                    }
                }
            }

            return "<span class='red-text'>Due</span>";
        }

        public static string TimeAge(DateTime now, string posted)
        {
            List<int> datetime = new List<int>();

            posted.Split('_').ToList().ForEach((e) =>
            {
                datetime.Add(Convert.ToInt32(e));
            });

            var then = new { Year = datetime[0], Month = datetime[1], Day = datetime[2], Hour = datetime[3], Minute = datetime[4] };

            if ((now.Year - then.Year) > 10)
            {
                return "more than a decade ago";
            }

            if ((now.Year - then.Year) > 1)
            {
                int years = (now.Year - then.Year);
                return years + " years ago";
            }
            else if ((now.Year - then.Year) == 1)
            {
                return "a year ago";
            }

            if ((now.Month - then.Month) > 1)
            {
                int months = (now.Month - then.Month);
                return months + " months ago";
            }
            else if ((now.Month - then.Month) == 1)
            {
                return "a month ago";
            }

            if (((now.Day - then.Day) / 7) > 1.0)
            {
                int weeks = (now.Day - then.Day) / 7;
                return weeks + " weeks ago";
            }
            else if (((now.Day - then.Day) / 7) == 1.0)
            {
                return "a week ago";
            }

            if (((now.Day - then.Day) > 1))
            {
                int days = (now.Day - then.Day);
                return days + " days ago";
            }
            else if ((now.Day - then.Day) == 1)
            {
                return "yesterday";
            }

            if (((now.Hour - then.Hour) > 1))
            {
                int hours = (now.Hour - then.Hour);
                return hours + " hrs ago";
            }
            else if ((now.Hour - then.Hour) == 1)
            {
                return "an hour ago";
            }

            if (((now.Minute - then.Minute) > 1))
            {
                int mins = (now.Minute - then.Minute);
                return mins + " minutes ago";
            }
            else if ((now.Minute - then.Minute) == 1)
            {
                return "a minute ago";
            }

            return "just now";
        }

        private static Page FindPageX(string name)
        {
            return new HomePage().XPage.Find(p => p.name == name);
        }

        //Deal with the conversion of months 
        public static int GetMonth(string month)
        {
            month = month.ToUpper();
            string[] months = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
            for (int i = 0; i < 12; i++)
            {
                if (month.Contains(months[i]))
                {
                    return (i + 1);
                }
            }
            return 1;
        }

        //Get the location of the current user
        public static bool GetUserLocation(object userSection)
        {
            Account user = null;
            try
            {
                user = (Account)userSection;

                if (user == null)
                {
                    throw new Exception();
                }

                if (user.LocationX == null)
                {
                    //IF the location is not there we return false
                    throw new Exception();
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        //Get the details of the current logged in user
        public static Account GetUser(HttpSessionStateBase Session)
        {
            Account user = null;
            try
            {
                user = (Account)Session[X.UserX];
                if (user == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return user;

        }


        //Pass in the account and return the name of the specific user
        public static string GetUserName(object userSection)
        {
            Account user = null;
            try
            {
                user = (Account)userSection;
                if (user == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return "Unknown";
            }

            return user.Name;
        }

        //Pass in the account and return the id of the specific user
        public static int GetUserID(object userSection)
        {
            Account user = null;
            try
            {
                user = (Account)userSection;
                if (user == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

            return user.AccountID;
        }

        //This is used to simplify the Finding of pages whenever we wanna use them
        public static Page HomePage = FindPageX("Home");
        public static Page BrandSquare = FindPageX("BrandSquare");
        public static Page SendPage = FindPageX("Send");
        public static Page ReceivePage = FindPageX("Receive");
        public static Page RegisterPage = FindPageX("LogIn/Register");
        public static Page HelpPage = FindPageX("Help");
        public static Page LaundromatsPage = FindPageX("Laundromats");
        public static Page AdminPage = FindPageX("Administrate");

        //This returns all the pages that are available
        public static List<Page> AllPages = new HomePage().XPage;
        //The list of all possible clothes names to auto add on the tags for sending clothes
        public static string[] ClothesNames = { "T-Shirt", "Trouser", "Skirt", "Tie", "Jersey", "Blazer", "Jean", "Golf Shirt" };

        //THIS IS A LIST OF ALL POSSIBLE LAUNDRY PROPERTIES
        public static string[] LaundryProperties = { "Ironed", "Packed and folded accordingly", "Collected from me", "Delivered back", "Fabric softened", "Separately" };

        //THIS IS A LIST OF ALL POSSIBLE HOME PROPERTIES
        public static string[] HomeProperties = { "Hand wash", "Machine", "Use my washing powder", "Iron & fold and pack" };

        public static ClothesProp[] clothesprop = {
            new ClothesProp("Suit",4.9,50),new ClothesProp("Shirt",4.9,50) , new ClothesProp("Tie",4.9,50), new ClothesProp("Dress",4.9,50), new ClothesProp("Blouse",4.9,50), new ClothesProp("Skirt",4.9,50),new ClothesProp("Tank-top",4.9,50),new ClothesProp("Coat",4.9,50), new ClothesProp("Jacket",4.9,50),new ClothesProp("T-shirt",4.9,50), new ClothesProp("Trouser",4.9,50), new ClothesProp("Jacket",4.9,50), new ClothesProp("T-shirt",4.9,50), new ClothesProp("Jean",4.9,50), new ClothesProp("Short",4.9,50), new ClothesProp("Pull-over",4.9,50), new ClothesProp("Bikini",4.9,50), new ClothesProp("Pull-over",4.9,50), new ClothesProp("Swimsuit",4.9,50),new ClothesProp("Underwear",4.9,50), new ClothesProp("Vest",4.9,50), new ClothesProp("Bathrobes",4.9,50), new ClothesProp("Pajamas",4.9,50)
        };

        public static DryCleanProp[] DryCleanprop = {

        };

        //Names of the session variables
        public static string UserX { get { return "UserX"; } }
        public static string isActivatedX { get { return "UserXisActivated"; } }

        public static string CookieuserID { get { return "dxtfcyuybvnbrvhegfdx"; } }
        public static string CookieIsActive { get { return "xzcvbnoiuytrsdfghjk"; } }
    }


    public class HomePage
    {
        public List<HomeX.Page> XPage = new List<HomeX.Page>();

        public HomePage()
        {
            //Add all pages that are in the Home Page
            XPage.Add(new HomeX.Page("Home", "home", new HomeX.Link("Index", "Home", new { ErrorMessage = "" })));
            XPage.Add(new HomeX.Page("Laundromats", "store", new HomeX.Link("Laundromats", "Home")));
            XPage.Add(new HomeX.Page("BrandSquare", "star_border", new HomeX.Link("Index", "BrandSquare")));
            XPage.Add(new HomeX.Page("Send", "add_shopping_cart", new HomeX.Link("Send", "SendX")));
            XPage.Add(new HomeX.Page("Receive", "swap_horiz", new HomeX.Link("Receive", "ReceiveX")));
            XPage.Add(new HomeX.Page("LogIn/Register", "perm_identity", new HomeX.Link("Register", "Account")));
            XPage.Add(new HomeX.Page("Administrate", "verified_user", new HomeX.Link("Index", "Admin")));
            XPage.Add(new HomeX.Page("Help", "NoIcon", new HomeX.Link("Index", "Help")));
        }
    }

    public static class JavaScriptX
    {
        public static string convertString(object value)
        {
            return new JavaScriptSerializer().Serialize(value);
        }
    }

    public class ClothesProp
    {
        public string Type { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }

        public ClothesProp(string Type, double Weight, double Price)
        {
            this.Type = Type;
            this.Weight = Weight;
            this.Price = Price;
        }
    }

    public class DryCleanProp
    {
        public string Name { get; set; }
        public string Price { get; set; }
    }
}
