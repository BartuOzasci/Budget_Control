using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ButceTakip.Data;
using ButceTakip.Models;

namespace ButceTakip.Controllers
{
    public class IslemController : Controller
    {
        private readonly UygulamaDbContext _context;

        public IslemController(UygulamaDbContext context)
        {
            _context = context;
        }

        // GET: İşlem Listesi
        public async Task<IActionResult> Index()
        {
            // Include(x => x.Kategori) ÇOK ÖNEMLİ! 
            // Bunu yazmazsak listede kategori adı boş gelir.
            var islemler = await _context.Islemler.Include(x => x.Kategori).ToListAsync();
            return View(islemler);
        }

        // GET: Ekleme Sayfası
        public IActionResult Create()
        {
            // Dropdown (Açılır Kutu) için kategorileri hazırlayıp "Çanta"ya (ViewBag) koyuyoruz.
            // Parametreler: KaynakListe, ArkaPlandakiDeğer(Id), GörünenDeğer(Baslik)
            ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Baslik");
            return View();
        }

        // POST: Kaydetme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KategoriId,Tutar,Aciklama,Tarih")] Islem islem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(islem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Hata olursa formu tekrar gösterirken dropdown'ı yeniden doldurmalıyız
            ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Baslik", islem.KategoriId);
            return View(islem);
        }

        // POST: Silme
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var islem = await _context.Islemler.FindAsync(id);
            if (islem != null)
            {
                _context.Islemler.Remove(islem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}