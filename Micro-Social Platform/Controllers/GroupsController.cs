using Micro_Social_Platform.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Micro_Social_Platform.Controllers
{
    public class GroupsController : Controller
    {
        private Micro_Social_Platform.Models.ApplicationDbContext db = new Micro_Social_Platform.Models.ApplicationDbContext();
        
        // GET: Groups
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            NrRequests();
            List<string> friends = new List<string>();
            UserFriends(friends);
            ViewBag.friends = friends;
            ViewBag.UsersProfiles = db.Profiles;
            Profil();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            List<Group> groups = new List<Group>();
            foreach(var gr in db.Groups)
            {
                groups.Add(gr);
            }
            string currentUser = User.Identity.GetUserId();
            List<Group> aux = new List<Group>();
            var groupsUser = db.Groups.Where(g => g.Users.Any(u => u.Id == currentUser));
            foreach(var gru in groupsUser)
            {
                aux.Add(gru);
            }
            ApplicationUser user = db.Users.Find(currentUser);
            ViewBag.groupsUser = user.Groups;
            ViewBag.Groups = groups;
            return View();
        }

        //GET: SHOW
        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(int? id)
        {
            NrRequests();
            List<string> friends = new List<string>();
            UserFriends(friends);
            ViewBag.friends = friends;
            ViewBag.UsersProfiles = db.Profiles;
            Profil();
            Group group = db.Groups.Find(id);
            if (id != null)
            {
                List<Post> posts = new List<Post>();
                foreach (var post in db.Posts)
                {
                    if(post.GroupId == id)
                    {
                        posts.Add(post);
                    }
                }
                ViewBag.Posts = posts;
            }
            List<String> noPhoto = new List<string>();
            List<UploadFile> files = new List<UploadFile>();
            List<String> ids = new List<string>();
            foreach (var user in db.Users)
            {
                ids.Add(user.Id);
            }
            foreach (var idd in ids)
            {
                UploadFile file = null;
                foreach (var f in db.UploadFiles)
                {
                    if (f.UserId == idd)
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
                    noPhoto.Add(idd);
                }
            }
            ViewBag.noPhoto = noPhoto;
            ViewBag.files = files;
            SetAccessRight();
            string currentUser = User.Identity.GetUserId();
            List<Group> aux = new List<Group>();
            var groupsUser = db.Groups.Where(g => g.Users.Any(u => u.Id == currentUser));
            foreach (var gru in groupsUser)
            {
                aux.Add(gru);
            }
            if (aux.Contains(group) || User.IsInRole("Admin"))
            {
                return View(group);
            }
            else
            {
                TempData["message"] = "You are not in this group";
                return RedirectToAction("Index");
            }

        }

        //GET:  NEW
        [Authorize(Roles = "User,Admin")]
        public ActionResult New()
        {
            NrRequests();
            Profil();
            Group group = new Group();
            group.UserId = User.Identity.GetUserId();
            return View(group);
        }

        //POST: NEW
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(Group gr)
        {
            NrRequests();
            Profil();
            gr.Date = DateTime.Now;
            gr.UserId = User.Identity.GetUserId();
            //ApplicationDbContext context = new ApplicationDbContext();
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var user = UserManager.Users.FirstOrDefault(u => u.Id == gr.UserId);
            //gr.Users.Add(user);
            try
            {
                if (ModelState.IsValid)
                {
                    db.Groups.Add(gr);
                    db.SaveChanges();
                    TempData["message"] = "The Group has been added";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(gr);
                }
            }
            catch (Exception)
            {
                return View(gr);
            }
        }

        //GET: EDIT
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            NrRequests();
            Profil();
            Group group = db.Groups.Find(id);
            group.UserId = User.Identity.GetUserId();
            if (group.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(group);
            }
            else
            {
                TempData["message"] = "You are not allowed to edit this group.";
                return RedirectToAction("Index");
            }
  
        }

        //PUT: EDIT
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Group requestGroup)
        {
            NrRequests();
            Profil();
            try
            {
                if (ModelState.IsValid)
                {
                    Group group = db.Groups.Find(id);
                    if (group.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {

                        if (TryUpdateModel(group))
                        {
                            group.Name = requestGroup.Name;
                            group.Description = requestGroup.Description;
                            db.SaveChanges();
                            TempData["message"] = "Group was Edited!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return View(requestGroup);
                        }
                    }
                    else
                    {
                        TempData["message"] = "You are not allowed to edit this group.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestGroup);
                }
            }
            catch (Exception)
            {
                return View(requestGroup);
            }
        }

        //DELETE: DELETE
        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            NrRequests();
            Profil();
            Group group = db.Groups.Find(id);
            foreach(Post post in db.Posts)
            {
                if(post.GroupId == id)
                {
                    db.Posts.Remove(post);
                }
            }
            group.Users.Clear();
            db.Groups.Remove(group);
            db.SaveChanges();
            TempData["message"] = "The group was deleted";
            return RedirectToAction("Index");
        }

        //POST: AddUser
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult AddUser(int id)
        {
            NrRequests();
            Profil();
            string currentUser = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(currentUser);
            Group group = db.Groups.Find(id);
            if (!(group.Users.Contains(user)))
            {
                group.Users.Add(user);
                user.Groups.Add(group);
                db.SaveChanges();
            }
            else
            {
                TempData["message"] = "Userul este deja in grup";
            }
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