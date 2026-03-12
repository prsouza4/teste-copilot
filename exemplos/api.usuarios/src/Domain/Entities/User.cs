using System;

namespace Usuario.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string CPF { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Profession { get; private set; }
        public string Email { get; private set; }

        private User() { }

        public User(Guid id, string name, string cpf, DateTime dateOfBirth, string profession, string email)
        {
            Id = id;
            Name = name;
            CPF = cpf;
            DateOfBirth = dateOfBirth;
            Profession = profession;
            Email = email;
        }

        public void Update(string name, string profession, string email)
        {
            Name = name;
            Profession = profession;
            Email = email;
        }
    }
}
