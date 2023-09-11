using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public int IdOfOrganization { get; set; }
        [ForeignKey("IdOfOrganization")]
        public string Firstname { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }/*
        public string Login { get; set; }
        public string Password { get; set; }*/
    }
    public class UserDTO
    {
        public int Id { get; set; }
        public int IdOfOrganization { get; set; }
        public string Firstname { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class Organization
    {
        public int IdOfOrganization { get; set; }
        [Required]
        public string NameOfOrganization { get; set; }
    }
}

