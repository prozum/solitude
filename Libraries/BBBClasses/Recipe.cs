using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBBClasses
{
	public struct MashIngredient
	{
		public Malt Grain;
		public int QuantityGrams;

		public MashIngredient(Malt malt, int quantity)
		{
			this.Grain = malt;
			this.QuantityGrams = quantity;
		}
	}

	public struct BoilIngredient
	{
		public Hop Hop;
		public OtherIngredient Other;
		public int QuantityGrams;
		public TimeSpan BoilTime;

		public BoilIngredient(Hop hop, int quantity_grams, int boiltime_minutes)
		{
			this.Hop = hop;
			this.Other = OtherIngredient.NA;
			this.QuantityGrams = quantity_grams;
			this.BoilTime = TimeSpan.FromMinutes(boiltime_minutes);
		}

		public BoilIngredient(OtherIngredient ingredient, int quantity_grams, int boiltime_minutes)
		{
			this.Hop = Hop.NA;
			this.Other = ingredient;
			this.QuantityGrams = quantity_grams;
			this.BoilTime = TimeSpan.FromMinutes(boiltime_minutes);
		}
	}

	public struct DryIngredient
	{
		public Hop Hop;
		public OtherIngredient Other;
		public int QuantityGrams;
		public TimeSpan TimeAfterFermentationStart;

		public DryIngredient(Hop hop, int quantity_grams, int addition_time_hours)
		{
			this.Hop = hop;
			this.Other = OtherIngredient.NA;
			this.QuantityGrams = quantity_grams;
			this.TimeAfterFermentationStart = TimeSpan.FromHours(addition_time_hours);
		}

		public DryIngredient(OtherIngredient ingredient, int quantity_grams, int addition_time_hours)
		{
			this.Hop = Hop.NA;
			this.Other = ingredient;
			this.QuantityGrams = quantity_grams;
			this.TimeAfterFermentationStart = TimeSpan.FromHours(addition_time_hours);
		}
	}

	public struct YeastIngredient
	{
		public Yeast yeast;
		public int QuantityGrams;
	}

	public enum Malt { Pale, LightCrystal, Chocolate }
	public enum Hop { NA, Citra, Galaxy, Nelson }
	public enum OtherIngredient { NA, Protafloc, OrangePeel, Cinnamon}
	public enum Yeast { WLP001, Wyeast1187, Wyeast1056 }

	public class Recipe
	{
		public Guid RecipeId { get; set; }
		public Guid BrewerId { get; set; }
		public Guid BeerId { get; set; }

		public int Quantity { get; set; }

		/*Mash*/
		public int MashLiquor { get; set; }
		public TimeSpan MashTime { get; set; }
		public int MashTemperatureCelsius { get; set; }
		public IEnumerable<MashIngredient> MashIngredients;

		/* Boil */
		public int BoilLiquor { get; set; }
		public TimeSpan BoilTime { get; set; }
		public IEnumerable<BoilIngredient> BoilIngredients;

		/* Fermentation */
		public int FermentationTemperatureCelsius { get; set; }
		public TimeSpan FermentationTime { get; set; }
		public TimeSpan ConditioningTime { get; set; }
		public IEnumerable<DryIngredient> DryIngredients;

		public Yeast Yeast;
	}
}
