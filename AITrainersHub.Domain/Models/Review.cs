using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; } = null!;

        // مفيش لوجين، فبناخد اسم يكتبه الزائر بنفسه بدل حساب مسجل
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        // الريفيو ميظهرش للزوار غير بعد موافقتك من الـ Admin
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public Review()
        {
            
        }
        public Review(Guid platformId, string reviewerName, int rating, string comment)
        {
            PlatformId = platformId;
            ReviewerName = reviewerName;
            Rating = rating;
            Comment = comment;
        }
        public void Update(string? reviewerName, int? rating, string? comment, bool? isApproved)
        {
            ReviewerName = reviewerName ?? ReviewerName;
            Rating = rating ?? Rating;
            Comment = comment ?? Comment;
            IsApproved = isApproved ?? IsApproved;
        }
    }
}
