using Micro_Social_Platform.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Micro_Social_Platform.Controllers
{
    public class UsersController : Controller
    {
        private Micro_Social_Platform.Models.ApplicationDbContext db = new Micro_Social_Platform.Models.ApplicationDbContext();
        // GET: Users
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index(string search)
        {
            NrRequests();
            SetAccessRight();
            Profil();
           
            var users = (from user in db.Users
                                           .Include(u => u.SentRequests)
                                           .Include(u => u.ReceivedRequests)
                         select user).ToList();
            if (search != null)
            {
                search = search.Trim();
            }
            if (search == null)
            {
                ViewBag.Users = users;
                ViewBag.count = users.Count();
            }
            else
            {
                ViewBag.Users = users.Where(x => (x.NameUser.ToUpper().Contains(search.ToUpper())));
                ViewBag.count = users.Where(x => (x.NameUser.ToUpper().Contains(search.ToUpper()))).Count();
            }
            
            ViewBag.UsersProfiles = db.Profiles;
            AddFriendsButton();
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult AddFriend(FormCollection formData)
        {
            NrRequests();
            string currentUser = User.Identity.GetUserId();
            string friendToAdd = formData.Get("UserId"); 
            bool OK = false;
            foreach (var us in db.Users)
            {
                if(friendToAdd == us.Id)
                {
                    OK = true;
                }
            }
            try
            {
                if (OK)
                {
                    foreach (var fr in db.Friends)
                    {
                        if ((fr.User1_Id == currentUser && fr.User2_Id == friendToAdd && (fr.Accepted == true || fr.Pending == true)) || (fr.User2_Id == currentUser && fr.User1_Id == friendToAdd && (fr.Accepted == true || fr.Pending == true)) || (fr.User1_Id == fr.User2_Id))
                        {
                            TempData["Message"] = "Deja prieteni";
                            return RedirectToAction("Index");
                        }
                    }
                    Friend friendship = new Friend();
                    friendship.User1_Id = currentUser;
                    friendship.User2_Id = friendToAdd;
                    friendship.Accepted = false; 
                    friendship.Pending = true;
                    friendship.RequestTime = DateTime.Now;
                    db.Friends.Add(friendship);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "User inexistent";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Requests()
        {
            NrRequests();
            Profil();
            string currentUser = User.Identity.GetUserId();
            var Us = (from user in db.Users
                                           .Include(u => u.SentRequests)
                                           .Include(u => u.ReceivedRequests)
                      where user.Id == currentUser
                      select user).ToList();
            if (Us.Count() > 0)
            {
                ViewBag.user = Us[0];
                return View();
            }
            else
            {
                TempData["Message"] = "User inexistent";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult Accept(FormCollection formData)
        {
            NrRequests();
            int friend_id = -1;
            foreach (var fr in db.Friends)
            {
                if (Int32.Parse(formData.Get("FriendId")) == fr.FriendId)
                {
                    friend_id = fr.FriendId;
                }
            }
            if (friend_id != -1)
            {
                Friend fr = db.Friends.Find(friend_id);
                fr.Accepted = true;
                fr.Pending = false;
                db.SaveChanges();
                return RedirectToAction("Requests");
            }
            else
            {
                TempData["Message"] = "Id-ul nu a putut fi gasit";
                return RedirectToAction("Requests");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult Reject(FormCollection formData)
        {
            NrRequests();
            int friend_id = -1;
            foreach (var fr in db.Friends)
            {
                if (Int32.Parse(formData.Get("FriendId")) == fr.FriendId)
                {
                    friend_id = fr.FriendId;
                }
            }
            if (friend_id != -1)
            {
                Friend fr = db.Friends.Find(friend_id);
                fr.Accepted = false;
                fr.Pending = false;
                db.SaveChanges();
                return RedirectToAction("Requests");
            }
            else
            {
                TempData["Message"] = "Id-ul nu a putut fi gasit";
                return RedirectToAction("Requests");
            }
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult ShowFriends(string id)
        {
            NrRequests();
            SetAccessRight();
            Profil();
            List<string> friends = new List<string>();
            UserFriends(friends);
            var users = (from user in db.Users
                   .Include(u => u.SentRequests)
                   .Include(u => u.ReceivedRequests)
                   where friends.Contains(user.Id)
                         select user).ToList();
            ViewBag.Users = users;
            ViewBag.friends = friends;
            ViewBag.UsersProfiles = db.Profiles;
            return View("Index");
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(string id)
        {
            NrRequests();
            SetAccessRight();
            Profil();
            ApplicationUser user = db.Users.Find(id);

            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            //var userRole = roles.Where(j => j.Id == user.Roles.FirstOrDefault().RoleId).
            //               Select(a => a.Name).FirstOrDefault();

            string currentRole = user.Roles.FirstOrDefault().RoleId;

            var userRoleName = (from role in db.Roles
                                where role.Id == currentRole
                                select role.Name).First();
            ViewBag.roleName = userRoleName;
            AddFriendsButton();
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            NrRequests();
            SetAccessRight();
            Profil();
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            NrRequests();
            SetAccessRight();
            Profil();
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;

            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;

                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Name);
                    }

                    var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
                    UserManager.AddToRole(id, selectedRole.Name);

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                newData.Id = id;
                return View(newData);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles select role;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            NrRequests();
            SetAccessRight();
            Profil();
            ApplicationDbContext context = new ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);

            var posts = db.Posts.Where(a => a.UserId == id);
            foreach (var post in posts)
            {
                db.Posts.Remove(post);
            }
            var files = db.UploadFiles.Where(a => a.UserId == id);
            foreach (var file in files)
            {
                db.UploadFiles.Remove(file);
            }
            Profile profile = null;
            foreach(var pr in db.Profiles)
            {
                if(pr.UserId == id)
                {
                    profile = pr;
                }
            }
            if (profile != null) {
                db.Profiles.Remove(profile);
            }
            var comments = db.Comments.Where(comm => comm.UserId == id);
            foreach (var comment in comments)
            {
                db.Comments.Remove(comment);
            }
            List<int> aux = new List<int>();
            foreach (var gr in db.Groups)
            {
                aux.Add(gr.GroupId);
            }
            foreach (var i in aux)
            {
                Group gru = db.Groups.Find(i);
                if (gru != null)
                {
                    gru.Users.Remove(user);
                }
            }
            var groups = db.Groups.Where(gr => gr.UserId == id);
            List<int?> aux2 = new List<int?>();
            foreach (var gr in groups)
            {
                aux2.Add(gr.GroupId);
            }
            foreach (Post post in db.Posts)
            {
                if (aux2.Contains(post.GroupId))
                {
                    db.Posts.Remove(post);
                }
            }
            foreach (var gr in groups)
            {
                db.Groups.Remove(gr);
            }
            user.Groups.Clear();
            var friends = db.Friends.Where(fr => fr.User1_Id == user.Id || fr.User2_Id == user.Id);
            foreach(var fr in friends)
            {
                db.Friends.Remove(fr);
            }
            
            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
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
        private bool ProfileExists(string userId)
        {
            foreach (Profile prof in db.Profiles)
            {
                if (userId == prof.UserId)
                {
                    return true;
                }
            }
            return false;
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
        [NonAction]
        private void AddFriendsButton()
        {
            string currentUser = User.Identity.GetUserId();
            ViewBag.friends = new List<string>();
            ViewBag.friends.Add(currentUser);
            foreach (var fr in db.Friends)
            {
                if (fr.User1_Id == currentUser && (fr.Accepted == true || fr.Pending == true))
                {
                    ViewBag.friends.Add(fr.User2_Id);
                }
                if (fr.User2_Id == currentUser && (fr.Accepted == true || fr.Pending == true))
                {
                    ViewBag.friends.Add(fr.User1_Id);
                }
            }
        }
        [NonAction]
        private void UserFriends(List<string> friends)
        {
            string currentUser = User.Identity.GetUserId();
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