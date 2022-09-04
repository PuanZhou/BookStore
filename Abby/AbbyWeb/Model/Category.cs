using System.ComponentModel.DataAnnotations;

namespace AbbyWeb.Model
{
    public class Category
    {
        [Key] //非必要 假如屬性名稱是 ID Entity Framework 會自動設為 Primary key
        public int Id { get; set; }
        
        [Display(Name = "名稱")]
        [Required(ErrorMessage = "名稱為必填欄位")]
        public string Name { get; set; }
        
        [Display(Name="次序")]
        [Range(1,100,ErrorMessage ="次序的範圍必須在1-100")]
        [Required(ErrorMessage ="次序為必填欄位")]
        public int DisplayOrder { get; set; } //code first int type nullable=false 
    }
}
