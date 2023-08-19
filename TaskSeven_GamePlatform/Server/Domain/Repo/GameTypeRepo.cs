using Microsoft.EntityFrameworkCore;
using TaskSeven_GamePlatform.Server.Domain;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain.Repo
{
    public class GameTypeRepo : IGameTypeRepo
    {
        private readonly AppDbContext context;

        public GameTypeRepo(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> Delete(GameType entity)
        {
            context.GameTypes.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GameType>> GetAll()
        {
            return await context.GameTypes.ToListAsync();
        }

        public async Task<GameType?> GetById(Guid id)
        {
            return await context.GameTypes.SingleOrDefaultAsync(x => x.Id == id);

        }
        public async Task<GameType?> GetByName(string name)
        {
            return await context.GameTypes.SingleOrDefaultAsync(x => x.Name == name);

        }
        public async Task<Guid> Save(GameType entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
