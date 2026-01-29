using System.ComponentModel.DataAnnotations;

namespace ButceTakip.Models
{
    public class Kategori
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        public string Baslik { get; set; } = string.Empty;

        // Bu ikon, arayüzde kategori yanına ikon koymak istersen (örn: "fa-solid fa-home")
        public string Ikon { get; set; } = "fa-solid fa-circle"; 

        // Bu kategorinin "Gelir" mi yoksa "Gider" kategorisi mi olduğunu tutar.
        public string Tur { get; set; } = "Gider"; // "Gelir" veya "Gider"
    }
}