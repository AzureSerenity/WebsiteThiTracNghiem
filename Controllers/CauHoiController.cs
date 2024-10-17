using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAnCoSo.Controllers
{
    public class CauhoiController : Controller
    {

        private readonly ICauHoiRepository _CauhoiRepository;
        private readonly IBaiThiRepository _baithiRepository;
        private readonly ApplicationDbContext _context;
        public CauhoiController(ICauHoiRepository CauhoiRepository, IBaiThiRepository baiThiRepository, ApplicationDbContext context)
        {
            _CauhoiRepository = CauhoiRepository;
            _baithiRepository = baiThiRepository;
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            var cauHois = await _CauhoiRepository.GetAllAsync();
            return View(cauHois);
        }

        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> Add()
        {
            var baiThis = await _baithiRepository.GetAllAsync();
            ViewBag.BaiThi = new SelectList(baiThis, "Id", "Name");
            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(CauHoi Cauhoi, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                await _CauhoiRepository.AddAsync(Cauhoi);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var baiThis = await _baithiRepository.GetAllAsync();
            ViewBag.BaiThi = new SelectList(baiThis, "Id", "Name");
            return View(Cauhoi);
        }

        public async Task<IActionResult> Display(int id)
        {
            var Cauhoi = await _CauhoiRepository.GetByIdAsync(id);
            if (Cauhoi == null)
            {
                return NotFound();
            }
            return View(Cauhoi);
        }

        public async Task<IActionResult> Update(int id)
        {
            var Cauhoi = await _CauhoiRepository.GetByIdAsync(id);
            if (Cauhoi == null)
            {
                return NotFound();
            }
            var categories = await _baithiRepository.GetAllAsync();
            ViewBag.BaiThi = new SelectList(categories, "Id", "Name");
            return View(Cauhoi);
        }

        [HttpPost]
        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var Cauhoi = await _CauhoiRepository.GetByIdAsync(id);
            if (Cauhoi == null)
            {
                return NotFound();
            }
            return View(Cauhoi);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _CauhoiRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
