namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameSearchRequestModel
    {
        public GameSearchRequestModel(Guid playerId, string gameTypeName)
        {
            PlayerId=playerId;
            GameTypeName=gameTypeName;
        }

        public Guid PlayerId { get; set; }
        public string GameTypeName { get; set; }  
    }
}
