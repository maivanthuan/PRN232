namespace FE.Models
{
    public class PaymentViewModel
    {
        public string? PitchId { get; set; }
        public DateOnly SelectedDate { get; set; }
        public List<int> SelectedSlots { get; set; } = new List<int>();

        // THÊM THUỘC TÍNH MỚI
        public List<string> SelectedSlotTimes { get; set; } = new List<string>();

        public int TotalPrice { get; set; }
    }
}
