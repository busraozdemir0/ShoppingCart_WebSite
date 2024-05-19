using Shop.DataModels.CustomModels;
using Shop.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly ShoppingCartDBContext _dbContext = null;

        public AdminService(ShoppingCartDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResponseModel AdminLogin(LoginModel loginModel)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                var userData = _dbContext.AdminInfos.Where(x => x.Email == loginModel.EmailId).FirstOrDefault();
                if (userData is not null)
                {
                    if (userData.Password == loginModel.Password)
                    {
                        responseModel.Status = true;
                        responseModel.Message = Convert.ToString(userData.Id) + "|" + userData.Name + "|" + userData.Email;
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = "Your Password is Incorrect";
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Email not registered. Please check your Email Id";
                }
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = "An Error has occured. Please try again!";

                return responseModel;
            }

        }
   
        public CategoryModel SaveCategory(CategoryModel newCategory)
        {
            try
            {
                Category _category = new Category();
                _category.Name = newCategory.Name;
                _dbContext.Add(_category);
                _dbContext.SaveChanges();
                return newCategory;
            }
            catch(Exception ex)
            {
                throw;
            }
          
        }
    
        public List<CategoryModel> GetCategories()
        {
            var data = _dbContext.Categories.ToList();
            List<CategoryModel> _categoryList = new List<CategoryModel>();
            foreach(var c in data)
            {
                CategoryModel _categoryModel = new CategoryModel();
                _categoryModel.Id = c.Id;
                _categoryModel.Name = c.Name;
                _categoryList.Add(_categoryModel);
            }
            return _categoryList;
        }
   
        public bool UpdateCategory(CategoryModel categoryToUpdate)
        {
            bool flag = false;
            var _category = _dbContext.Categories.Where(x => x.Id == categoryToUpdate.Id).First();
            if(_category is not null)
            {
                _category.Name = categoryToUpdate.Name;
                _dbContext.Categories.Update(_category);
                _dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public bool DeleteCategory(CategoryModel categoryToDelete)
        {
            bool flag = false;
            var _category = _dbContext.Categories.Where(x => x.Id == categoryToDelete.Id).First();
            if (_category is not null)
            {
                _dbContext.Categories.Remove(_category);
                _dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }
    }
}
