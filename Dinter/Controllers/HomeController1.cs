using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinterDL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DinterDL.Models;

namespace Dinter
{
    // Om du inte är inloggad får du ett felmeddelande om du försöker routa till den här controllern.
    [Authorize]
    public class PersonController : Controller
    {
        private readonly DinterDLContext _dinterContext;

        // Context kommer in via dependency injection
        public PersonController(DinterDLContext DinterContext)
        {
            _dinterContext = DinterContext;
        }

        // Du måste ha rollen admin för att komma åt den här metoden.
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var personEntities = _dinterContext.Users.ToList();

            // Mappa entiteter till vymodeller.
            var users = personEntities.Select(u => new User
            {
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                Password = u.Password
            }).ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            // Eftersom Name-propertyn var nullable känns det vettigt att verifiera ett Name är satt innan vi går vidare.
            if (string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                return RedirectToAction("Error", "Home");
            }

            // Såhär kan man plocka upp den person som har det användarnamn som inloggad användare har.
            //var user = _mvcDemoContext.Persons.SingleOrDefault(p => p.UserName == User.Identity.Name);
            //if (user == null)
            //{
            //    //...
            //}

            // Mappa vymodell till entitet,
            _dinterContext.Users.Add(new DinterDL.Models.User
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Password = user.Password
                //UserName = User.Identity.Name
            });

            // Vid SaveChanges körs SQL:en mot databasen.
            _dinterContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
