using LaundramatX.Classes;
using LaundramatX.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaundramatX.Controllers
{
    public class SendXController : Controller
    {
        public LaundromatModel LaundromatX = new LaundromatModel();

        //If a user tries to send incorrectly we redirect that nigga
        [HttpGet]
        public ActionResult Send()
        {
            //We do not allow none users to send
            if (!Convert.ToBoolean(Session[X.isActivatedX]))
            {
                string message = "Please log in or register for you to send clothes";
                return RedirectToAction("Index", "Home", new { Errormessage = message });
            }
            else
            {
                return View();
            }
        }

        public void GetImage(string image)
        {
            string path = Server.MapPath($"ProfilePic{X.GetUser(Session).AccountID}.jpg");
            new System.Threading.Thread(() =>
            {
                byte[] bytes = Convert.FromBase64String(image);
                MemoryStream ms = new MemoryStream(bytes);
                Image img = Image.FromStream(ms);

                //TODO: Fix this saving coz its gonna change the world
                img.Save(path, ImageFormat.Jpeg);
                img.Dispose();
                ms.Dispose();
                img = null;
                ms = null;
            }).Start();
        }

        // GET: SendX
        [HttpPost]
        public string Send(string UserID, string Message, string Price, string DateDue, string TimeDue, string Items, string Quantity, string Properties, int Type)
        {
            
            var date = DateDue.Split(',');
            var hour = Convert.ToInt32(TimeDue.Split(':')[0]);
            var minute = Convert.ToInt32(TimeDue.Split(':')[1]);
            var day = Convert.ToInt32(date[0].Split()[0]);
            var month = X.GetMonth(date[0].Split()[1]);
            var year = Convert.ToInt32(date[1]);

            DateTime DateTimeDue = new DateTime(year, month, day, hour, minute, 0, 0);

            try
            {

                int userid = Convert.ToInt32(UserID);

                Post post = new Post();
                post.UserID = userid;
                post.PostPrice = Convert.ToDouble(Price);
                post.PostTime = X.FormatDateTime(DateTime.Now);
                post.PostDue = X.FormatDateTime(DateTimeDue);
                post.PostViews = 0;
                post.PostType = Type;
                post.Status = "Waiting for help";
                Properties.Split(',').ToList().ForEach((value) =>
                {
                    post.PostMessage += value + ",";
                });
                post.LocationID = X.GetUser(Session).LocationID;

                LaundromatX.Posts.Add(post);
                LaundromatX.SaveChanges();

                new System.Threading.Thread(() =>
                {
                    //Add all the items to the DB each in a new line
                    var name = Items.Split(',').ToList();
                    var quantity = Quantity.Split(',').ToList();

                    for (int i = 0; i < name.Count; i++)
                    {
                        LaundromatX.Clothings.Add(new Clothing() { Name = name[i], Quantity = Convert.ToInt32(quantity[i]), PostID = post.PostID });
                    }
                    if (Message.Length > 2)
                    {
                        LaundromatX.Comments.Add(new Comment() { UserID = userid, PostID = post.PostID, CommentMessage = Message, CommentTime = X.FormatDateTime(DateTime.Now) });
                    }

                    LaundromatX.SaveChanges();

                }).Start();
            }
            catch (Exception ex)
            {
                return "Error 900 " + ex.Message;

            }

            return "Done";
        }

        public string GetUserLocations(int UserID){
            try
            {
                var user = LaundromatX.Accounts.Where(acc => acc.AccountID == UserID).First();
                List<string> locations = new List<string>();

                if (user.LocationX1 != null)
                {
                    var home = new {
                        Lat = user.LocationX1.LocationLat,
                        Lon = user.LocationX1.LocationLon,
                        HouseNumber = user.LocationX1.LocationHouseShopNumber,
                        StreetName = user.LocationX1.LocationStreetName,
                        LocalName = user.LocationX1.LocationLocalName,
                        City_TownName = user.LocationX1.LocationTownCity,
                        Province = user.LocationX1.LocationProvince,
                        Country = user.LocationX1.LocationCountry
                    };

                    locations.Add(JavaScriptX.convertString(home));
                }
                else {
                    locations.Add("No Location");
                }

                if (user.LocationX2 != null)
                {
                    var work = new
                    {
                        Lat = user.LocationX2.LocationLat,
                        Lon = user.LocationX2.LocationLon,
                        HouseNumber = user.LocationX2.LocationHouseShopNumber,
                        StreetName = user.LocationX2.LocationStreetName,
                        LocalName = user.LocationX2.LocationLocalName,
                        City_TownName = user.LocationX2.LocationTownCity,
                        Province = user.LocationX2.LocationProvince,
                        Country = user.LocationX2.LocationCountry
                    };
                    locations.Add(JavaScriptX.convertString(work));
                }
                else
                {
                    locations.Add("No Location");
                }

                if (user.LocationX != null)
                {
                    var current = new
                    {
                        Lat = user.LocationX.LocationLat,
                        Lon = user.LocationX.LocationLon,
                        HouseNumber = user.LocationX.LocationHouseShopNumber,
                        StreetName = user.LocationX.LocationStreetName,
                        LocalName = user.LocationX.LocationLocalName,
                        City_TownName = user.LocationX.LocationTownCity,
                        Province = user.LocationX.LocationProvince,
                        Country = user.LocationX.LocationCountry
                    };
                    locations.Add(JavaScriptX.convertString(current));
                }
                else
                {
                    locations.Add("No Location");
                }

                return JavaScriptX.convertString(locations.ToArray());
            }
            catch (Exception ex) {
                return "Error " + ex.Message;
            }
        }

        public string GetClosestDryCleaners(double lat = 0, double lon = 0)
        {
            System.Threading.Thread.Sleep(4000);

            if (lat == 0 && lon == 0)
            {
                //This user has a location in our database
                //TODO : Take is coordinates and put them here
                lat = 0;
                lon = 0;
            }
            else
            {
                //Take his lat
            }

            //TODO : GO TO THE DATABASE TAKE ALL THE NEARBY DRY CLEANS AND RETURN THEM AS A JSON

            //If still the lat and lon are 0 then we do not wanna hep you
            if (lat == 0 && lon == 0)
            {
                return "false";

            }
            else
            {
                var answer = new
                {
                    Name = "DryCleanName",
                    Location = new
                    {
                        Lat = "Lat",
                        Lon = "Longitude",
                    },
                    Times = new
                    {
                        Open = "Open",
                        Close = "close"
                    },
                    Description = "Who we are",
                    Image = "Image of the Company",
                };

                return JavaScriptX.convertString(answer);

            }

        }
    }
}