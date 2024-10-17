using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BaiThiController : Controller
    {
        private readonly ICauHoiRepository _cauHoiRepository;
        private readonly IBaiThiRepository _baithiRepository;
        private readonly IKetQuaRepository _ketQuaRepository;
        private readonly ApplicationDbContext _context;

        public BaiThiController(ICauHoiRepository CauhoiRepository, IBaiThiRepository baiThiRepository, IKetQuaRepository ketQuaRepository, ApplicationDbContext context)
        {
            _cauHoiRepository = CauhoiRepository;
            _baithiRepository = baiThiRepository;
            _ketQuaRepository = ketQuaRepository;
            _context = context;
        }

        [Route("danh-sach-bai-thi")]
        public async Task<IActionResult> Index()
        {
            var baiThis = await _baithiRepository.GetAllAsync();
            return View(baiThis);
        }

        [Route("them-bai-thi")]
        public IActionResult Add()
        {
            return View();
        }

        [Route("them-bai-thi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BaiThi baiThi)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    await _baithiRepository.AddAsync(baiThi);
                    TempData["SuccessMessage"] = "Thêm đề thi thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi thêm mới đề thi.");
                }
            }
            return View(baiThi);
        }

        [Route("cap-nhat-bai-thi")]
        public async Task<IActionResult> Update(int id)
        {
            var baiThi = await _baithiRepository.GetByIdAsync(id);
            if (baiThi == null)
            {
                return NotFound();
            }
            return View(baiThi);
        }

        [Route("cap-nhat-bai-thi")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, BaiThi baiThi)
        {
            if (id != baiThi.Id)
            {
                return NotFound();
            }
            //if (ModelState.IsValid)
            //{
            await _baithiRepository.UpdateAsync(baiThi);
            return RedirectToAction(nameof(Index));
            //}
            return View(baiThi);
        }

        [Route("xoa-bai-thi")]
        public async Task<IActionResult> Delete(int id)
        {
            var baiThi = await _baithiRepository.GetByIdAsync(id);
            if (baiThi == null)
            {
                return NotFound();
            }
            return View(baiThi);
        }

        [Route("xoa-bai-thi")]
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baiThi = await _baithiRepository.GetByIdAsync(id);
            if (baiThi != null)
            {
                await _baithiRepository.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
