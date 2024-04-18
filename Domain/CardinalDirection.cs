using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CardinalDirection
    {
        public char Label { get; }
        public int XMovement { get; }
        public int YMovement { get; }
        public char ClockwiseCardinalDirection { get; }
        public char CounterclockwiseCardinalDirection { get; }

        public CardinalDirection(char label, int xMovement, int yMovement, char clockwiseCardinalDirection, char counterclockwiseCardinalDirection)
        {
            Label = label;
            XMovement = xMovement;
            YMovement = yMovement;
            ClockwiseCardinalDirection = clockwiseCardinalDirection;
            CounterclockwiseCardinalDirection = counterclockwiseCardinalDirection;
        }
    }

    public class CardinalDirections : ReadOnlyCollection<CardinalDirection>
    {
        public static CardinalDirection North = new CardinalDirection(label: 'N', xMovement: 0, yMovement: 1, clockwiseCardinalDirection: 'E', counterclockwiseCardinalDirection: 'W');
        public static CardinalDirection East = new CardinalDirection(label: 'E', xMovement: 1, yMovement: 0, clockwiseCardinalDirection: 'S', counterclockwiseCardinalDirection: 'N');
        public static CardinalDirection South = new CardinalDirection(label: 'S', xMovement: 0, yMovement: -1, clockwiseCardinalDirection: 'W', counterclockwiseCardinalDirection: 'E');
        public static CardinalDirection West = new CardinalDirection(label: 'W', xMovement: -1, yMovement: 0, clockwiseCardinalDirection: 'N', counterclockwiseCardinalDirection: 'S');

        public static ReadOnlyCollection<CardinalDirection> All = new ReadOnlyCollection<CardinalDirection>((from members in typeof(CardinalDirections).GetFields()
                                                                                                                           where members.FieldType == typeof(CardinalDirection)
                                                                                                             select (CardinalDirection)members.GetValue(typeof(CardinalDirection).GetField(members.Name)))
                                                                                                            .ToList());

        public static CardinalDirection GetByLabel(char label)
        {
            var direction = CardinalDirections.All.SingleOrDefault(s => char.ToLower(s.Label) == char.ToLower(label));

            if (direction == null)
                throw new Exception($"No CardinalDirection exists with label {label}");

            return direction;
        }

        public CardinalDirections() : base(All) { }
    }
}
