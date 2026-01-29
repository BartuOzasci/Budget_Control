using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ButceTakip.Models
{
    public class Islem
    {
        [Key]
        public int Id { get; set; }

        // İlişki: Her işlemin bir kategorisi vardır.
        public int KategoriId { get; set; }
        
        [ForeignKey("KategoriId")]
        public Kategori? Kategori { get; set; }

        [Required(ErrorMessage = "Tutar girilmesi zorunludur.")]
        public decimal Tutar { get; set; }

        [MaxLength(75)]
        public string? Aciklama { get; set; } // Örn: "Market alışverişi"

        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}