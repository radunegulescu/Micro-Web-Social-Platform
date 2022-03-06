using Micro_Social_Platform.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Security.Application;
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
    public class PostsController : Controller
    {   
        private Micro_Social_Platform.Models.ApplicationDbContext db = new Micro_Social_Platform.Models.ApplicationDbContext();
        private int _perPage = 3;
        // GET: Posts
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            NrRequests();
            string currentUser = User.Identity.GetUserId();
            ViewBag.user = db.Users.Find(currentUser);
            List<string> friends = new List<string>();
            List<Post> posts = new List<Post>();
            UserFriends(friends);
            ViewBag.friends = friends;
            foreach (var post in db.Posts)
            {
                if ((friends.Contains(post.UserId) || User.IsInRole("Admin") || post.UserId == currentUser) && post.GroupId == null)
                {
                    posts.Add(post);
                }
            }
            ViewBag.Posts = posts;
            SetAccessRight();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }
            List<String> noPhoto = new List<string>();
            List<UploadFile> files = new List<UploadFile>();
            List<String> ids = new List<string>();
            foreach (var user in db.Users)
            {
                ids.Add(user.Id);
            }
            foreach (var id in ids)
            {
                UploadFile file = null;
                foreach (var f in db.UploadFiles)
                {
                    if (f.UserId == id)
                    {
                        file = f;
                    }
                    
                }
                if (file != null)
                {
                    files.Add(file);
                }
                else
                {
                    noPhoto.Add(id);
                }
            }
            ViewBag.noPhoto = noPhoto;
            ViewBag.files = files;
            Profil();
            ViewBag.UsersProfiles = db.Profiles;


            var totalItems = posts.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }

            var paginatedPosts = posts.Skip(offset).Take(this._perPage);

            //ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Posts = paginatedPosts;
            return View();
        }

        //GET: SHOW
        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(int id)
        {
            NrRequests();
            List<string> friends = new List<string>();
            UserFriends(friends);
            ViewBag.friends = friends;
            var posts = db.Posts.Include("Group");
            Post post = null;
            SetAccessRight();
            Profil();
            foreach (Post p in posts)
            {
                if(p.PostId == id)
                {
                    post = p;
                }
            }
            ViewBag.UsersProfiles = db.Profiles;
            string currentUser = User.Identity.GetUserId();
            List<Group> aux = new List<Group>();
            var groupsUser = db.Groups.Where(g => g.Users.Any(u => u.Id == currentUser));
            foreach (var gru in groupsUser)
            {
                aux.Add(gru);
            }
            if (friends.Contains(post.UserId) || User.IsInRole("Admin") || (post.GroupId != null && aux.Contains(post.Group)))
            {
                return View(post);
            }
            else
            {
                TempData["message"] = "Postare inaccesibila";
                return RedirectToAction("Index");
            }
        }

        //POST: SHOW
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(Comment comm)
        {
            NrRequests();
            List<string> friends = new List<string>();
            UserFriends(friends);
            ViewBag.friends = friends;
            ViewBag.UsersProfiles = db.Profiles;
            comm.Date = DateTime.Now;
            comm.UserId = User.Identity.GetUserId();
            Profil();
            try
            {  
                if(ModelState.IsValid)
                {
                    db.Comments.Add(comm);
                    db.SaveChanges();
                    return Redirect("/Posts/Show/" + comm.PostId);

                }
                else
                {
                  Post a =db.Posts.Find (comm.PostId);
                    SetAccessRight();
                    return View(a);
                }


            }
            catch
            {
                Post a = db.Posts.Find(comm.PostId);
                SetAccessRight();
                return View(a);
            }

        }

        //GET: NEW
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(int? id)
        {
            NrRequests();
            Profil();
            Post post = new Post();
            post.GroupId = id;
            post.UserId = User.Identity.GetUserId();
            if (id != null)
            {
                Group group = db.Groups.Find(id);
                ViewBag.gr = group;
            }
            post.UserId = User.Identity.GetUserId();
            return View(post);
        }

        //POST: NEW
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        [ValidateInput(false)]
        public ActionResult New(Post post)
        {
            NrRequests();
            Profil();
            post.Date = DateTime.Now;
            post.UserId = User.Identity.GetUserId();
            int? grid = post.GroupId;
            if (post.GroupId != null)
            {
                Group group = db.Groups.Find(post.GroupId);
                ViewBag.gr = group;
            }
            try
            {
                if (ModelState.IsValid)
                {
                    post.Content = Sanitizer.GetSafeHtmlFragment(post.Content);
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "The post has been added.";
                    if(post.GroupId != null)
                    {
                        return RedirectToRoute(new { controller = "Groups", action = "Show", id = post.GroupId });
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(post);
                }
            }
            catch (Exception e)
            {
                post.Title = e.InnerException.Message;
                post.Title = e.InnerException.ToString();
                return View(post);
            }
        }

        //GET: EDIT
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            NrRequests();
            Profil();
            Post post = db.Posts.Find(id);
            post.UserId = User.Identity.GetUserId();
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(post);
            }
            else
            {
                TempData["message"] = "You are not allowed to edit this post.";
                return RedirectToAction("Index");
            }
        }

        //PUT: EDIT
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Post requestPost)
        {
            NrRequests();
            Profil();
            try
            {
                if (ModelState.IsValid)
                {
                    Post post = db.Posts.Find(id);
                    if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(post))
                        {
                            requestPost.Content = Sanitizer.GetSafeHtmlFragment(requestPost.Content);
                            //post = requesPost;
                            post.Title = requestPost.Title;
                            post.Content = requestPost.Content;
                            db.SaveChanges();
                            TempData["message"] = "The post has been edited.";
                            if (post.GroupId != null)
                            {
                                return RedirectToRoute(new { controller = "Groups", action = "Show", id = post.GroupId });
                            }
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return View(requestPost);
                        }
                    }
                    else
                    {
                        TempData["message"] = "You are not allowed to edit this post.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestPost);
                }
            }
            catch (Exception)
            {
                return View(requestPost);
            }
        }

        //DELETE: DELETE
        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            NrRequests();
            Profil();
            Post post = db.Posts.Find(id);
            int? id1 = post.GroupId;
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                if (User.IsInRole("Admin"))
                {
                    string authorEmail = post.User.Email;

                    ///Create the notification body
                    string notificationBody = "<p> A fost stearsa postarea dumneavoastra cu titlul: <p>";
                    notificationBody += "<p><strong>" + post.Title + "<strong>, ";
                    notificationBody += "<br /";
                    notificationBody += "<p> Aceasta a fost considerata neadecvata de catre administratorii acestui site.</p>";
                    notificationBody += "<br />";
                    notificationBody += "Continutul acestei postari este: <br /><br /><em>";
                    notificationBody += post.Content;
                    notificationBody += "</em>";
                    notificationBody += "<br /><br /> Daca doriti sa contestati decizia va rugam sa raspundeti acestui email.";
                    SendEmailNotification(authorEmail, "Postarea dumnevoastra a fost stearsa" , notificationBody);
                }
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "The post has been deleted.";
                if (id1 != null)
                {
                    return RedirectToRoute(new { controller = "Groups", action = "Show", id = id1 });
                }
                return RedirectToAction("Index");
                
            }

            else
            {
                TempData["message"] = "You are not allowed to delete this post.";
                return RedirectToAction("Index");
            }
        }
        [NonAction]
        private void SetAccessRight()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("User") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }
        [NonAction]
        private void Profil()
        {
            foreach(ApplicationUser user in db.Users)
            {
                if(User.Identity.GetUserId() == user.Id)
                {
                    ViewBag.user = user;
                }
            }
            foreach (Profile profile in db.Profiles)
            {
                if (User.Identity.GetUserId() == profile.UserId)
                {
                    ViewBag.prof = profile;
                }
            }
        }

        [NonAction]
        private void UserFriends(List<string> friends)
        {
            string currentUser = User.Identity.GetUserId();
            friends.Add(currentUser);
            foreach (var fr in db.Friends)
            {
                if (fr.User1_Id == currentUser && (fr.Accepted == true))
                {
                    friends.Add(fr.User2_Id);
                }
                if (fr.User2_Id == currentUser && (fr.Accepted == true))
                {
                    friends.Add(fr.User1_Id);
                }
            }
        }

        [NonAction]
        private bool Friendf(string currentUser, string friend)
        {
            foreach(var friendship in db.Friends)
            {
                if((friendship.User1_Id == currentUser && friendship.User2_Id == friend && friendship.Accepted) || (friendship.User2_Id == currentUser && friendship.User1_Id == friend && friendship.Accepted))
                {
                    return true;
                }
            }
            return false;
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


    }
}