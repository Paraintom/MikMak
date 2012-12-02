namespace MikMak.DomainModel.Entities
{

    /// <summary>
    /// This is a class that represent or an Existant Pawn, or a click where you want a pawn to be set.
    /// </summary>
    public class Pawn
    {
        /// <summary>
        /// Could be 'Q' for Queen, as it could be 'B' for black...        
        /// </summary>
        public char Name { get; set; }

        /// <summary>
        /// There is two Queen, give the pawn's owner number
        /// </summary>
        public Player Player { get; set; }

        public Coord Coord { get; set; }

        public Pawn(char name, int x, int y): this()
        {
            this.Name = name;
            Coord.x = x;
            Coord.y = y;
        }

        public Pawn()
        {
            Coord = new Coord();
        }
    }
}
