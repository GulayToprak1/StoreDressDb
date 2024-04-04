using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDressDb
{
    public class Dress : EntityBase
    {
        [DisplayName("Adı")]
        [Required(ErrorMessage = "Adı alanı zorunludur")]
        [MaxLength(50, ErrorMessage = "Adı alanı maksimum 50 karakter olmalıdır")]
        public string Name { get; set; }

        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage = "Kategori Adı alanı zorunludur")]
        public int CategoryId { get; set; }

        [DisplayName("Tasarımcı Adı")]
        [Required(ErrorMessage = "Tasarımcı Adı alanı zorunludur")]
        public int DesignerId { get; set; }

        [DisplayName("Özellikleri")]
        public string Description { get; set; }

        [DisplayName("Fiyat")]
        public double Price { get; set; }

        [DisplayName("Resim")]
        [MaxLength(250, ErrorMessage = "Resim alanı maksimum 250 karakter olmalıdır")]
        public string? ImageURL { get; set; }

        [DisplayName("Büyük Resim")]
        [MaxLength(250, ErrorMessage = "Büyük Resim alanı maksimum 250 karakter olmalıdır")]
        public string? BigImageURL { get; set; }

        [DisplayName("Küçük Resim")]
        [MaxLength(250, ErrorMessage = "Küçük Resim alanı maksimum 250 karakter olmalıdır")]
        public string? SmallImageURL { get; set; }

        [DisplayName("Sizin için seçtiklerimiz listesinde görünsün mü ?")]
        public bool IsSelected { get; set; }

        [DisplayName("Çok satanlar listesinde görünsün mü ?")]
        public bool IsBestSeller { get; set; }
    }
}
