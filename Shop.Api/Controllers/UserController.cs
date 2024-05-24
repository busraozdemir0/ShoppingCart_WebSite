using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.DataModels.CustomModels;
using Shop.Logic.Services;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("GetCategories")]
        public IActionResult GetCategories()
        {
            var data = _userService.GetCategories();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetProductByCategoryId")]
        public IActionResult GetProductByCategoryId(int categoryId)
        {
            var data = _userService.GetProductByCategoryId(categoryId).Result; // Burada async metotlari senkron olarak bekletmeyiyoruz.
            return Ok(data);
        }

        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult RegisterUser(RegisterModel registerModel)
        {
            var data = _userService.RegisterUser(registerModel);
            return Ok(data);
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser(LoginModel loginModel)
        {
            var data = _userService.LoginUser(loginModel);
            return Ok(data);
        }

        [HttpPost]
        [Route("Checkout")]
        public IActionResult Checkout(List<CartModel> cartItems) // Birden fazla urun siparisi verilebilme durumu oldguu icin CartModel sinifini Liste biciminde tanimladik.
        {
            var data = _userService.Checkout(cartItems);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetOrdersByCustomerId")]
        public IActionResult GetOrdersByCustomerId(int customerId) 
        {
            var data = _userService.GetOrdersByCustomerId(customerId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetOrderDetailForCustomer")]
        public IActionResult GetOrderDetailForCustomer(int customerId, string order_number)
        {
            var data = _userService.GetOrderDetailForCustomer(customerId, order_number);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetShippingStatusForOrder")]
        public IActionResult GetShippingStatusForOrder(string order_number)
        {
            var data = _userService.GetShippingStatusForOrder(order_number);
            return Ok(data);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(PasswordModel passwordModel)
        {
            var data = _userService.ChangePassword(passwordModel);
            return Ok(data);
        }

    }
}
