using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        ContactsRepository ContactsRepository;

        public ContactsController() => ContactsRepository = new ContactsRepository();

        // GET api/Contacts/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            try
            {
                var guid = Guid.Parse(id);
                var contact = ContactsRepository.Get(guid);
                if (contact != null)
                {
                    var contactModel = GetContactModel(contact);
                    return Content(contactModel.ToString());
                }
                return "Not Found";
            }
            catch
            {
                return "Id is Incorrect";
            }
        }

        [HttpGet("GetProfilePicture/{id}")]
        public ActionResult<string> GetProfilePicture(string id)
        {
            Guid guid;
            if (Guid.TryParse(id, out guid))
            {
                var contact = ContactsRepository.Get(guid);
                if (contact != null)
                    return File(contact.ProfilePicture, contact.ProfilePictureType);
            }
            return "value";
        }

        // POST api/Contacts/Create
        [HttpPost("Create")]
        public ObjectResult Create([FromBody] ContactViewModel contact)
        {
            CheckModel(contact);
            if (ModelState.IsValid)
            {
                var id = Guid.NewGuid();
                var entity = GetContact(contact, id);
                ContactsRepository.Create(entity);
                return StatusCode((int)HttpStatusCode.Created,id.ToString());
            }
            return BadRequest(ModelState);
        }

        [HttpPost("UploadProfilePicture/{id}")]
        public void UploadProfilePicture(string id)
        {
            var files = Request.Form.Files;
            Guid guid;
            if (Guid.TryParse(id, out guid))
            {
                var contact = ContactsRepository.Get(guid);

                foreach (var image in files)
                {
                    

                    if (image != null)

                    {
                        if (image.Length > 0)
                        //Convert Image to byte and save to database
                        {

                            byte[] imagebytes = null;
                            using (var fileStream = image.OpenReadStream())
                            using (var memoryStream = new MemoryStream())
                            {
                                fileStream.CopyTo(memoryStream);
                                imagebytes = memoryStream.ToArray();
                            }
                            contact.ProfilePicture = imagebytes;
                            contact.ProfilePictureType = image.ContentType;

                            ContactsRepository.Update(contact);
                        }
                    }
                }
            }
        }

        // PUT api/Contacts/Update/5
        [HttpPut("/Update/{id}")]
        public ActionResult Update(string id, [FromBody] ContactViewModel contact)
        {
            CheckModel(contact);
            if (ModelState.IsValid)
            {
                Guid guid;
                if (Guid.TryParse(id, out guid))
                {
                    var entity = GetContact(contact, guid);
                    ContactsRepository.Update(entity);
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }

        // DELETE api/Contacts/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            Guid guid;
            if (Guid.TryParse(id, out guid))
            {
                var contact = ContactsRepository.Get(guid);
                ContactsRepository.Delete(contact);
            }
        }

        [HttpGet("GetByEmail")]
        public ActionResult GetByEmail(string email)
        {
            var contact = ContactsRepository.FindByCondition(x => x.Email == email).FirstOrDefault();
            var contactModel = GetContactModel(contact);
            return new JsonResult(contactModel);
        }

        [HttpGet("GetContactsByCity")]
        public ActionResult GetByCity(string city)
        {
            var contacts = ContactsRepository.FindByCondition(x => x.City == city)
                .Select(x => GetContactModel(x));
            return new JsonResult(contacts);
        }

        [HttpGet("GetContactsByState")]
        public ActionResult GetByState(string state)
        {
            var contacts = ContactsRepository.FindByCondition(x => x.State == state)
                .Select(x => GetContactModel(x));
            return new JsonResult(contacts);
        }

        [HttpGet("GetByPhone")]
        public ActionResult GetByPhone(string phone, bool isWorkPhone)
        {

            var contact = ContactsRepository.FindByCondition(x => (isWorkPhone? x.WorkPhone: x.PersonalPhone) == phone).FirstOrDefault();
            var contactModel = GetContactModel(contact);
            return new JsonResult(contactModel);
        }

        private ContactViewModel GetContactModel(Contact contact)
        {
            return new ContactViewModel()
            {
                Name = contact.Name,
                Email = contact.Email,
                Company = contact.Company,
                WorkPhone = contact.WorkPhone,
                PersonalPhone = contact.PersonalPhone,
                Birthday = contact.Birthday.ToShortDateString(),
                Address = contact.Address,
                City = contact.City,
                State = contact.State
            };
        }

        private Contact GetContact(ContactViewModel contact, Guid id )
        {
            return new Contact()
            {
                Id = id,
                Name = contact.Name,
                Email = contact.Email,
                Company = contact.Company,
                WorkPhone = contact.WorkPhone,
                PersonalPhone = contact.PersonalPhone,
                Birthday = DateTime.Parse(contact.Birthday),
                Address = contact.Address,
                City = contact.City,
                State = contact.State
            };
        }

        private void CheckModel(ContactViewModel model)
        {
            if (!IsEmailValid(model.Email))
                ModelState.AddModelError("Email", "The email format is not valid");
            DateTime date;
            if(!DateTime.TryParse(model.Birthday, out date))
            {
                ModelState.AddModelError("Birthday", "The date format is not valid");
            }
            if (!IsPhoneNumber(model.WorkPhone)) {
                ModelState.AddModelError("WorkPhone", "The work phone format is not valid");
            }
            if (!IsPhoneNumber(model.PersonalPhone))
            {
                ModelState.AddModelError("PersonalPhone", "The personal phone format is not valid");
            }
        }

        public bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^\d+$").Success;
        }
    }
}
