using Bulkybook.DataAcess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace BulkyBookweb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]//目前Controller中所有的Action都需要身分驗證
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderToal { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var clainsIdentity = (ClaimsIdentity)User.Identity;
            var claim = clainsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product")
            };

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            //    var clainsIdentity = (ClaimsIdentity)User.Identity;
            //    var claim = clainsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //    ShoppingCartVM = new ShoppingCartVM()
            //    {
            //        ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
            //        includeProperties: "Product")
            //    };

            //    foreach (var cart in ShoppingCartVM.ListCart)
            //    {
            //        cart.Price = GetPriceOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            //        ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            //    }
            //    return View(ShoppingCartVM);
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartid)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartid);
            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartid)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartid);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private decimal GetPriceOnQuantity(decimal quantity, decimal price, decimal price50, decimal price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else if (quantity <= 100)
            {
                return price50;
            }
            else
            {
                return price100;
            }
        }
    }
}
