using Microsoft.EntityFrameworkCore;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain.Repo
{
    public class GameStateRepo : IGameStateRepo
    {
        private readonly AppDbContext context;

        public GameStateRepo(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> Delete(GameState entity)
        {
            context.GameStates.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GameState>> GetAll()
        {
            return await context.GameStates.ToListAsync();
        }

        public async Task<GameState?> GetById(Guid id)
        {
            return await context.GameStates.SingleOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Guid> Save(GameState entity)
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
