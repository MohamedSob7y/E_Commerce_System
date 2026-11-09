using AutoMapper;
using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using E_Commerce.Domain.Entityes.Basket_Module;
using E_Commerce.Domain.Entityes.OrderModule;
using E_Commerce.Services.Abstraction;
using E_Commerce.Services.Specifications.OrderSpecification;
using E_Commerce.Shared.Common_Responses;
using E_Commerce.Shared.DTOS.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Error = E_Commerce.Shared.Common_Responses.Error;
namespace E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUniteofWork _uniteofWork;

        public OrderService(IMapper mapper,
            IBasketRepository basketRepository,
            IUniteofWork uniteofWork)
        {
            _mapper = mapper;
            _BasketRepository = basketRepository;
            _uniteofWork = uniteofWork;
        }

        public IBasketRepository _BasketRepository { get; }

        private OrderItems CreateOrderItem(BasketItem item, Product product)
        {
            return new OrderItems//Add Each Product اللى موجودين فى الbasket will Be Added To Orders
            {
                Product = new ProductItemOrdered
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureURL
                },
                Price = item.Price,
                Quantity = item.Quantity

            };
        }

        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email)
        {

            //1: Maps the provided shipping address to the order address entity.
            var orderaddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);//علمه فى Order Profile

            //2: Retrieves the basket and validates its existence.
            var basket = await _BasketRepository.GetBasketAsync(orderDto.BasketId);
            if (basket is null)
                return Error.NotFound("Basket not found", $"The Basket with id : {orderDto.BasketId} is Not Found");


            //3: Creates a list of order items by fetching product details from the database and validating each product.
            List<OrderItems> orderItems = new List<OrderItems>();
            foreach (var item in basket.Items)
            {
                var product = await _uniteofWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound("Product not found", $"The Product with id : {item.Id} is Not Found");


                //iterate For Each Product in Basket to add this Product to OrderItems List
                orderItems.Add(CreateOrderItem(item, product));
            }

            //4: Retrieves the selected delivery method and validates its existence.
            var deliverymethod = await _uniteofWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if (deliverymethod is null)
                return Error.NotFound("Delivery Method not found", $"The Delivery Method with id : {orderDto.DeliveryMethodId} is Not Found");

            //5: Calculates the subtotal of the order based on the items and their quantities.
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);//عشان عايز اعرف الorder كامل بكام السعر بتاع كل واحد فى الكمية بتاعت كل حاجة 

            //6: Creates a new Order with all relevant details.
            var order = new Order
            {
                UserEmail = email,//take this Email From Token عشان كدة مش محتاج اعمل Validate عليه
                Address = orderaddress,
                DeliveryMethod = deliverymethod,
                Items = orderItems,
                SubTotal = subtotal,

            };


            //7: Returns a DTO containing the full order details to the client, including Id [OrderId] , UserEmail ,  items [ProductName , PictureUrl , Price , Quantity], address, delivery method [ShortName], order status, OrderDate , subtotal, and total price
            await _uniteofWork.GetRepository<Order, Guid>().AddAsync(order);//Add Order + Orderitems Locally
            bool Result = await _uniteofWork.SaveChangesAsync() > 0;//Save Changes to Database
            if (!Result)//this Failure To Create Object
                Error.Failure("Order.Failure", "Failed to Create Order object");
            return _mapper.Map<Order, OrderToReturnDto>(order);//Return Order To Return Dto
        }

        public async Task<Result<IEnumerable<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync()
        {
            //This service retrieves and returns all available delivery methods supported by the system.It allows the client to display delivery options that users can select during the checkout process
            var deliverymethods = await _uniteofWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (!deliverymethods.Any())

                return Error.NotFound("DeliveryMethods.NotFound", "No Delivery Methods Available in the System");

            var Data = _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(deliverymethods);
            if (Data is null)
                return Error.NotFound("DeliveryMethods.NotFound", "No Delivery Methods Available in the System");
            return Result<IEnumerable<DeliveryMethodDto>>.Ok(Data);


        }

        public async Task<Result<IEnumerable<OrderToReturnDto>>> GetAllOrdersAsync(string email)
        {
            //Get AllOrders For Specific User Take This Email From Token
            //محتاج اكون Query For Db عشان retrive All Orders From Database So Use => Specification Design Pattern
            var orderSpec=new OrderSpecification(email);//Generate Query عشان يعمل كل الQuery اللى موجودة جواه فى الDb
            var orders =await _uniteofWork.GetRepository<Order, Guid>().GetAllAsync(orderSpec);//Get All Orders For Specific User From Database
            //كدة انا عملت Fileration Orders Based on Email
            if(!orders.Any())
                return Error.NotFound("Orders.NotFound", $"No Orders Found For User With Email : {email}");
            
            var data=_mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(orders);
            return Result<IEnumerable<OrderToReturnDto>>.Ok(data);//Map Orders To OrderToReturnDto
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdForUserAsync(Guid Id, string email)
        {
            //Make Specification For Filteration by Id + Email + Include OrderItem+ DeliveryMethod For this Order
            var orderSpec = new OrderSpecification(Id, email);
            var order = await _uniteofWork.GetRepository<Order, Guid>().GetByIdAsync(orderSpec);//Get Order By Id For Specific User From Database
            //كدة انا عملت Fileration Orders Based on Email + Id + Include OrderItem+ DeliveryMethod For this Order
            if(order is null)
                return Error.NotFound("Order.NotFound", $"No Order Found For User With Email : {email} And Order Id : {Id}");

            var data = _mapper.Map<Order, OrderToReturnDto>(order);
            return Result<OrderToReturnDto>.Ok(data);//Map Order To OrderToReturnDto
        }
    }
}
