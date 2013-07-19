using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RegistrationViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }

        [Required]
        public bool RememberMe { get; set; }

        /*public string[] ColorsForFirlds = {"Black", "Black", "Black", "Black", "Black", "Black", "Black"};
        public string this[string propName]
        {
            get
            {
                int i = -1;
                switch (propName)
                {
                    case "Login":
                        //логин не должен быть пустым
                        if (string.IsNullOrEmpty(Login))
                        {
                            ColorsForFirlds[0] = "red";
                            return "Do not enter your login";
                        }
                        break;
                    case "Password":
                        //пароль не должен быть пустым
                        if (string.IsNullOrEmpty(Password) || Password!=PasswordConfirmation)
                        {
                            ColorsForFirlds[1] = "red";
                            return "Do not enter your password";
                        }
                        break;
                    case "PasswordConfirmation":
                        //подтверждение пароля не должено быть пустым и должно совпадать с паролем
                        if (Password != PasswordConfirmation || string.IsNullOrEmpty(PasswordConfirmation))
                        {
                            ColorsForFirlds[2] = "red";
                            return "Confirm password is incorrect";
                        }
                        break;
                    case "Email":
                        //проверка Мыла
                        if (string.IsNullOrEmpty(Email) || !Regex.IsMatch(Email, ".+\\@.+\\..+"))
                        {
                            ColorsForFirlds[3] = "red";
                            return "Please enter a valid email address";
                        }
                        break;
                    case "Phone":
                        //Необязательное поле.
                        //Если чтото ввели то проверка телефона, телефон не должен содержать ничего кроме цифр
                        if (!string.IsNullOrEmpty(Phone))
                        {
                            if (!validString(Phone, char.IsDigit))
                            {
                                ColorsForFirlds[4] = "red";
                                return "Enter valid phone number or do not enter any thing";
                            }
                        }
                        break;
                    case "Country":
                        //Необязательное поле.
                        //Если чтото ввели то проверка страны, страна может содержать буквы и пробелы
                        if (!string.IsNullOrEmpty(Country))
                        {
                            if (!validString(Country, char.IsLetter, char.IsWhiteSpace))
                            {
                                ColorsForFirlds[5] = "red";
                                return "Enter valid country name or do not enter any thing";
                            }
                        }
                            
                        break;
                    case "City":
                        //Необязательное поле.
                        //Если чтото ввели то проверка города, город может содержать буквы и пробелы
                        if (!string.IsNullOrEmpty(City))
                        {
                            if (!validString(City, char.IsLetter, char.IsWhiteSpace))
                            {
                                ColorsForFirlds[6] = "red";
                                return "Enter valid city name or do not enter any thing";
                            }
                        }
                        break;
                }
                return null;
            }
        }
        //метод проверки строки
        private bool validString(string s, params isValid[] valid)
        {
            return s.Select(t => valid.Aggregate(false, (current, t1) => current | t1(t))).All(z => z);
        }

        public string Error { get; private set; }*/
    }
}