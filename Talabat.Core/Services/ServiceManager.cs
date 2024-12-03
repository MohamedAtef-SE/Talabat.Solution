using AutoMapper;
using Microsoft.Extensions.Logging;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Services.Products;
using Talabat.Core.Domain.Contracts;

namespace Talabat.Core.Application.Services
{
    internal class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductServices> _logger;

        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IPaymentService> _paymentService;

        public ServiceManager(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ProductServices> logger,
            Func<IAuthService> authService,
            Func<IOrderService> orderService,
            Func<IBasketService> basketService,
            Func<IPaymentService> paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _productService = new Lazy<IProductService>(new ProductServices(_unitOfWork,_logger, _mapper));
            _authService = new Lazy<IAuthService>(authService, LazyThreadSafetyMode.ExecutionAndPublication);
            _orderService = new Lazy<IOrderService>(orderService, LazyThreadSafetyMode.ExecutionAndPublication);
            _basketService = new Lazy<IBasketService>(basketService, LazyThreadSafetyMode.ExecutionAndPublication);
            _paymentService = new Lazy<IPaymentService>(paymentService,LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IAuthService AuthService => _authService.Value;

        public IProductService ProductService => _productService.Value;

        public IOrderService OrderService => _orderService.Value;

        public IBasketService BasketService => _basketService.Value;
        
        public IPaymentService PaymentService => _paymentService.Value;
    }
}
