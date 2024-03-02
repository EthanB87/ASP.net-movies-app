namespace Movies.Entities
{
    public class Actor
    {
        // PK:
        public int ActorId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        // A read-only (i.e. get only) prop to show full name:
        public string? FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        // Nav to all Castings:
        public ICollection<Casting>? Castings { get; set; }
    }
}
