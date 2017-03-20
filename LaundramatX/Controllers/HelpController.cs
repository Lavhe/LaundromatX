using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaundramatX.Classes;


namespace LaundramatX.Controllers
{
    public class HelpController : Controller
    {
        // GET: Help
        public ActionResult Stuff()
        {
            return View();
        }
        
        public ActionResult FAQ()
        {
            
            return View();
        }

        public string TalkToUs(string Name,string Email,string Message)
        {
            string txtMessage = "Hello my name is " + Name + " and my email is " + Email + " i would like to say " + Environment.NewLine
                + Message;
            //TODO: Take the 'txtMessage' and email it to our selves
            return "Done";
        }
    }
}