using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaundramatX.Models;
using LaundramatX.Classes;

namespace LaundramatX.Controllers
{
    public class AdminController : Controller
    {
        public LaundromatModel LaundromatX = new LaundromatModel();

        //This will be called whenever the Laundromat's page loads to get all the rating stars
        public string GetAllStars(int UserID)
        {
            
            var answer = CalcRate(LaundromatX.Rates.Where(acc => acc.UserID == UserID).Select(p => p.RateValue).ToList());
        
            return answer;
        }

        public string CalcRate(List<int> Rates)
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

        //This is used to rate the user by a user who is logged in only
        public string RateUser(string userID, string RateValue, string raterID)
        {
            try
            {

                int UserID = Convert.ToInt32(userID);
                int RaterID = Convert.ToInt32(raterID);
                int Ratevalue = Convert.ToInt32(RateValue);

                try
                {
                    LaundromatX.Rates.Where(rate => rate.RaterID == RaterID && rate.UserID == UserID).First().RateValue = Ratevalue;
                }
                catch (Exception ex)
                {
                    LaundromatX.Rates.Add(new Rate() { RaterID = RaterID, RateValue = Ratevalue, UserID = UserID });
                }

                LaundromatX.SaveChanges();

                var answer = CalcRate(LaundromatX.Rates.Where(rate => rate.UserID == UserID).Select(p => p.RateValue).ToList());

                return answer;
            }
            catch (Exception ex) {
            }

            return "Done";
        }

        [HttpPost]
        //This is used to remove a company via the edit profile page
        public string RemoveCompany(int AdminId, int CompanyID, int UserID)
        {
            //Check if they wanna hack us
            if (Convert.ToBoolean(Session[X.isActivatedX]))
            {
                if (X.GetUserID(Session[X.UserX]) != UserID)
                {
                    return "Error , Trying to hack us???";
                }

            }
            else
            {
                return "Error , you are not logged in";
            }

            try
            {
                var company = LaundromatX.Companies.ToList().Find(c => c.CompanyID == CompanyID);
                LaundromatX.Companies.Remove(company);
                LaundromatX.SaveChanges();

            }
            catch (Exception ex)
            {
                //They are tryimg yo hack us again
                return "Error , " + ex.Message + " please report this error";
            }
            return "Done";
        }

        public ActionResult Index()
        {
            //TODO : ONLY ADMINS MUST ENTER HERE
            return View(LaundromatX);
        }

        public ActionResult ManageUsers()
        {

            //TODO : ONLY ADMINS MUST ENTER HERE
            return View(LaundromatX.Accounts.ToList());
        }

        [HttpPost]
        public ActionResult RemoveUser(string AccountID)
        {

            //TODO : ONLY ADMINS MUST ENTER HERE
            try
            {
                int id = Convert.ToInt32(AccountID);
                var person = LaundromatX.Accounts.Where(acc => acc.AccountID == id).ToList()[0];
                LaundromatX.Accounts.Remove(person);
                LaundromatX.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            return View("ManageUsers");

        }

    }
}