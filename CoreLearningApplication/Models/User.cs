using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreLearningApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreLearningApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Не указан e-mail")]
        //[EmailAddress(ErrorMessage = "Некорректный адрес")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }

        public static User GetDefaultUser(string name)
        {
            return new User
            {
                Name = name,
                Password = null,
                Email = null,
                UserType = UserType.Unregistered
            };
            
        }
    }
}
