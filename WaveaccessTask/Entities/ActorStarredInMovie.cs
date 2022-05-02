using Microsoft.EntityFrameworkCore;

namespace WaveaccessTask.Entities
{
    [Keyless]
    public class ActorStarredInMovie
    {
        public Guid ActorId { get; set; }
        public Guid MovieId { get; set; }
    }
}
