using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IMAGIO.Services
{
    public enum Role
    {
        [Display(Name = "Гость")]
        Guest = 0,
        [Display(Name = "Пользователь")]
        User = 1,
        [Display(Name = "Пользователь PRO")]
        UserPRO = 2,
    }
}
