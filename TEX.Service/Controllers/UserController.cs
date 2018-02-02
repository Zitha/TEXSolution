using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TEX.Model;
using Text.Data;

namespace TEX.Service.Controllers
{
    public class UserController : ApiController
    {
        EncryptString encription = new EncryptString();

        [HttpPut]
        public User Login(string email, string password)
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                User loginUser = unit.Users.GetAll().Include(a => a.Address)
                    .FirstOrDefault(user => user.Email.ToLower() == email.ToLower());

                if (loginUser != null)
                {
                    string decriptedPassword = EncryptString.Decrypt(loginUser.Password, "TEX");
                    if (decriptedPassword == password)
                    {
                        return loginUser;
                    }
                }
                return null;
            }
        }

        [HttpPut]
        public bool ResertPassword(string email)
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                User foundUser = unit.Users.GetAll().FirstOrDefault(user => user.Email.ToLower() == email.ToLower());

                if (foundUser != null)
                {
                    string decriptedPassword = EncryptString.Decrypt(foundUser.Password, "TEX");
                    EmailService emailService = new EmailService();

                    //Send Email with Invoice
                    Task sendEmailTask = new Task(() => emailService.SendResertPasswordEmail(foundUser.Email, decriptedPassword));
                    // Start the task.
                    sendEmailTask.Start();

                    return true;
                }
                return false;
            }
        }

        [HttpPost]
        public User CreateAccount(User user)
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                User hasUser = unit.Users.GetAll().FirstOrDefault(u => u.Email.ToLower() == user.Email.ToLower());
                if (hasUser == null)
                {
                    string password = EncryptString.Encrypt(user.Password, "TEX");
                    user.Password = password;
                    user.Role = "Customer";
                    unit.Users.Add(user);
                    unit.SaveChanges();
                    EmailService emailService = new EmailService();
                    //Send Email with Invoice
                    Task sendEmailTask = new Task(() => emailService.SendWelcomeEmail(user.Email));
                    // Start the task.
                    sendEmailTask.Start();

                    return user;
                }
                return null;
            }
        }

        [HttpPut]
        public Address UpdateAddress(User user)
        {
            try
            {
                using (ApplicationUnit unit = new ApplicationUnit())
                {
                    if (user.Address != null && user.Address.Id > 0)
                    {
                        Address addressToUpdate = unit.Addresses.GetAll().FirstOrDefault(a => a.Id == user.Address.Id);

                        addressToUpdate.PostalCode = user.Address.PostalCode;
                        addressToUpdate.StreetAddress = user.Address.StreetAddress;
                        addressToUpdate.Surburb = user.Address.Surburb;
                        addressToUpdate.ContactNumber = user.Address.ContactNumber;
                        addressToUpdate.CityTown = user.Address.CityTown;
                        addressToUpdate.BuildingName = user.Address.BuildingName;

                        unit.Addresses.Update(addressToUpdate);
                        unit.SaveChanges();

                        return addressToUpdate;
                    }
                    User userToUpdate = unit.Users.GetAll().FirstOrDefault(u => user.Id == u.Id);

                    userToUpdate.Address = user.Address;
                    unit.Users.Update(userToUpdate);
                    unit.SaveChanges();

                    return user.Address;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
