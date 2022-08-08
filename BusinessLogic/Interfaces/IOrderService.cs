
using Common.ModelDTO;

namespace BusinessLogic.Interfaces
{
    public interface IOrderService
    {
        void Create(OrderDTO order);
        void Delete(int id);
    }
}
