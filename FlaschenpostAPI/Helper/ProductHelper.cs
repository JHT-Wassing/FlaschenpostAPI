namespace FlaschenpostAPI.Helper
{
    public static class ProductHelper
    {
        /// <summary>
        /// Extracts the price per litre out of the pricePerUnitText
        /// </summary>
        /// <param name="pricePerUnitText"></param>
        /// <returns>double pricePerLitre</returns>
        public static double GetPricePerLitre(string pricePerUnitText)
        {
            var pricePerLitre = 0.0;

            if (!string.IsNullOrEmpty(pricePerUnitText))
            {
                if (pricePerUnitText.Length > 0)
                {
                    // Get substring from pricePerUnitText
                    // Text looks like: "(1,80 €/Liter)"
                    // --> start after first character, then look for "€" and substract 1
                    var subtext = pricePerUnitText.Substring(1, pricePerUnitText.IndexOf("€") - 1);

                    subtext = subtext.Trim();

                    _ = double.TryParse(subtext, out pricePerLitre);
                }

            }

            // if parsing was successful, price will be returned. Otherwise 0.0
            return pricePerLitre;
        }

        /// <summary>
        /// Extracts the amount of bottles out of the shortDescription
        /// </summary>
        /// <param name="shortDescription"></param>
        /// <returns>int amountOfBottles</returns>
        public static int GetBottles(string shortDescription)
        {
            var amountOfBottles = 0;

            if (!string.IsNullOrEmpty(shortDescription))
            {
                if (shortDescription.Length > 0)
                {
                    // Get substring from shortDescription
                    // Text looks like: "20 x 0,5L (Glas)"
                    // --> start from beginning, then look for "x" and substract 1
                    var subtext = shortDescription.Substring(0, shortDescription.IndexOf("x") - 1);

                    // trim to delete whitespaces
                    subtext = subtext.Trim();

                    _ = int.TryParse(subtext, out amountOfBottles);
                }

            }

            // if parsing was successful, amount of bottles will be returned. Otherwise 0
            return amountOfBottles;
        }
    }
}
