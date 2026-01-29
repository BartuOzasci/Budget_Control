using Microsoft.AspNetCore.Mvc;
using ButceTakip.Data;
using ButceTakip.Models;
using Microsoft.EntityFrameworkCore;

namespace ButceTakip.Controllers
{
    public class KategoriController : Controller
    {
        private readonly UygulamaDbContext _context;

        // Dependency Injection (Bağımlılık Enjeksiyonu)
        // Veritabanı bağlantısını buraya "enjekte" ediyoruz.
        public KategoriController(UygulamaDbContext context)
        {
            _context = context;
        }

        // GET: Kategori Listesi
        public async Task<IActionResult> Index()
        {
            // Veritabanındaki kategorileri "Gelir" veya "Gider" olmasına göre sıralayıp getirir.
            return View(await _context.Kategoriler.ToListAsync());
        }

        // GET: Yeni Kategori Ekleme Sayfası
        public IActionResult Create()
        {
            return View();
        }

        // POST: Yeni Kategori Kaydetme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind kısmından "Ikon"u çıkardık çünkü biz atayacağız.
        public async Task<IActionResult> Create([Bind("Id,Baslik,Tur")] Kategori kategori) 
        {
            if (ModelState.IsValid)
            {
                // OTOMATİK İKON ATAMA MANTIĞI
                if (kategori.Tur == "Gelir")
                {
                    kategori.Ikon = "💰"; // Para torbası
                }
                else
                {
                    kategori.Ikon = "💳"; // Kredi kartı
                }

                _context.Add(kategori);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }
        
        // POST: Silme İşlemi
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori != null)
            {
                _context.Kategoriler.Remove(kategori);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}