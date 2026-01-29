using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ButceTakip.Data;
using ButceTakip.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için gerekli
using Newtonsoft.Json;

#pragma warning disable CS8602 // Disable null reference warnings for ViewBag assignments

namespace ButceTakip.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UygulamaDbContext _context;

        public DashboardController(UygulamaDbContext context)
        {
            _context = context;
        }

        // Parametreler eklendi (nullable, çünkü ilk açılışta filtre yok)
        public async Task<IActionResult> Index(DateTime? filtreTarih, int? filtreKategoriId)
        {
            // 1. TARİH FİLTRESİ AYARLARI
            // Eğer tarih seçilmediyse bugünün tarihini al
            DateTime secilenTarih = filtreTarih ?? DateTime.Now;
            
            // Ayın başı (Örn: 01.12.2025 00:00:00)
            DateTime baslangic = new DateTime(secilenTarih.Year, secilenTarih.Month, 1);
            // Ayın sonu (Örn: 01.01.2026'dan 1 gün öncesi -> 31.12.2025)
            DateTime bitis = baslangic.AddMonths(1).AddDays(-1);

            // 2. TEMEL SORGUNUN HAZIRLANMASI
            var sorgu = _context.Islemler
                .Include(x => x.Kategori)
                .Where(x => x.Kategori != null && x.Tarih >= baslangic && x.Tarih <= bitis);

            // 3. KATEGORİ FİLTRESİ (Eğer "Tümü" seçilmediyse)
            if (filtreKategoriId != null && filtreKategoriId > 0)
            {
                sorgu = sorgu.Where(x => x.KategoriId == filtreKategoriId);
            }

            // Sorguyu çalıştır ve veriyi çek
            var islemler = await sorgu.ToListAsync();

            // --- VIEW İÇİN HAZIRLIKLAR ---

            // Filtreleri View'a geri gönder (Seçili kalsınlar diye)
            // HTML5 input type="month" formatı yyyy-MM şeklindedir
            ViewBag.SecilenTarih = secilenTarih.ToString("yyyy-MM"); 
            ViewBag.SecilenKategoriId = filtreKategoriId;

            // Kategori Dropdown'ını doldur
            // "Id=0, Baslik=Tümü" seçeneğini manuel ekleyeceğiz View tarafında veya burada
            ViewBag.Kategoriler = new SelectList(_context.Kategoriler, "Id", "Baslik", filtreKategoriId);


            // --- HESAPLAMALAR VE GRAFİKLER (Aynı Mantık) ---

            decimal toplamGelir = islemler.Where(i => i.Kategori.Tur == "Gelir").Sum(i => i.Tutar);
            decimal toplamGider = islemler.Where(i => i.Kategori.Tur == "Gider").Sum(i => i.Tutar);

            ViewBag.ToplamGelir = toplamGelir.ToString("C0");
            ViewBag.ToplamGider = toplamGider.ToString("C0");
            ViewBag.ToplamGiderDecimal = toplamGider;
            ViewBag.Bakiye = (toplamGelir - toplamGider).ToString("C0");

            // DOUGHNUT CHART
            var giderOzeti = islemler
                .Where(i => i.Kategori.Tur == "Gider")
                .GroupBy(j => j.Kategori.Id)
                .Select(k => new
                {
                    Baslik = k.First().Kategori.Ikon + " " + k.First().Kategori.Baslik,
                    Tutar = k.Sum(j => j.Tutar)
                })
                .OrderByDescending(l => l.Tutar)
                .ToList();

            ViewBag.DoughnutLabels = Newtonsoft.Json.JsonConvert.SerializeObject(giderOzeti.Select(x => x.Baslik));
            ViewBag.DoughnutData = Newtonsoft.Json.JsonConvert.SerializeObject(giderOzeti.Select(x => x.Tutar));

            // TOP 5 HARCAMA KATEGORİLERİ
            ViewBag.TopKategoriler = giderOzeti.Take(5).ToList();

            // SPLINE CHART (Haftalık detay)
            List<string> weeks = new List<string>();
            List<decimal> weeklyIncomes = new List<decimal>();
            List<decimal> weeklyExpenses = new List<decimal>();

            int daysInMonth = DateTime.DaysInMonth(secilenTarih.Year, secilenTarih.Month);
            for (int week = 1; week <= 5; week++) // Max 5 hafta
            {
                int startDay = (week - 1) * 7 + 1;
                int endDay = Math.Min(week * 7, daysInMonth);
                if (startDay > daysInMonth) break;

                DateTime startDate = new DateTime(secilenTarih.Year, secilenTarih.Month, startDay);
                DateTime endDate = new DateTime(secilenTarih.Year, secilenTarih.Month, endDay);

                decimal inc = islemler.Where(x => x.Kategori.Tur == "Gelir" && x.Tarih >= startDate && x.Tarih <= endDate).Sum(x => x.Tutar);
                decimal exp = islemler.Where(x => x.Kategori.Tur == "Gider" && x.Tarih >= startDate && x.Tarih <= endDate).Sum(x => x.Tutar);

                weeks.Add($"Hafta {week}");
                weeklyIncomes.Add(inc);
                weeklyExpenses.Add(exp);
            }

            ViewBag.SplineAxisX = Newtonsoft.Json.JsonConvert.SerializeObject(weeks);
            ViewBag.SplineIncome = Newtonsoft.Json.JsonConvert.SerializeObject(weeklyIncomes);
            ViewBag.SplineExpense = Newtonsoft.Json.JsonConvert.SerializeObject(weeklyExpenses);

            // BAR CHART: Son 6 ayın gelir/gider karşılaştırması
            List<string> barLabels = new List<string>();
            List<decimal> barIncomes = new List<decimal>();
            List<decimal> barExpenses = new List<decimal>();

            for (int i = 5; i >= 0; i--)
            {
                DateTime ay = secilenTarih.AddMonths(-i);
                string label = ay.ToString("MMM yyyy");
                decimal gelir = _context.Islemler
                    .Include(x => x.Kategori)
                    .Where(x => x.Kategori != null && x.Kategori.Tur == "Gelir" && x.Tarih.Year == ay.Year && x.Tarih.Month == ay.Month)
                    .Sum(x => x.Tutar);
                decimal gider = _context.Islemler
                    .Include(x => x.Kategori)
                    .Where(x => x.Kategori != null && x.Kategori.Tur == "Gider" && x.Tarih.Year == ay.Year && x.Tarih.Month == ay.Month)
                    .Sum(x => x.Tutar);

                barLabels.Add(label);
                barIncomes.Add(gelir);
                barExpenses.Add(gider);
            }

            ViewBag.BarLabels = Newtonsoft.Json.JsonConvert.SerializeObject(barLabels);
            ViewBag.BarIncomes = Newtonsoft.Json.JsonConvert.SerializeObject(barIncomes);
            ViewBag.BarExpenses = Newtonsoft.Json.JsonConvert.SerializeObject(barExpenses);

            return View();
        }
    }
}