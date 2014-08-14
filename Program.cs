using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public const int CALORIES = 800;
        public const int GRAMS_OF_CARBS = 100;
        public const int GRAMS_OF_PROTEIN = 50;
        public const int GRAMS_OF_FAT = 22;

        public const int NUMBER_OF_CYCLES = 100000;

        static void Main(string[] args)
        {
            var availableParts = new List<Part>{
		new Part{
			Name="Whey Powder",
			UnitName="g",
			CaloriesPerUnit=3.75,
			CarbsPerUnit=0.09375,
			ProteinPerUnit=0.75,
            FatPerUnit=0.046875
		},
		new Part{
			Name="Oat Flour",
			UnitName="g",
			CaloriesPerUnit=4.04,
			CarbsPerUnit=0.66,
			ProteinPerUnit=0.15,
            FatPerUnit=.091
		},
		new Part{
			Name="Maltodextrin",
			UnitName="g",
			CaloriesPerUnit=3.82,
			CarbsPerUnit=.94,
			ProteinPerUnit=0,
            FatPerUnit=0
		},
		new Part{
			Name="Oil",
			UnitName="g",
			CaloriesPerUnit=8,
			CarbsPerUnit=.94,
			ProteinPerUnit=0,
            FatPerUnit=.9
		}
	};

            var bestRecipe = new List<RecipeLine>{
		new RecipeLine
		{
			NumberOfUnits = 10,
			Ingredient = availableParts[0]
		},
		new RecipeLine
		{
			NumberOfUnits = 10,
			Ingredient = availableParts[1]
		},
		new RecipeLine
		{
			NumberOfUnits = 10,
			Ingredient = availableParts[2]
		},
		new RecipeLine
		{
			NumberOfUnits = 10,
			Ingredient = availableParts[3]
		}
	};

            DumpRecipe(bestRecipe);

            for (int i = 0; i < NUMBER_OF_CYCLES; i++)
            {
                var mutations = new List<List<RecipeLine>>();
                for (int j = 0; j < 5; j++)
                {
                    mutations.Add(Mutate(bestRecipe.Select(item => (RecipeLine)item.Clone()).ToList()));
                }

                for (int j = 0; j < mutations.Count; j++)
                {
                    if (GetScore(mutations[j]) < GetScore(bestRecipe))
                    {
                        bestRecipe = mutations[j];
                    }
                }

                if (GetScore(bestRecipe) == 0)
                {
                    break;
                }
            }

            DumpRecipe(bestRecipe);
        }

        public class Part
        {
            public string Name;
            public string UnitName;
            public double CaloriesPerUnit;
            public double CarbsPerUnit;
            public double ProteinPerUnit;
            public double FatPerUnit;
        }

        public class RecipeLine:ICloneable
        {
            public int NumberOfUnits;
            public Part Ingredient;

            public object Clone()
            {
                return new RecipeLine { Ingredient = this.Ingredient, NumberOfUnits = this.NumberOfUnits };
            }
        }

        public static double GetScore(List<RecipeLine> recipe)
        {
            return Math.Abs(recipe.Sum(s => s.Ingredient.CaloriesPerUnit * s.NumberOfUnits) - CALORIES) +
                Math.Abs(recipe.Sum(s => s.Ingredient.ProteinPerUnit * s.NumberOfUnits) - GRAMS_OF_PROTEIN) +
                Math.Abs(recipe.Sum(s => s.Ingredient.CarbsPerUnit * s.NumberOfUnits) - GRAMS_OF_CARBS) +
                Math.Abs(recipe.Sum(s => s.Ingredient.FatPerUnit * s.NumberOfUnits) - GRAMS_OF_FAT);
        }

        public static List<RecipeLine> Mutate(List<RecipeLine> recipe)
        {
            var random = new Random();
            var result = random.Next(1, 2);

            switch (result)
            {
                case 1:
                    recipe = MutateRandomQuantity(recipe);
                    break;
            }

            return recipe;
        }

        public static List<RecipeLine> MutateRandomQuantity(List<RecipeLine> recipe)
        {
            var random = new Random();
            var indexResult = random.Next(0, recipe.Count);
            var directionResult = random.Next(0, 2);

            recipe[indexResult].NumberOfUnits = (int)Math.Round(recipe[indexResult].NumberOfUnits * (directionResult == 1 ? 1.1 : 0.9));

            return recipe;
        }

        public static void DumpRecipe(List<RecipeLine> recipe)
        {
            Debug.WriteLine("Recipe ======");
            foreach(var line in recipe)
            {
                Debug.WriteLine(line.NumberOfUnits + line.Ingredient.UnitName + " of  " + line.Ingredient.Name);
            }
            Debug.WriteLine("Score of " + GetScore(recipe));
            Debug.WriteLine("=============");
        }

        public class Recipe:List<RecipeLine>
        {

        }
    }
}
