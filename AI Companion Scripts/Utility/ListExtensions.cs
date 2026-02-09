using System;
using System.Collections.Generic;
using System.Linq;

//~~ This is a utility class for extending the functionality of lists in C#.
// I'm personally using it as it will assist in helping to build the proper functionality for battle of my AI companion.
// Below I will reference the source of the code and the author.
//~~ Author: adammyhre
//~~ Last Commit: May 5th 2024 
//~~ Accessed: May 4th 2025
//~~ Source: https://github.com/adammyhre/Unity-Behaviour-Trees/blob/master/Assets/_Project/Scripts/Utilities/ListExtensions.cs
public static class ListExtensions
{
	static Random rng;

	/// <summary>
	/// Shuffles the elements in the list using the Durstenfeld implementation of the Fisher-Yates algorithm.
	/// This method modifies the input list in-place, ensuring each permutation is equally likely, and returns the list for method chaining.
	/// Reference: http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
	/// </summary>
	/// <param name="list">The list to be shuffled.</param>
	/// <typeparam name="T">The type of the elements in the list.</typeparam>
	/// <returns>The shuffled list.</returns>
	public static IList<T> Shuffle<T>(this IList<T> list)
	{
		if (rng == null) rng = new Random();
		int count = list.Count;
		while (count > 1)
		{
			--count;
			int index = rng.Next(count + 1);
			(list[index], list[count]) = (list[count], list[index]);
		}

		return list;
	}
}