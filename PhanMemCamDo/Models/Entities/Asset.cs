using System.ComponentModel.DataAnnotations;

namespace PhanMemCamDo.Models.Entities
{
    public class Asset
    {
        public int Id { get; set; }
        public string ?AssetName { get; set; }
        public string ?Description { get; set; }

        // 👇 XÓA DÒNG CŨ:
        // public virtual ICollection<AssetImage> AssetImages { get; set; }

        // 👇 THÊM 2 DÒNG MỚI (Liên kết với Danh mục):
        [Display(Name = "Loại Tài Sản")]
        public int? AssetCategoryId { get; set; } // Cho phép null tạm thời để tránh lỗi dữ liệu cũ
        public virtual AssetCategory? AssetCategory { get; set; }

        // Liên kết Hợp đồng (Giữ nguyên)
        public virtual ICollection<PawnContract> ?PawnContracts { get; set; }
    }
}