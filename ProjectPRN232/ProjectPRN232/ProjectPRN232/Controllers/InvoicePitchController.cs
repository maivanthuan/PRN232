using BusinessObject.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.IServices;

namespace ProjectPRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicePitchController : ControllerBase
    {
        private readonly IInvoicePitchService _invoicePitchService;

        public InvoicePitchController(IInvoicePitchService invoicePitchService)
        {
            _invoicePitchService = invoicePitchService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetInvoicePitches()
        {
            return Ok(_invoicePitchService.GetInvoicePitchesAsQueryable());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoicePitch(int id)
        {
            var invoicePitch = await _invoicePitchService.GetInvoicePitchByIdAsync(id);
            if (invoicePitch == null) return NotFound();
            return Ok(invoicePitch);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoicePitch(InvoicePitchCreateDTO createDto)
        {
            var createdInvoice = await _invoicePitchService.CreateInvoicePitchAsync(createDto);
            return CreatedAtAction(nameof(GetInvoicePitch), new { id = createdInvoice.InvoicePitchId }, createdInvoice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoicePitch(int id, InvoicePitchUpdateDTO updateDto)
        {
            var updatedInvoice = await _invoicePitchService.UpdateInvoicePitchAsync(id, updateDto);
            if (updatedInvoice == null) return NotFound("Không tìm thấy hóa đơn chi tiết để cập nhật.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoicePitch(int id)
        {
            var success = await _invoicePitchService.DeleteInvoicePitchAsync(id);
            if (!success) return NotFound("Không tìm thấy hóa đơn chi tiết để xóa.");
            return NoContent();
        }
    }
}
