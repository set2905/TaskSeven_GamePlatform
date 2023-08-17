using Microsoft.EntityFrameworkCore;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain.Repo
{
    public class PlayerRepo : IPlayerRepo
    {
        private const int GAMESEARCH_INVALIDAFTERSECONDS = 10;

        private readonly AppDbContext context;

        public PlayerRepo(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Player?> GetByName(string name)
        {
            return await context.Players.SingleOrDefaultAsync(x => x.Name == name);

        }
        public async Task<Player?> FindOpponent(GameType game, Guid playerId)
        {
            DateTime allowedGameSearchStart = DateTime.Now.Subtract(TimeSpan.FromSeconds(GAMESEARCH_INVALIDAFTERSECONDS));
            return await context.Players.FirstOrDefaultAsync(p => p.Id!=playerId
                                                                  &&p.LookingForOpponent
                                                                  && p.CurrentGameType==game
                                                                  && p.GameSearchStarted>allowedGameSearchStart);
        }
        public async Task<bool> Delete(Player entity)
        {
            context.Players.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Player>> GetAll()
        {
            return await context.Players.ToListAsync();

        }

        public async Task<Player?> GetById(Guid id)
        {
            return await context.Players.SingleOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Guid> Save(Player entity)
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
