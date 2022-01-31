using System;
using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;

namespace toll_calculator
{
    public class TollCalculator
    {
        public decimal CalculateToll(object vehicle) =>
        
            #region draft code 
            //vehicle switch
            //{
            //    Car c => 2.00m,
            //    Taxi t => 3.50m,
            //    Bus b => 5.00m,
            //    DeliveryTruck t => 10.00m,
            //    { } => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle)),
            //    null => throw new ArgumentNullException(nameof(vehicle))
            //};

            //APPLYING RULES
            //vehicle switch
            //{
            //    Car { Passengers: 0 } => 2.00m + 0.50m,
            //    Car { Passengers: 1 } => 2.0m,
            //    Car { Passengers: 2 } => 2.0m - 0.50m,
            //    Car => 2.00m - 1.0m,

            //    Taxi { Fares: 0 } => 3.50m + 1.00m,
            //    Taxi { Fares: 1 } => 3.50m,
            //    Taxi { Fares: 2 } => 3.50m - 0.50m,
            //    Taxi => 3.50m - 1.00m,

            //    Bus b when ((double)b.Riders / (double)b.Capacity) < 0.50 => 5.00m + 2.00m,
            //    Bus b when ((double)b.Riders / (double)b.Capacity) > 0.90 => 5.00m - 1.00m,
            //    Bus => 5.00m,

            //    DeliveryTruck t when (t.GrossWeightClass > 5000) => 10.00m + 5.00m,
            //    DeliveryTruck t when (t.GrossWeightClass < 3000) => 10.00m - 2.00m,
            //    DeliveryTruck => 10.00m,

            //    { } => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle)),
            //    null => throw new ArgumentNullException(nameof(vehicle))
            //};
        #endregion

            //OPTIMIZED CODE
            vehicle switch
            {
                Car c => c.Passengers switch
                {
                    0 => 2.00m + 0.5m,
                    1 => 2.0m,
                    2 => 2.0m - 0.5m,
                    _ => 2.00m - 1.0m
                },

                Taxi t => t.Fares switch
                {
                    0 => 3.50m + 1.00m,
                    1 => 3.50m,
                    2 => 3.50m - 0.50m,
                    _ => 3.50m - 1.00m
                },

                Bus b when ((double)b.Riders / (double)b.Capacity) < 0.50 => 5.00m + 2.00m,
                Bus b when ((double)b.Riders / (double)b.Capacity) > 0.90 => 5.00m - 1.00m,
                Bus b => 5.00m,

                DeliveryTruck t when (t.GrossWeightClass > 5000) => 10.00m + 5.00m,
                DeliveryTruck t when (t.GrossWeightClass < 3000) => 10.00m - 2.00m,
                DeliveryTruck t => 10.00m,

                { } => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle)),
                null => throw new ArgumentNullException(nameof(vehicle))
            };

        //LAST SECTION, PREMIUM TIMES
        public decimal PeakTimePremium(DateTime timeOfToll, bool inbound) =>
            (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
            {
                (true, TimeBand.Overnight, _) => 0.75m,
                (true, TimeBand.Daytime, _) => 1.5m,
                (true, TimeBand.MorningRush, true) => 2.0m,
                (true, TimeBand.EveningRush, false) => 2.0m,
                _ => 1.0m,
            };

        #region draft code 2
        
        // <SnippetPremiumWithoutPattern>
        public decimal PeakTimePremiumIfElse(DateTime timeOfToll, bool inbound)
        {
            if ((timeOfToll.DayOfWeek == DayOfWeek.Saturday) ||
                (timeOfToll.DayOfWeek == DayOfWeek.Sunday))
            {
                return 1.0m;
            }
            else
            {
                int hour = timeOfToll.Hour;
                if (hour < 6)
                {
                    return 0.75m;
                }
                else if (hour < 10)
                {
                    if (inbound)
                    {
                        return 2.0m;
                    }
                    else
                    {
                        return 1.0m;
                    }
                }
                else if (hour < 16)
                {
                    return 1.5m;
                }
                else if (hour < 20)
                {
                    if (inbound)
                    {
                        return 1.0m;
                    }
                    else
                    {
                        return 2.0m;
                    }
                }
                else // Overnight
                {
                    return 0.75m;
                }
            }
        }
        // </SnippetPremiumWithoutPattern>

        // <SnippetTuplePatternOne>
        public decimal PeakTimePremiumFull(DateTime timeOfToll, bool inbound) =>
            (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
            {
                (true, TimeBand.MorningRush, true) => 2.00m,
                (true, TimeBand.MorningRush, false) => 1.00m,
                (true, TimeBand.Daytime, true) => 1.50m,
                (true, TimeBand.Daytime, false) => 1.50m,
                (true, TimeBand.EveningRush, true) => 1.00m,
                (true, TimeBand.EveningRush, false) => 2.00m,
                (true, TimeBand.Overnight, true) => 0.75m,
                (true, TimeBand.Overnight, false) => 0.75m,
                (false, TimeBand.MorningRush, true) => 1.00m,
                (false, TimeBand.MorningRush, false) => 1.00m,
                (false, TimeBand.Daytime, true) => 1.00m,
                (false, TimeBand.Daytime, false) => 1.00m,
                (false, TimeBand.EveningRush, true) => 1.00m,
                (false, TimeBand.EveningRush, false) => 1.00m,
                (false, TimeBand.Overnight, true) => 1.00m,
                (false, TimeBand.Overnight, false) => 1.00m,
            };
        // </SnippetTuplePatternOne>
        
        #endregion

        // <SnippetIsWeekDay>
        private static bool IsWeekDay(DateTime timeOfToll) =>
            timeOfToll.DayOfWeek switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true
            };
        // </SnippetIsWeekDay>

        // <SnippetGetTimeBand>
        private enum TimeBand
        {
            MorningRush,
            Daytime,
            EveningRush,
            Overnight
        }
        private static TimeBand GetTimeBand(DateTime timeOfToll) =>
           timeOfToll.Hour switch
           {
               < 6 or > 19 => TimeBand.Overnight,
               < 10 => TimeBand.MorningRush,
               < 16 => TimeBand.Daytime,
               _ => TimeBand.EveningRush,
           };
        // </SnippetGetTimeBand>
    }

}

/*The switch expression makes other refinements to the syntax that surrounds the switch statement. 
 * The case keyword is omitted, and the result of each arm is an expression. The last two arms show a new language feature. 
 * The { } case matches any non-null object that didn't match an earlier arm. This arm catches any incorrect types passed to this method. 
 * The { } case must follow the cases for each vehicle type. If the order were reversed, the { } case would take precedence. 
 * Finally, the null constant pattern detects when null is passed to this method. 
 * The null pattern can be last because the other patterns match only a non-null object of the correct type.*/