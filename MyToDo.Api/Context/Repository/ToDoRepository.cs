using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MyToDo.Api.Context.Repository
{

    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }

}
