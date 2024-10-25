using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using travel.Models;
using Humanizer;

namespace travel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        KarneldbContext db = new KarneldbContext();
        IWebHostEnvironment _webHostEnvironment;
        public HomeController(ILogger<HomeController> logger ,IWebHostEnvironment hc)
        {
            _logger = logger;
            _webHostEnvironment = hc;

        }

        public IActionResult Index()
        {
            List<Guidetb> d1 = db.Guidetbs.ToList();
            List<Servicestb> d2 = db.Servicestbs.ToList();
            List<Processtb> d3 = db.Processtbs.ToList();
            List<Packagetb> d4 = db.Packagetbs.ToList();
            List<Destinationtb> d5 = db.Destinationtbs.ToList();



            ViewBag.guide = d1;
            ViewBag.service = d2;
            ViewBag.process = d3;
            ViewBag.package = d4;
            ViewBag.dest = d5;

            return View();
        }



        // about us page
        [Authorize(Roles = "Member")]
        public IActionResult about()
        {
            List<Guidetb> d1 = db.Guidetbs.ToList();
            ViewBag.guide = d1;


            return View();
        }

     

        // contact page
        [Authorize(Roles = "Member")]
        public IActionResult contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult contact(Mailtb contact , string email)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(email);
            mm.To.Add(new MailAddress("arqamm904@gmail.com"));

            mm.Subject = contact.MailSubject;
            mm.Body = contact.MailMessage;

            mm.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;

            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("arqamm904@gmail.com", "cssn ksyb xzqy fvwk");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.UseDefaultCredentials = false;

            smtp.Send(mm);

            db.Mailtbs.Add(contact);
            db.SaveChanges();

            return View();
        }

        


        [Authorize(Roles = "Member")]
        public IActionResult destination()
        {
            List<Destinationtb> d5 = db.Destinationtbs.ToList();
            ViewBag.dest = d5;

            return View();
        }



        [Authorize(Roles = "Member")]
        public IActionResult packages()
        {
            List<Processtb> d3 = db.Processtbs.ToList();
            List<Packagetb> d4 = db.Packagetbs.ToList();

            ViewBag.process = d3;
            ViewBag.package = d4;

            return View();
        }


        [Authorize(Roles = "Member")]
        public IActionResult services()
        {
            List<Servicestb> d2 = db.Servicestbs.ToList();

            ViewBag.service = d2;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult adminpanel()
        {
            List<Destinationtb> d5 = db.Destinationtbs.ToList();

            ViewBag.dest = d5;

            List<Bookingtb> d6 = db.Bookingtbs.ToList();

            ViewBag.book = d6;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult servicelist()
        {
            var data = db.Servicestbs.ToList();
            return View(data);
        }

        public IActionResult addservice()
        {

            return View();
        }


        [HttpPost]
        public IActionResult addservice(Servicestb add, serimg img)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                if (img.Photo != null)
                {
                    string uploadfolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    filename = Guid.NewGuid().ToString() + "_" + img.Photo.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    img.Photo.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                Servicestb p = new Servicestb
                {
                    Title = img.Title,
                    Description = img.Description,
                    LogoImg = filename
                };
                db.Servicestbs.Add(p);
                db.SaveChanges();
                return RedirectToAction("servicelist");

            }

            return View();
        }

        [HttpGet]
        public IActionResult editservice(int id)
        {
            // Retrieve the existing entity from the database based on its ID
            var existingImage = db.Servicestbs.Find(id);

            if (existingImage == null)
            {
                return NotFound(); // Or handle the case where the entity is not found
            }

            // Map the properties of the entity to your view model (if needed)
            var product1 = new serimg
            {
                ServiceId = existingImage.ServiceId,
                Title = existingImage.Title,
                Description = existingImage.Description

               

                // You may need to populate other properties depending on your requirements
            };


            return View(product1);
        }

        [HttpPost]
        public IActionResult editservice(serimg editedProduct)
        {
            if (ModelState.IsValid)
            {
                Servicestb existingImage = db.Servicestbs.Find(editedProduct.ServiceId);

                if (existingImage == null)
                {
                    return NotFound();
                }

                // Update the properties with the edited values
                existingImage.Title = editedProduct.Title;
                existingImage.Description = editedProduct.Description;

                // Check if a new photo is provided
                if (editedProduct.Photo != null)
                {


                    // Save the new photo
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string newFilename = Guid.NewGuid().ToString() + "_" + editedProduct.Photo.FileName;
                    string newFilePath = Path.Combine(uploadFolder, newFilename);
                    editedProduct.Photo.CopyTo(new FileStream(newFilePath, FileMode.Create));
                    existingImage.LogoImg = newFilename;
                }
               

                // Save changes to the database
                db.SaveChanges();

                ViewBag.success = "Record Updated";

                return RedirectToAction("servicelist"); // Redirect to the list of images or another appropriate action
            }

            // If ModelState is not valid, return to the edit view with the provided data
            return View(editedProduct);
        }


        public IActionResult servicedetail(int? id)
        {
            var user = db.Servicestbs.FirstOrDefault(u => u.ServiceId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult servicedelete(int? id)
        {
            var d = db.Servicestbs.Where(u => u.ServiceId == id).SingleOrDefault();
            return View(d);
        }

        [HttpPost]
        public IActionResult servicedelete(Servicestb t)
        {
            db.Servicestbs.Remove(t);
            db.SaveChanges();
            return RedirectToAction("servicelist");
        }

      



        [Authorize(Roles = "Admin")]
        public IActionResult userlist()
        {
            var data = db.AspNetUsers.ToList();
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult guidelist()
        {
            var data = db.Guidetbs.ToList();
            return View(data);
        }

        public IActionResult addguide()
        {

            return View();
        }


        [HttpPost]
        public IActionResult addguide(Guidetb add, guideimg img)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                if (img.GuidePhoto != null)
                {
                    string uploadfolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    filename = Guid.NewGuid().ToString() + "_" + img.GuidePhoto.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    img.GuidePhoto.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                Guidetb p = new Guidetb
                {
                    GuideName = img.GuideName,
                    GuideDestination = img.GuideDestination,
                    GuideImage = filename
                };
                db.Guidetbs.Add(p);
                db.SaveChanges();
                return RedirectToAction("guidelist");

            }

            return View();
        }


        [HttpGet]
        public IActionResult editguide(int id)
        {
            // Retrieve the existing entity from the database based on its ID
            var existingImage = db.Guidetbs.Find(id);

            if (existingImage == null)
            {
                return NotFound(); // Or handle the case where the entity is not found
            }

            // Map the properties of the entity to your view model (if needed)
            var product1 = new guideimg
            {
                GudieId = existingImage.GudieId,
                GuideName = existingImage.GuideName,
                GuideDestination = existingImage.GuideDestination,
               

                // You may need to populate other properties depending on your requirements
            };


            return View(product1);
        }

        [HttpPost]
        public IActionResult editguide(guideimg editedProduct)
        {
            if (ModelState.IsValid)
            {
                Guidetb existingImage = db.Guidetbs.Find(editedProduct.GudieId);

                if (existingImage == null)
                {
                    return NotFound();
                }

                // Update the properties with the edited values
                existingImage.GuideName = editedProduct.GuideName;
                existingImage.GuideDestination = editedProduct.GuideDestination;

                // Check if a new photo is provided
                if (editedProduct.GuidePhoto != null)
                {


                    // Save the new photo
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string newFilename = Guid.NewGuid().ToString() + "_" + editedProduct.GuidePhoto.FileName;
                    string newFilePath = Path.Combine(uploadFolder, newFilename);
                    editedProduct.GuidePhoto.CopyTo(new FileStream(newFilePath, FileMode.Create));
                    existingImage.GuideImage = newFilename;
                }
               

                // Save changes to the database
                db.SaveChanges();

                ViewBag.success = "Record Updated";

                return RedirectToAction("guidelist"); // Redirect to the list of images or another appropriate action
            }

            // If ModelState is not valid, return to the edit view with the provided data
            return View(editedProduct);
        }

        public IActionResult guidedetail(int? id)
        {
            var user = db.Guidetbs.FirstOrDefault(u => u.GudieId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult guidedelete(int? id)
        {
            var d = db.Guidetbs.Where(u => u.GudieId == id).SingleOrDefault();
            return View(d);
        }

        [HttpPost]
        public IActionResult guidedelete(Guidetb t)
        {
            db.Guidetbs.Remove(t);
            db.SaveChanges();
            return RedirectToAction("guidelist");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult packagelist()
        {
            var data = db.Packagetbs.ToList();
            return View(data);
        }


        public IActionResult addpackage()
        {
            List<Guidetb> d1 = db.Guidetbs.ToList();

            ViewBag.guide = d1;

            return View();
        }


        [HttpPost]
        public IActionResult addpackage(Packagetb add, packimg img , string ActivePackage , string PopularPackage)
        {
            


            if (ModelState.IsValid)
            {
                string filename = "";
                var ac = img.ActivePackage;
                var po = img.PopularPackage;
                if (img.PackagePhoto != null)
                {
                    string uploadfolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    filename = Guid.NewGuid().ToString() + "_" + img.PackagePhoto.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    img.PackagePhoto.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                if(ActivePackage == "true" )
                {
                    
                    ac = true;

                }
                else
                {
                    ac = false;
                }
                if (PopularPackage == "true")
                {

                    po = true;

                }
                else
                {
                    po = false;
                }
                Packagetb p = new Packagetb
                {
                    PackageCountry = img.PackageCountry,
                    PackageGuide = img.PackageGuide,
                    PackageDate1 = img.PackageDate1,
                    PackageDate2 = img.PackageDate2,
                    PackageDate3 = img.PackageDate3,
                    PackagePrice = img.PackagePrice,
                    PackageDays = img.PackageDays,
                    PackageDescription = img.PackageDescription,
                    PackagePerson = img.PackagePerson,
                    PackageImage = filename,
                    ActivePackage = ac,
                    PopularPackage = img.PopularPackage
                };
                db.Packagetbs.Add(p);
                db.SaveChanges();
                return RedirectToAction("packagelist");

            }

            return View();
        }

        [HttpGet]
        public IActionResult editpack(int id)
        {
            // Retrieve the existing entity from the database based on its ID
            var existingImage = db.Packagetbs.Find(id);

            List<Guidetb> d1 = db.Guidetbs.ToList();

            ViewBag.guide = d1;

            if (existingImage == null)
            {
                return NotFound(); // Or handle the case where the entity is not found
            }

            // Map the properties of the entity to your view model (if needed)
            var product1 = new packimg
            {
                PackageId = existingImage.PackageId,
                PackageCountry = existingImage.PackageCountry,
                PackageGuide = existingImage.PackageGuide,
                PackageDate1 = existingImage.PackageDate1,
                PackageDate2 = existingImage.PackageDate2,
                PackageDate3 = existingImage.PackageDate3,
                PackagePrice = existingImage.PackagePrice,
                PackageDays = existingImage.PackageDays,
                PackageDescription = existingImage.PackageDescription,
                PackagePerson = existingImage.PackagePerson,
                ActivePackage = existingImage.ActivePackage,
                PopularPackage = existingImage.PopularPackage

                // You may need to populate other properties depending on your requirements
            };


            return View(product1);
        }

        [HttpPost]
        public IActionResult editpack( packimg editedProduct, string ActivePackage, string PopularPackage)
        {
            if (ModelState.IsValid)
            {
                Packagetb existingImage = db.Packagetbs.Find(editedProduct.PackageId);
                var ac = editedProduct.ActivePackage;
                var po = editedProduct.PopularPackage;

                if (existingImage == null)
                {
                    return NotFound();
                }

                // Update the properties with the edited values
                existingImage.PackageCountry = editedProduct.PackageCountry;
                existingImage.PackageGuide = editedProduct.PackageGuide;
                existingImage.PackageDate1 = editedProduct.PackageDate1;
                existingImage.PackageDate2 = editedProduct.PackageDate2;
                existingImage.PackageDate3 = editedProduct.PackageDate3;
                existingImage.PackageDescription = editedProduct.PackageDescription;
                existingImage.PackageDays = editedProduct.PackageDays;
                existingImage.PackagePerson = editedProduct.PackagePerson;
                existingImage.PackagePrice = editedProduct.PackagePrice;

                // Check if a new photo is provided
                if (editedProduct.PackagePhoto != null)
                {


                    // Save the new photo
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string newFilename = Guid.NewGuid().ToString() + "_" + editedProduct.PackagePhoto.FileName;
                    string newFilePath = Path.Combine(uploadFolder, newFilename);
                    editedProduct.PackagePhoto.CopyTo(new FileStream(newFilePath, FileMode.Create));
                    existingImage.PackageImage = newFilename;
                }
                
                if (ActivePackage == "true")
                {

                    ac = true;
                    existingImage.ActivePackage = ac;

                }
                else
                {
                    ac = false;
                    existingImage.ActivePackage = ac;

                }
                if (PopularPackage == "true")
                {

                    po = true;
                    existingImage.PopularPackage = po;

                }
                else
                {
                    po = false;
                    existingImage.PopularPackage = po;

                }

                // Save changes to the database
                db.SaveChanges();

                ViewBag.success = "Record Updated";

                return RedirectToAction("packagelist"); // Redirect to the list of images or another appropriate action
            }

            // If ModelState is not valid, return to the edit view with the provided data
            return View(editedProduct);
        }


        public IActionResult packagedetail(int? id)
        {
            var user = db.Packagetbs.FirstOrDefault(u => u.PackageId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult packagedelete(int? id)
        {
            var d = db.Packagetbs.Where(u => u.PackageId == id).SingleOrDefault();
            return View(d);
        }

        [HttpPost]
        public IActionResult packagedelete(Packagetb t)
        {
            db.Packagetbs.Remove(t);
            db.SaveChanges();
            return RedirectToAction("packagelist");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult destinationlist()
        {
            var data = db.Destinationtbs.ToList();
            return View(data);
        }
        public IActionResult adddest()
        {
            List<Guidetb> d1 = db.Guidetbs.ToList();

            ViewBag.guide = d1;

            return View();
        }


        [HttpPost]
        public IActionResult adddest(Destinationtb add, adddest img, string ActiveDestination, string PopularDestination)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                var ac = img.ActiveDestination;
                var po = img.PopularDestination;
                if (img.DestinationPhoto != null)
                {
                    string uploadfolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    filename = Guid.NewGuid().ToString() + "_" + img.DestinationPhoto.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    img.DestinationPhoto.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                if (ActiveDestination == "true")
                {

                    ac = true;

                }
                else
                {
                    ac = false;
                }
                if (PopularDestination == "true")
                {

                    po = true;

                }
                else
                {
                    po = false;
                }
                Destinationtb p = new Destinationtb
                {
                    DestinationCountry = img.DestinationCountry,
                    DestinationGuider = img.DestinationGuider,
                    DestinationDate1 =img.DestinationDate1,
                    DestinationDate3 =img.DestinationDate3,
                    DestinationDate2 =img.DestinationDate2,
                    DestinationPrice = img.DestinationPrice,
                    ActiveDestination = img.ActiveDestination,
                    PopularDestination = img.PopularDestination,
                    DestinationImage = filename
                };
                db.Destinationtbs.Add(p);
                db.SaveChanges();
                return RedirectToAction("destinationlist");

            }

            return View();
        }

        [HttpGet]
        public IActionResult editdest(int id)
        {
            // Retrieve the existing entity from the database based on its ID
            var existingImage = db.Destinationtbs.Find(id);

            List<Guidetb> d1 = db.Guidetbs.ToList();

            ViewBag.guide = d1;

            if (existingImage == null)
            {
                return NotFound(); // Or handle the case where the entity is not found
            }

            // Map the properties of the entity to your view model (if needed)
            var product1 = new adddest
            {
                DestinationId = existingImage.DestinationId,
                DestinationCountry = existingImage.DestinationCountry,
                DestinationGuider = existingImage.DestinationGuider,
                DestinationDate1 = existingImage.DestinationDate1,
                DestinationDate3 = existingImage.DestinationDate3,
                DestinationDate2 = existingImage.DestinationDate2,
                DestinationPrice = existingImage.DestinationPrice,
                ActiveDestination = existingImage.ActiveDestination,
                PopularDestination = existingImage.PopularDestination

                // You may need to populate other properties depending on your requirements
            };


            return View(product1);
        }

        [HttpPost]
        public IActionResult editdest(adddest editedProduct, string ActiveDestination, string PopularDestination)
        {
            if (ModelState.IsValid)
            {
                Destinationtb existingImage = db.Destinationtbs.Find(editedProduct.DestinationId);
                var ac = editedProduct.ActiveDestination;
                var po = editedProduct.PopularDestination;

                if (existingImage == null)
                {
                    return NotFound();
                }

                // Update the properties with the edited values
                 existingImage.DestinationCountry = editedProduct.DestinationCountry;
                existingImage.DestinationGuider = editedProduct.DestinationGuider;
                existingImage.DestinationDate1 = editedProduct.DestinationDate1;
                existingImage.DestinationDate3 = editedProduct.DestinationDate3;
                existingImage.DestinationDate2 = editedProduct.DestinationDate2;
                existingImage.DestinationPrice = editedProduct.DestinationPrice;

                // Check if a new photo is provided
                if (editedProduct.DestinationPhoto != null)
                {


                    // Save the new photo
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string newFilename = Guid.NewGuid().ToString() + "_" + editedProduct.DestinationPhoto.FileName;
                    string newFilePath = Path.Combine(uploadFolder, newFilename);
                    editedProduct.DestinationPhoto.CopyTo(new FileStream(newFilePath, FileMode.Create));
                    existingImage.DestinationImage = newFilename;
                }
                
                if (ActiveDestination == "true")
                {

                    ac = true;
                    existingImage.ActiveDestination = ac;

                }
                else
                {
                    ac = false;
                    existingImage.ActiveDestination = ac;

                }
                if (PopularDestination == "true")
                {

                    po = true;
                    existingImage.PopularDestination = po;

                }
                else
                {
                    po = false;
                    existingImage.PopularDestination = po;

                }

                // Save changes to the database
                db.SaveChanges();

                ViewBag.success = "Record Updated";

                return RedirectToAction("destinationlist"); // Redirect to the list of images or another appropriate action
            }

            // If ModelState is not valid, return to the edit view with the provided data
            return View(editedProduct);
        }


        public IActionResult destdetail(int? id)
        {
            var user = db.Destinationtbs.FirstOrDefault(u => u.DestinationId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult destdelete(int? id)
        {
            var d = db.Destinationtbs.Where(u => u.DestinationId == id).SingleOrDefault();
            return View(d);
        }

        [HttpPost]
        public IActionResult destdelete(Destinationtb t)
        {
            db.Destinationtbs.Remove(t);
            db.SaveChanges();
            return RedirectToAction("destinationlist");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult processlist()
        {
            var data = db.Processtbs.ToList();
            return View(data);
        }

        public IActionResult addprocess()
        {

            return View();
        }


        [HttpPost]
        public IActionResult addprocess(Processtb add, proimg img)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                if (img.ProcessPhoto != null)
                {
                    string uploadfolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    filename = Guid.NewGuid().ToString() + "_" + img.ProcessPhoto.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    img.ProcessPhoto.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                Processtb p = new Processtb
                {
                    ProcessName = img.ProcessName,
                    ProcessDescription = img.ProcessDescription,
                    ProcessImage = filename
                };
                db.Processtbs.Add(p);
                db.SaveChanges();
                return RedirectToAction("processlist");

            }

            return View();
        }

        [HttpGet]
        public IActionResult editpro(int id)
        {
            // Retrieve the existing entity from the database based on its ID
            var existingImage = db.Processtbs.Find(id);

            if (existingImage == null)
            {
                return NotFound(); // Or handle the case where the entity is not found
            }

            // Map the properties of the entity to your view model (if needed)
            var product1 = new proimg
            {
                ProcessId = existingImage.ProcessId,
                ProcessName = existingImage.ProcessName,
                ProcessDescription = existingImage.ProcessDescription,
               

                // You may need to populate other properties depending on your requirements
            };


            return View(product1);
        }

        [HttpPost]
        public IActionResult editpro(proimg editedProduct)
        {
            if (ModelState.IsValid)
            {
                Processtb existingImage = db.Processtbs.Find(editedProduct.ProcessId);

                if (existingImage == null)
                {
                    return NotFound();
                }

                // Update the properties with the edited values
                existingImage.ProcessName = editedProduct.ProcessName;
                existingImage.ProcessDescription = editedProduct.ProcessDescription;

                // Check if a new photo is provided
                if (editedProduct.ProcessPhoto != null)
                {


                    // Save the new photo
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string newFilename = Guid.NewGuid().ToString() + "_" + editedProduct.ProcessPhoto.FileName;
                    string newFilePath = Path.Combine(uploadFolder, newFilename);
                    editedProduct.ProcessPhoto.CopyTo(new FileStream(newFilePath, FileMode.Create));
                    existingImage.ProcessImage = newFilename;
                }
               

                // Save changes to the database
                db.SaveChanges();

                ViewBag.success = "Record Updated";

                return RedirectToAction("processlist"); // Redirect to the list of images or another appropriate action
            }

            // If ModelState is not valid, return to the edit view with the provided data
            return View(editedProduct);
        }

        public IActionResult processdetail(int? id)
        {
            var user = db.Processtbs.FirstOrDefault(u => u.ProcessId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult processdelete(int? id)
        {
            var d = db.Processtbs.Where(u => u.ProcessId == id).SingleOrDefault();
            return View(d);
        }

        [HttpPost]
        public IActionResult processdelete(Processtb t)
        {
            db.Processtbs.Remove(t);
            db.SaveChanges();
            return RedirectToAction("processlist");
        }

        public IActionResult booklist()
        {
            var data = db.Bookingtbs.ToList();
            return View(data);
        }

        public IActionResult bookdetail(int? id)
        {
            var user = db.Bookingtbs.FirstOrDefault(u => u.BookId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult addbook(int? id)
        {
            
            

            Destinationtb existingImage = db.Destinationtbs.Find(id);
            ViewBag.s = existingImage;

            

            return View();

        }

        [HttpPost]
        public IActionResult addbook(Bookingtb addo)
        {
            
            if (ModelState.IsValid)
            {
                db.Bookingtbs.Add(addo);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Your order is booked successfully.";

                return RedirectToAction("bookuser");
            }
            return View(); 
        }

        public IActionResult book(int? id)
        {



            Packagetb existingImage = db.Packagetbs.Find(id);
            ViewBag.s = existingImage;



            return View();

        }

        [HttpPost]
        public IActionResult book(Bookingtb addo)
        {

            if (ModelState.IsValid)
            {
                db.Bookingtbs.Add(addo);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Your order is booked successfully.";
                return RedirectToAction("bookuser");
            }
            return View();
        }



        [HttpPost]
        public IActionResult SetPending(int requestId)
        {
            var request = db.Bookingtbs.Find(requestId);
            if (request != null)
            {
                request.Status = "pending";
                db.SaveChanges();
            }
            return RedirectToAction("booklist");
        }

        [HttpPost]
        public IActionResult RejectRequest(int requestId)
        {
            var request = db.Bookingtbs.Find(requestId);
            if (request != null)
            {
                request.Status = "Rejected";
                db.SaveChanges();
            }
            return RedirectToAction("booklist");
        }

        [HttpPost]
        public IActionResult ApproveRequest(int requestId)
        {
            var request = db.Bookingtbs.Find(requestId);
            if (request != null)
            {
                request.Status = "Approved";
                db.SaveChanges();
            }
            return RedirectToAction("booklist");
        }

        public IActionResult bookuser()
        {
            var data = db.Bookingtbs.ToList();

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
