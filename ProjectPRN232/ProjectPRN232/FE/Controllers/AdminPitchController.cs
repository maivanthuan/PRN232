using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using Microsoft.AspNetCore.Hosting; // Add this namespace

namespace FE.Controllers
{
    public class AdminPitchController : Controller
    {
        private readonly IPitchServices _pitchServices;
        private readonly IWebHostEnvironment _webHostEnvironment; // New field

        public AdminPitchController(IPitchServices pitchServices, IWebHostEnvironment webHostEnvironment)
        {
            _pitchServices = pitchServices;
            _webHostEnvironment = webHostEnvironment; // Initialize
        }

        // READ: Hiển thị danh sách Pitch
        public async Task<IActionResult> Index()
        {
            var pitches = await _pitchServices.GetPitchAsQueryable().ToListAsync();
            return View(pitches);
        }

        // CREATE: Hiển thị form tạo mới
        public IActionResult Create()
        {
            return View();
        }

        // CREATE: Xử lý submit form tạo mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PitchCreateDTO pitchCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var newPitch = await _pitchServices.CreatePitchAsync(pitchCreateDTO);
                if (newPitch != null)
                {
                    TempData["SuccessMessage"] = "Pitch created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Could not create pitch.");
            }
            return View(pitchCreateDTO);
        }

        // EDIT: Hiển thị form chỉnh sửa
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pitch = await _pitchServices.GetPitchByIdAsync(id);
            if (pitch == null)
            {
                return NotFound();
            }
            // Chuyển PitchDTO sang PitchUpdateDTO để truyền vào View Edit
            var pitchUpdateDTO = new PitchUpdateDTO
            {
                PitchId = pitch.PitchId,
                PitchType = pitch.PitchType,
                Image = pitch.Image,
                Status = pitch.Status,
                Price = pitch.PricePitch,
                TimeEnd = DateTime.UtcNow.AddYears(1)
            };
            return View(pitchUpdateDTO);
        }

        // EDIT: Xử lý submit form chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, PitchUpdateDTO pitchUpdateDTO)
        {
            if (id != pitchUpdateDTO.PitchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedPitch = await _pitchServices.UpdatePitchAsync(id, pitchUpdateDTO);
                if (updatedPitch != null)
                {
                    TempData["SuccessMessage"] = "Pitch updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Could not update pitch.");
            }
            return View(pitchUpdateDTO);
        }

        // DELETE: Hiển thị xác nhận xóa
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pitch = await _pitchServices.GetPitchByIdAsync(id);
            if (pitch == null)
            {
                return NotFound();
            }
            return View(pitch);
        }

        // DELETE: Xử lý xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var isDeleted = await _pitchServices.DeletePitchAsync(id);
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Pitch deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Could not delete pitch.";
            return RedirectToAction(nameof(Index));
        }
    }
}