using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.Repositories
{
    public class QuestionListRepository : Repository<QuestionList>, IQuestionListRepository
    {
        public QuestionListRepository(DayMemoryDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<QuestionList> GetQuery()
        {
            return base.GetQuery()
                .Include(i => i.Questions);
        }

    }
}