using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(DayMemoryDbContext dbContext) : base(dbContext)
        {
        }
    }
}