namespace GoFish
{
    public class Card
    {
        public Values Value { get; set; }
        public Suits Suit { get; set; }
        public string ValueSuit { get { return $"{Value} of {Suit}"; } }
        public Card(Values value, Suits suit)
        {
            this.Value = value;
            this.Suit = suit;
        }

        public override string ToString()
        {
            return this.ValueSuit;
        }

    }
}
