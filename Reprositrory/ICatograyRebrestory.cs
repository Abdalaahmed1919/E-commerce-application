using Simple_E_commers_App.Models;

namespace Simple_E_commers_App.Reprositrory
{
    public interface ICatograyRebrestory
    {
         void AddCategory(Category category);
         void UpdateCategory(Category category);
         void DeleteCategory(int id);
         Category GetCategoryById(int id);
         IEnumerable<Category> GetAllCategories();
         IEnumerable<Category> GetAllCategoriesIncludingProducts();
         void Save();
    }
}
