using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserContext db;
        public UsersController(UserContext context)
        {
            db = context;
            if (!db.Users.Any())
            {
                db.Users.Add(new User
                {
                    Login = "AdminUser1",
                    Password = "123456",
                    Name = "John",
                    Gender = 0,
                    Birthday = null,
                    Admin = true,
                    CreatedOn = DateTime.Now,
                    CreatedBy = "",
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = "",
                    RevokedOn = null,
                    RevokedBy = ""
                });
                db.SaveChanges();
            }
        }

        //        Create
        //DONE!
        //todo 1) Создание пользователя по логину, паролю, имени, полу и дате рождения + указание будет ли
        //пользователь админом(Доступно Админам)
        // POST api/users
        [HttpPost]
        public async Task<ActionResult> Post(UserDTO user, string login, string password)
        {
            User creator = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (user == null && !creator.Admin)
            {
                return BadRequest();
            }
            User newUser = new User
            {
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = creator.Admin == true && user.Admin,
                CreatedOn = DateTime.Now,
                CreatedBy = creator.Login,
                ModifiedOn = DateTime.Now,
                ModifiedBy = creator.Login,
                RevokedOn = null,
                RevokedBy = ""
            };
            db.Users.Add(newUser);
            await db.SaveChangesAsync();
            return Ok(newUser);
        }
        //Update-1
        //DONE!
        // 2) Изменение имени, пола или даты рождения пользователя(Может менять Администратор, либо
        //лично пользователь, если он активен (отсутствует RevokedOn))
        [HttpPut("UpdateInfo")]
        public async Task<ActionResult> UpdateInfo(UserDTO user, string login, string password, string name, int gender, string birthday)
        {
            User changer = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            User oldUser = await db.Users.FirstOrDefaultAsync(x => x.Login == user.Login);
            if (oldUser == null)
            {
                return NotFound("пользователь не найден");
            }
            if (!changer.Admin)
            {
                return BadRequest("вы не администратор");
            }
            if (changer == null || (changer.RevokedOn != null && changer.Login == user.Login))
            {
                return BadRequest("вы не можете изменять данные");
            }


            oldUser.Name = name ?? oldUser.Name;
            oldUser.Gender = gender != 0 || gender != 1 || gender != 2 ? oldUser.Gender : gender;
            oldUser.Birthday = birthday != null ? DateTime.Parse(birthday) : oldUser.Birthday;
            oldUser.ModifiedOn = DateTime.Now;
            oldUser.ModifiedBy = login;

            db.Update(oldUser);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        //done 3) Изменение пароля(Пароль может менять либо Администратор, либо лично пользователь, если
        //он активен (отсутствует RevokedOn))
        [HttpPut("UpdatePassword")]
        public async Task<ActionResult> UpdatePassword(string userLogin, string newPassword, string login, string password)
        {
            User changer = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            User oldUser = await db.Users.FirstOrDefaultAsync(x => x.Login == userLogin);
            if (changer == null || oldUser == null || (changer.RevokedOn != null && changer.Login != userLogin) || !changer.Admin)
            {
                return BadRequest();
            }

            oldUser.Password = newPassword;

            db.Update(oldUser);
            await db.SaveChangesAsync();
            return Ok(oldUser);
        }

        //done 4) Изменение логина(Логин может менять либо Администратор, либо лично пользователь, если
        //он активен (отсутствует RevokedOn), логин должен оставаться уникальным)
        [HttpPut("UpdateLogin/{newLogin}")]
        public async Task<ActionResult> UpdateLogin(string newLogin, string oldLogin, string login, string password)
        {
            User changer = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            User oldUser = await db.Users.FirstOrDefaultAsync(x => x.Login == oldLogin);
            if (changer == null || oldUser == null || (changer.RevokedOn != null && changer.Login != oldLogin) || !changer.Admin)
            {
                return BadRequest();
            }
            if (db.Users.Any(x => x.Login == newLogin))
            {
                return BadRequest("Такой логин уже существует");
            }
            oldUser.Login = newLogin;
            db.Update(oldUser);
            await db.SaveChangesAsync();
            return Ok(oldUser);
        }

        //Read 
        //done 5) Запрос списка всех активных(отсутствует RevokedOn) пользователей, список отсортирован по
        //CreatedOn(Доступно Админам)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get(string login, string password)
        {
            User asker = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (!asker.Admin || asker == null)
            { return BadRequest("You are not admin"); }
            return await db.Users.Where(x => x.RevokedOn == null).OrderBy(x => x.CreatedOn).ToListAsync();
        }
        //done 6) Запрос пользователя по логину, в списке долны быть имя, пол и дата рождения статус активный
        //или нет(Доступно Админам)
        // GET api/users/5
        [HttpGet("login/{userLogin}")]
        public async Task<ActionResult<User>> Get(string userLogin, string login, string password)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Login == userLogin);
            User asker = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (!asker.Admin || asker == null)
            {
                return BadRequest("You are not admin");
            }
            if (user == null)
            {
                return NotFound();
            }

            bool active = user.RevokedOn == null;
            return new ObjectResult(new
            {
                user.Name,
                user.Gender,
                user.Birthday,
                active
            });
        }

        //done 7) Запрос пользователя по логину и паролю(Доступно только самому пользователю, если он
        //активен (отсутствует RevokedOn))
        [HttpGet("loginpass")]
        public async Task<ActionResult<User>> GetOlderThan(string login, string password)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (user == null)
            {
                return BadRequest();
            }
            if (user.RevokedOn != null)
            {
                return BadRequest("User is not active");
            }
            return new ObjectResult(user);
        }

        //DONE
        //8) Запрос всех пользователей старше определённого возраста(Доступно Админам)
        [HttpGet("age/{age}")]
        public async Task<ActionResult<IEnumerable<User>>> GetOlderThan(int age, string login, string password)
        {

            User changer = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            var date = new DateTime(DateTime.Today.Year - age, DateTime.Today.Month, DateTime.Today.Day);
            if (changer == null && !changer.Admin)
            {
                return BadRequest();
            }
            return await db.Users.Where(x => x.Birthday != null && DateTime.Compare(date, (DateTime)x.Birthday) >= 0).ToListAsync();

        }




        //Delete
        //done 9) Удаление пользователя по логину полное или мягкое(При мягком удалении должна
        //происходить простановка RevokedOn и RevokedBy) (Доступно Админам)


        [HttpDelete("{deleteLogin}")]
        public async Task<ActionResult<User>> Delete(string deleteLogin, string login, string password)
        {
            User changer = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            User user = db.Users.FirstOrDefault(x => x.Login == deleteLogin);
            if (changer == null && !changer.Admin)
            {
                return BadRequest();
            }

            if (user == null)
            {
                return NotFound();
            }
            user.RevokedOn = DateTime.Now;
            user.RevokedBy = changer.Login;
            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        //Update-2
        //done 10) Восстановление пользователя - Очистка полей(RevokedOn, RevokedBy) (Доступно Админам)
        [HttpPut("RecoverUser/{recoverLogin}")]
        public async Task<ActionResult> RecoverUser(string recoverLogin, string login, string password)
        {
            User changer = await db.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            User oldUser = await db.Users.FirstOrDefaultAsync(x => x.Login == recoverLogin);
            if (!changer.Admin && oldUser.RevokedOn == null && changer == null)
            {
                return BadRequest();
            }
            if (oldUser == null)
            {
                return NotFound();
            }
            oldUser.RevokedOn = null;
            oldUser.RevokedBy = "";
            oldUser.ModifiedOn = DateTime.Now;
            oldUser.ModifiedBy = login;

            db.Update(oldUser);
            await db.SaveChangesAsync();
            return Ok(oldUser);
        }









    }
}
