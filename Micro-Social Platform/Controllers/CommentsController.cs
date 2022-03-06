using Micro_Social_Platform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Micro_Social_Platform.Controllers
{
    public class CommentsController : Controller
    {
        private Micro_Social_Platform.Models.ApplicationDbContext db = new Micro_Social_Platform.Models.ApplicationDbContext();

        /*// GET: INDEX
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            var comms = db.Comments.Include("User");
            return View();
        }*/

        //DELETE: DELETE
        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            NrRequests();
            Profil();
            Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                if (User.IsInRole("Admin"))
                {
                    string authorEmail = comm.User.Email;

                    ///Create the notification body
                    string notificationBody = "<p> Un comentariul postat de dumneavoastra a fost sters deoarece a fost considerat neadecvat de catre administratorii acestui site. <p>";
                    notificationBody += "<br />";
                    notificationBody += "Continutul acestui comentariu este: <br /><br /><em>";
                    notificationBody += comm.Content;
                    notificationBody += "</em>";
                    notificationBody += "<br /><br /> Daca dorito sa contestati decizia va rugam sa raspundeti acestui email.";
                    SendEmailNotification(authorEmail, "Comentariul dumnevoastra a fost stears", notificationBody);
                }
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["message"] = "The comment has been deleted.";
                return Redirect("/Posts/Show/" + comm.PostId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Posts");
            }
        }

        //GET: EDIT
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            NrRequests();
            Profil();
            Comment comm = db.Comments.Find(id);
            ViewBag.Comment = comm;
            comm.UserId = User.Identity.GetUserId();
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                TempData["message"] = "You are not allowed to edit this comment.";
                return RedirectToAction("Index");
            }
     
        }

        //PUT: EDIT
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Comment requestComment)
        {
            NrRequests();
            Profil();
            try
            {
                Comment comm = db.Comments.Find(id);
                if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(comm))
                    {
                        comm.Content = requestComment.Content;
                        db.SaveChanges();
                    }
                }
                else
                {
                    TempData["message"] = "You are not allowed to edit this comment.";
                    return RedirectToAction("Index");
                }
                return Redirect("/Posts/Show/" + comm.PostId);
            }
            catch (Exception)
            {
                return View();
            }

        }
        [NonAction]
        private void Profil()
        {
            foreach (Profile profile in db.Profiles)
            {
                if (User.Identity.GetUserId() == profile.UserId)
                {
                    ViewBag.prof = profile;
                }
            }
        }
        private void SendEmailNotification(string toEmail, string subject, string content)
        {
            const string senderEmail = "irina.pavalasc@my.fmi.unibuc.ro";
            const string senderPassword = "parola.fmi123";
            const string smtpServer = "smtp.gmail.com";
            const int smtpPort = 587;

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            MailMessage email = new MailMessage(senderEmail, toEmail, subject, content);
            email.IsBodyHtml = true;
            email.BodyEncoding = UTF8Encoding.UTF8;

            try
            {
                ///send  the email
                System.Diagnostics.Debug.WriteLine("Sending email...");
                smtpClient.Send(email);
                System.Diagnostics.Debug.WriteLine("Email sent!");
            }

            catch (Exception e)
            {
                ///Failed to send the email message
                System.Diagnostics.Debug.WriteLine("Error occured while trying to send email");
                System.Diagnostics.Debug.WriteLine(e.Message.ToString());
            }
        }
        [NonAction]
        private void NrRequests()
        {
            string currentUser = User.Identity.GetUserId();
            int nr = 0;
            foreach (var fr in db.Friends)
            {
                if (fr.User2_Id == currentUser && fr.Pending == true)
                {
                    nr += 1;
                }
            }
            ViewBag.NrReceivedRequests = nr;
        }
    }
}