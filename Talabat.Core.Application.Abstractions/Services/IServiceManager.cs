namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IServiceManager
    {
        public IAuthService AuthService { get; }
        public IProductService ProductService { get; }
        public IOrderService OrderService { get; }
        public IBasketService BasketService { get; }
        public IPaymentService PaymentService { get; }
    }
}
