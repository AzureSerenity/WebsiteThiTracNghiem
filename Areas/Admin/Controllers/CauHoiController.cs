using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CauHoiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICauHoiRepository _cauHoiRepository;
        private readonly IBaiThiRepository _baiThiRepository;

        public CauHoiController(ApplicationDbContext context, ICauHoiRepository cauHoiRepository, IBaiThiRepository baiThiRepository)
        {
            _context = context;
            _cauHoiRepository = cauHoiRepository;
            _baiThiRepository = baiThiRepository;
        }

        [Route("thu-vien-cau-hoi")]
        public IActionResult Index()
        {
            var baiThiList = _baiThiRepository.GetAllAsync().Result;
            ViewBag.BaiThiList = new SelectList(baiThiList, "Id", "TenBaiThi");


            var cauHois = _context.CauHois.Where(c => c.Mabt == null).ToList();

            return View(cauHois);
        }

        [Route("them-cau-hoi")]
        public async Task<IActionResult> Add()
        {
            var baiThis = await _baiThiRepository.GetAllAsync();
            ViewBag.DeThis = new SelectList(baiThis, "Id", "TenBaiThi");
            return View();
        }

        [Route("them-cau-hoi")]
        [HttpPost]
        public async Task<IActionResult> Add(CauHoi cauHoi)
        {
            if (ModelState.IsValid)
            {
                await _cauHoiRepository.AddAsync(cauHoi);
                return RedirectToAction(nameof(Index));
            }
            var baiThis = await _baiThiRepository.GetAllAsync();
            ViewBag.DeThis = new SelectList(baiThis, "Id", "TenBaiThi");
            return View(cauHoi);
        }

        [Route("chinh-sua-cau-hoi")]
        [HttpPost]
        public async Task<IActionResult> Update(int id)
        {
            var cauhoi = await _cauHoiRepository.GetByIdAsync(id);
            if (cauhoi == null)
            {
                return NotFound();
            }
            var baiThis = await _baiThiRepository.GetAllAsync();
            ViewBag.DeThis = new SelectList(baiThis, "Id", "TenBaiThi", cauhoi.Mabt);
            return View(cauhoi);
        }

        [Route("xoa-cau-hoi")]
        public async Task<IActionResult> Delete(int id)
        {
            var cauhoi = await _cauHoiRepository.GetByIdAsync(id);
            if (cauhoi == null)
            {
                return NotFound();
            }
            return View(cauhoi);
        }

        [Route("xoa-cau-hoi")]
        // Process deletion of a question
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cauHoiRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
