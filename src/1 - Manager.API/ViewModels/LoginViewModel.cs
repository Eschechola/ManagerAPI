using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModes{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O login não pode vazio.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A senha não pode vazio.")]
        public string Password { get; set; }
    }
}