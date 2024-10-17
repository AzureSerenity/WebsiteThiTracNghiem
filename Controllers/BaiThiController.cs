using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace DoAnCoSo.Controllers
{
    public class BaiThiController : Controller
    {
        private readonly ICauHoiRepository _cauHoiRepository;
        private readonly IBaiThiRepository _baiThiRepository;
        private readonly IKetQuaRepository _ketQuaRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILichSuLamBaiRepository _lichSuLamBaiRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BaiThiController(ICauHoiRepository cauHoiRepository, IBaiThiRepository baiThiRepository, ApplicationDbContext context, IKetQuaRepository ketQuaRepository, ILichSuLamBaiRepository lichSuLamBaiRepository, UserManager<ApplicationUser> userManager)
        {
            _cauHoiRepository = cauHoiRepository;
            _baiThiRepository = baiThiRepository;
            _ketQuaRepository = ketQuaRepository;
            _context = context;
            _userManager = userManager;
            _lichSuLamBaiRepository = lichSuLamBaiRepository;
        }

        [Route("danh-sach-cac-bai-thi")]
        public async Task<IActionResult> Index()
        {
            var baiThis = await _baiThiRepository.GetAllAsync();
            return View(baiThis);
        }

        [Route("bai-thi")]
        public async Task<IActionResult> Display(int id)
        {
            if (HttpContext.Session.GetString($"BaiThi_{id}_Completed") == "true")
            {
                TempData["Error"] = "Bạn không thể truy cập lại bài thi đã nộp.";
                return RedirectToAction("Index");
            }

            var baiThi = await _baiThiRepository.GetByIdAsync(id);
            if (baiThi == null)
            {
                return NotFound();
            }

            var cauHois = baiThi.CauHois.ToList();
            ViewBag.BaiThiId = id;
            ViewBag.ThoiGianThi = baiThi.ThoiGianLamBai;
            return View(cauHois);
        }

        [Route("ket-qua-bai-thi")]
        [HttpPost]
        public async Task<IActionResult> KetQua(int? id, IFormCollection form)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiThi = await _baiThiRepository.GetByIdAsync(id.Value);
            if (baiThi == null)
            {
                return NotFound();
            }

            var questions = baiThi.CauHois.ToList();
            List<CauHoi> cauHois = new List<CauHoi>();
            int correctAnswersCount = 0;
            DateTime startTime = DateTime.Parse(form["StartTime"]);
            DateTime endTime = DateTime.Now;

            foreach (var question in questions)
            {
                string answerKey = $"SelectedAnswer[{question.Mach}]";
                string selectedAnswer = form[answerKey];

                cauHois.Add(new CauHoi
                {
                    Mach = question.Mach,
                    Cauhoi = question.Cauhoi,
                    Dapan = question.Dapan,
                    SelectedAnswer = selectedAnswer
                });

                if (selectedAnswer == question.Dapan)
                {
                    correctAnswersCount++;
                }
            }

            double score = questions.Count > 0 ? (double)correctAnswersCount / questions.Count * 10 : 0;
            var user = await _userManager.GetUserAsync(User);

            var ketQua = new KetQua
            {
                Madt = id.Value,
                SoCauDung = correctAnswersCount,
                Diem = score,
                ThoiDiemHoanThanh = endTime,
                Id = user.Id
            };

            await _ketQuaRepository.AddAsync(ketQua);

            var lichSuLamBai = new LichSuLamBai
            {
                Id = user.Id,
                KetQuaId = ketQua.KetQuaId,
                ThoiDiemLamBai = endTime
            };

            await _lichSuLamBaiRepository.AddAsync(lichSuLamBai);

            ViewBag.CauHois = cauHois;
            ViewBag.TongSoCau = questions.Count;
            ViewBag.TimeTaken = (endTime - startTime).ToString(@"hh\:mm\:ss");

            return View("KetQua", ketQua);
        }

        [Route("lich-su-lam-bai")]
        public async Task<IActionResult> LichSuLamBai()
        {
            var user = await _userManager.GetUserAsync(User);
            var history = await _lichSuLamBaiRepository.GetHistoryByUserIdAsync(user.Id);
            return View(history);
        }
    }
}
