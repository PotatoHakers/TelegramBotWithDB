
using AutoMapper;
using BusinessLogic.Interfaces;
using Common.ModelDTO;
using Model;
using Model.Model;

namespace BusinessLogic.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationContext _appContext;
        private readonly IMapper _mapper;
        public OrderService(ApplicationContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
        }

        public void Create(OrderDTO model)
        {
            var order = _mapper.Map<Order>(model);
            _appContext.Orders.Add(order);
            _appContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Order order = Find(id);
            _appContext.Orders.Remove(order);
            _appContext.SaveChanges();
        }
        private Order Find(int id)
        {
            Order order = _appContext.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) throw new Exception("Object = null");
            return order;
        }
    }
}
