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

        [HttpGet]
        [Route("CheckoutStripe")]
        public IActionResult CheckoutStripe(string cardNumber, int expMonth, int expYear, string cvc, decimal value)
        {
            var data = _userService.MakePaymentStripe(cardNumber, expMonth, expYear, cvc, value);
            return Ok(data);
        }

        [HttpGet]
        [Route("CheckoutPayPal")]
        public IActionResult CheckoutPayPal(double total)
        {
            var url = _userService.MakePaymentPaypal(total);
            return Ok(url);
        }

        [HttpPost]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout(List<CartModel> cartItems) // Birden fazla urun siparisi verilebilme durumu oldguu icin CartModel sinifini Liste biciminde tanimladik.
        {
            // Hangi odeme yontemi secilmisse ona gore calismasi icin (PayPal ve Stripe kisminda api key'den dolayi calismiyor.)
            ResponseModel responseModel = new ResponseModel();
            var record = cartItems.FirstOrDefault();
            if (record != null)
            {
                if (record.PaymentMode == "CashOnDelivery")
                {
                    responseModel = _userService.Checkout(cartItems);
                }
                if (record.PaymentMode == "PayPal")
                {
                    var data = _userService.MakePaymentPaypal(record.PayPalPayment);
                    if (data != null)
                    {
                        var ref_number = data.Result.Split("&")[1];
                        cartItems.FirstOrDefault().orderReference = ref_number.Split("=")[1];
                        responseModel = _userService.Checkout(cartItems);
                    }
                }
                if (record.PaymentMode == "Stripe")
                {
                    var data = await _userService.MakePaymentStripe(record.Stripecard_Number, record.Stripeexp_Month, record.Stripeexp_Year, record.Stripeexp_Cvc, record.Stripe_Value);
                    if (data != null && data.Contains("Success"))
                    {
                        cartItems.FirstOrDefault().orderReference = data.Split("=")[1];
                        responseModel = _userService.Checkout(cartItems);
                    }
                }
            }


            return Ok(responseModel);


            //var data = _userService.Checkout(cartItems);
            //return Ok(data);
        }
    }
}
