using Microsoft.EntityFrameworkCore;
using Simple_E_commers_App.Models;

namespace Simple_E_commers_App.Reprositrory
{
    public class CategoryReprosetory : ICatograyRebrestory
    {
        public AppDbContext context { get; }
        public CategoryReprosetory(AppDbContext context) {
            this.context = context;
        }


        public void AddCategory(Category category)
        {
            context.Categories.Add(category);
        }

        public void DeleteCategory(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                context.Categories.Remove(category);
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateCategory(Category category)
        {
            var categoryToUpdate = context.Categories.FirstOrDefault(c => c.Id == category.Id);

            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategoriesIncludingProducts()
        {
            return context.Categories.Include(c => c.Products).ToList();
        }
    }
}
