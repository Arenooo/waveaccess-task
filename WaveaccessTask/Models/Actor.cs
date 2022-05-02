using System.ComponentModel.DataAnnotations.Schema;

namespace WaveaccessTask.Models
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public List<Movie> MoviesStarredIn { get; set; }
    }
}
