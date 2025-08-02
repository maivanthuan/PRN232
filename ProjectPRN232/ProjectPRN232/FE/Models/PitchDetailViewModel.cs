namespace FE.Models
{
    public class FeedbackCreateViewModel
    {
        public string? PitchId { get; set; }
        public string? Content { get; set; }
        public double? Rating { get; set; }
    }

    public class PitchDetailViewModel
    {
        public PitchViewModel Pitch { get; set; } = new PitchViewModel();
        public List<FeedbackViewModel> Feedbacks { get; set; } = new List<FeedbackViewModel>();
        public List<BookingTimeViewModel> AvailableSlots { get; set; } = new List<BookingTimeViewModel>();
        public DateOnly SelectedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // THÊM THUỘC TÍNH MỚI
        // Thuộc tính này sẽ được dùng để binding với form tạo feedback
        public FeedbackCreateViewModel NewFeedback { get; set; } = new FeedbackCreateViewModel();
    }
}
