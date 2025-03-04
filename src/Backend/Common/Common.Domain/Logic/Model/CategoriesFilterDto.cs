namespace Common.Domain.Logic.Model
{
    public class CategoriesFilterDto : Filtro
    {
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
