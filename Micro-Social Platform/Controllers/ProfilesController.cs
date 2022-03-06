using Micro_Social_Platform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Micro_Social_Platform.Controllers
{
    public class ProfilesController : Controller
    {
        private Micro_Social_Platform.Models.ApplicationDbContext db = new Micro_Social_Platform.Models.ApplicationDbContext();

        // GET: INDEX
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index(string search)
        {
            NrRequests();
            List<string> friends = new List<string>();
            UserFriends(friends);
            ViewBag.friends = friends;
            if (search != null)
            {
                search = search.Trim();
            }
            bool ok = User.IsInRole("Admin");
            var profiles = db.Profiles.Where(x => (x.Name.ToUpper().Contains(search.ToUpper()) || search == null ) && ((x.IsPublic && friends.Contains(x.UserId)) || ok));
            ViewBag.profiles = profiles;
            ViewBag.count = profiles.Count();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
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
            Profil();
            return View();
        }

        // GET: SHOW
        public ActionResult Show(int id)
        {
            NrRequests();
            List<string> friends = new List<string>();
            UserFriends(friends);
            ViewBag.friends = friends;
            Profile profile = db.Profiles.Find(id);
            ViewBag.file = null;
            foreach (var fl in db.UploadFiles)
            {
                if (fl.UserId == profile.UserId)
                {
                    ViewBag.file = fl;
                }
            }
            ViewBag.posts = db.Posts.Include("User").Include("Group");
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
            Profil();
            SetAccessRight();
            try
            {
                if (profile.IsPublic || profile.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (friends.Contains(profile.UserId) || User.IsInRole("Admin"))
                    {
                        return View(profile);
                    }
                    else
                    {
                        TempData["message"] = "You are not friends.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["message"] = "The profile is private.";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //GET: NEW
        [Authorize(Roles = "User,Admin")]
        public ActionResult New()
        {
            NrRequests();
            Profil();
            int ok = 1;
            foreach (var prof in db.Profiles)
            {
                if(prof.UserId == User.Identity.GetUserId())
                {
                    ok = 0;
                }
            }
            if (ok == 1)
            {
                Profile profile = new Profile();
                profile.UserId = User.Identity.GetUserId();
                return View(profile);
            }
            return RedirectToAction("Index", "Posts");
        }

        //POST: NEW
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(Profile profile, HttpPostedFileBase uploadedFile)
        {
            NrRequests();
            Profil();
            profile.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid && ProfileExists(profile.UserId) == false)
                {
                    db.Profiles.Add(profile);
                    db.SaveChanges();
                    TempData["message"] = "The profile has been created.";
                }
                else
                {
                    return View(profile);
                }
            }
            catch (DbEntityValidationException e)
            {
                return View(profile);
            }
            try
            {
                if (uploadedFile != null)
                {
                    // Se preia numele fisierul
                    string uploadedFileName = uploadedFile.FileName;
                    string uploadedFileExtension = Path.GetExtension(uploadedFileName);

                    // Se poate verifica daca extensia este intr-o lista dorita
                    if (uploadedFileExtension == ".png" || uploadedFileExtension == ".jpg" || uploadedFileExtension == ".pdf")
                    {
                        // Se stocheaza fisierul in folderul Files (folderul trebuie creat in proiect)

                        // 1. Se seteaza calea folderului de upload
                        string uploadFolderPath = Server.MapPath("~//Files//");

                        // 2. Se salveaza fisierul in acel folder
                        uploadedFile.SaveAs(uploadFolderPath + uploadedFileName);

                        // 3. Se face o instanta de model si se populeaza cu datele necesare
                        UploadFile file = new UploadFile();
                        file.UserId = User.Identity.GetUserId();
                        file.Extension = uploadedFileExtension;
                        file.FileName = uploadedFileName;
                        file.FilePath = uploadFolderPath + uploadedFileName;

                        // 4. Se adauga modelul in baza de date
                        db.UploadFiles.Add(file);
                        db.SaveChanges();
                        // 5. Return catre index cu mesaj de succes
                        TempData["message"] = "The profile photo has been added.";
                    }
                    else
                    {
                        TempData["message"] = "not a photo";
                        return Redirect("/Profiles/Show/" + profile.ProfileId);
                    }
                }
                else
                {
                    TempData["message"] = "no photo";
                    return Redirect("/Profiles/Show/" + profile.ProfileId);
                }
            }
            catch (DbEntityValidationException e)
            {
                return View(profile);
            }
            return Redirect("/Profiles/Show/" + profile.ProfileId);
        }

        //GET: PROFILE PHOTO
        [Authorize(Roles = "User,Admin")]
        public ActionResult ProfilePhoto(int id)
        {
            var profile = db.Profiles.Find(id);
            ViewBag.ProfileId = profile.ProfileId;
            if (profile.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                TempData["message"] = "You are not allowed to edit this profile.";
                return RedirectToAction("Index");
            }
        }

        //POST: PROFILE PHOTO
        [HttpPost]
        public ActionResult ProfilePhoto(int id, HttpPostedFileBase uploadedFile)
        {
            Profile profile = db.Profiles.Find(id);
            if (profile.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                if (uploadedFile != null)
                {
                    // Se preia numele fisierul
                    string uploadedFileName = uploadedFile.FileName;
                    string uploadedFileExtension = Path.GetExtension(uploadedFileName);

                    // Se poate verifica daca extensia este intr-o lista dorita
                    if (uploadedFileExtension == ".png" || uploadedFileExtension == ".jpg" || uploadedFileExtension == ".pdf")
                    {
                        // Se stocheaza fisierul in folderul Files (folderul trebuie creat in proiect)

                        // 1. Se seteaza calea folderului de upload
                        string uploadFolderPath = Server.MapPath("~//Files//");

                        // 2. Se salveaza fisierul in acel folder
                        uploadedFile.SaveAs(uploadFolderPath + uploadedFileName);

                        // 3. Se face o instanta de model si se populeaza cu datele necesare
                        UploadFile file = new UploadFile();
                        var prof = db.Profiles.Find(id);
                        file.UserId = prof.UserId;
                        file.Extension = uploadedFileExtension;
                        file.FileName = uploadedFileName;
                        file.FilePath = uploadFolderPath + uploadedFileName;

                        // 4. Se adauga modelul in baza de date
                        db.UploadFiles.Add(file);
                        db.SaveChanges();

                        // 5. Return catre index cu mesaj de succes
                        TempData["message"] = "The profile photo has been added.";
                        return Redirect("/Profiles/Show/" + profile.ProfileId);
                    }
                    else
                    {
                        TempData["message"] = "Not a photo.";
                        return Redirect("/Profiles/Show/" + profile.ProfileId);
                    }
                }
                else
                {
                    TempData["message"] = "No photo.";
                    return Redirect("/Profiles/Show/" + profile.ProfileId);
                }
            }
            else
            {
                TempData["message"] = "You are not allowed to edit this profile.";
                return RedirectToAction("Index");
            }
        }

        //DELETE: DELETE PROFILE PHOTO
        [HttpDelete]
        public ActionResult DeleteProfilePhoto(int id)
        {
            Profile profile = db.Profiles.Find(id);
            if (profile.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var userId = db.Profiles.Find(id).UserId;
                int? fileId = null;
                foreach (var f in db.UploadFiles)
                {
                    if (f.UserId == userId)
                    {
                        fileId = f.FileId;
                    }
                }
                if (fileId != null)
                {
                    UploadFile file = db.UploadFiles.Find(fileId);
                    db.UploadFiles.Remove(file);
                    db.SaveChanges();
                }
                return Redirect("/Profiles/Show/" + profile.ProfileId);
            }
            else
            {
                TempData["message"] = "You are not allowed to edit this profile.";
                return RedirectToAction("Index");
            }
        }

        //GET: EDIT
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            NrRequests();
            Profil();
            Profile profile = db.Profiles.Find(id);
            if (profile.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(profile);
            }
            else
            {
                TempData["message"] = "You are not allowed to edit this profile.";
                return RedirectToAction("Index");
            }
        }

        //PUT: EDIT
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Profile requestProfile)
        {
            NrRequests();
            Profil();
            try
            {
                if (ModelState.IsValid)
                {
                    Profile profile = db.Profiles.Find(id);
                    if (profile.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(profile))
                        {
                            profile.Name = requestProfile.Name;
                            profile.Description = requestProfile.Description;
                            db.SaveChanges();
                            TempData["message"] = "The profile has been edited.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return View(requestProfile);
                        }
                    }
                    else
                    {
                        TempData["message"] = "You are not allowed to edit this profile.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestProfile);
                }
            }
            catch (Exception)
            {
                return View(requestProfile);
            }
        }

        //DELETE: DELETE
        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            NrRequests();
            Profile profile = db.Profiles.Find(id);
            if (profile.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var files = db.UploadFiles.Where(a => a.UserId == profile.UserId);
                foreach (var file in files)
                {
                    db.UploadFiles.Remove(file);
                }
                db.Profiles.Remove(profile);
                db.SaveChanges();
                TempData["message"] = "The profile has been deleted.";
                Profil();
                return RedirectToAction("Index");
            }
            else
            {
                Profil();
                TempData["message"] = "You are not allowed to delete this profile.";
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
        private bool ProfileExists(string userId)
        {
            foreach(Profile prof in db.Profiles)
            {
                if(userId == prof.UserId)
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