using System.ComponentModel.DataAnnotations;

namespace Json_Article_Website.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        [Display(Name = "Return URL")]
        public string? ReturnUrl { get; set; } = null;

    }

    public class LoginSession
    {
        public string UserName { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; } = false;
        public DateTime LoginTime { get; set; } = DateTime.Now;
         
    }
}
