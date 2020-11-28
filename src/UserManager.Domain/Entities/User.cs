using System;
using System.Collections.Generic;
using UserManager.Domain.Validators;
using UserManager.Infra.CrossCutting.Exceptions;

namespace UserManager.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        //EF
        protected User() { }

        public User(long id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            _errors = new List<string>();

            Validate();
        }


        public void ChangeName(string name)
        {
            Name = name;
            Validate();
        }


        public void ChangeEmail(string email)
        {
            Email = email;
            Validate();
        }

        public void ChangePassword(string password)
        {
            Password = password;
            Validate();
        }

        public override bool Validate()
        {
            //a palavra this valida a própria entidade
            var validator = new UserValidator();
            var validation = validator.Validate(this);

            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    Errors.Add(error.ErrorMessage);

                throw new DomainException("Alguns campos estão inválidos, por favor corrija-os.", Errors);
            }

            return true;
        }
    }
}
