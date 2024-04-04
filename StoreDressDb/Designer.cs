using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDressDb
{
    public class Designer : EntityBase
    {
        [DisplayName("Adı Soyadı")]
        [Required(ErrorMessage = "Adı Soyadı alanı zorunludur")]
        [MaxLength(50, ErrorMessage = "Adı Soyadı alanı maksimum 50 karakter olmalıdır")]
        public string NameSurname { get; set; }

      
    }
}
