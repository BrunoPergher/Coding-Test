public class ATM
{
    private readonly decimal[] denominations = { 10, 50, 100 };

    public List<List<decimal>> CalculateCombinations(decimal amount)
    {
        if (amount < 10)
        {
            Console.WriteLine($"It's not possible to make combinations for {amount} EUR. The amount should be greater or equal to ten.\n");
            return new List<List<decimal>>();
        }

        List<List<decimal>> combinations = new List<List<decimal>>();
        CalculateCombinationsHelper(amount, 0, new List<decimal>(), combinations);
        return combinations;
    }

    private void CalculateCombinationsHelper(decimal amount, int index, List<decimal> currentCombination, List<List<decimal>> combinations)
    {
        if (amount == 0)
        {
            combinations.Add(new List<decimal>(currentCombination));
            return;
        }

        for (int i = index; i < denominations.Length; i++)
        {
            if (amount >= denominations[i])
            {
                currentCombination.Add(denominations[i]);
                CalculateCombinationsHelper(amount - denominations[i], i, currentCombination, combinations);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }
    }

    public static void Main()
    {
        ATM atm = new ATM();
        decimal[] payouts = { 9, 30, 50, 60, 80, 140, 230, 370, 610, 980 };

        foreach (decimal payout in payouts)
        {
            Console.WriteLine($"Combinations for {payout} EUR:");

            var combinations = atm.CalculateCombinations(payout);

            if (combinations.Count == 0)
            {
                continue;
            }

            Console.WriteLine($"Possibilities: {combinations.Count}");

            foreach (var combination in combinations)
            {
                var quantityByDenomination = combination
                    .GroupBy(x => x)
                    .Select(x => $"{x.Count()} x {x.Key} EUR");

                Console.WriteLine(string.Join(" + ", quantityByDenomination));
            }

            Console.WriteLine("\n");
        }
    }
}
