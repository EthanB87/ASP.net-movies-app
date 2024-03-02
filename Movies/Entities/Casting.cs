namespace Movies.Entities
{
    public class Casting
    {
        // Composite PK made of 2 FKs to Movie & Actor respectively:
        public int ActorId { get; set; }
        public int MovieId { get; set; }

        public string? Role { get; set; }

        // Nav props to Movie & Actor:
        public Movie? Movie { get; set; }

        public Actor? Actor { get; set; }
    }
}
