using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding_Daryl
{
	//Need to use star class and not pure gameobject because need to know the connections of each star
	//Also need to have a custom class to hold the 
	private static List<Star> galaxyStarList;

	public static List<Star> FindPath(Star startStar, Star endGoalStar)
	{
		//Do the algorithm.
		//Initialise the lists to be used in the algorithm.
		//List<StarPath> completedList = new List<StarPath>();
		List<StarPath> priorityList = new List<StarPath>();

		//Construct the initial priority list from the stars in the galaxy.
		//This is what the algorithm will loop through and use to know what stars are connected to what other stars in the galaxy graph.
		foreach (Star star in galaxyStarList)
		{
			StarPath newPath = new StarPath(star, null);
			if (star == startStar)
			{
				newPath.shortestPathToStart = newPath;
				newPath.smallestDistanceToStart = 0.0f;
			}
			priorityList.Add(newPath);
		}
		priorityList = OrderPriorityListByDistance(priorityList); //Before the algorithm begins. We want to make sure the starting star is at the top priority to look at.


		//Go through the priority list and for each star look at it's connections and find the distance to the start star.
		//We know that there is a path between the start and end stars when the end node is at the start of the priority list and it has the highest priority.
		while (priorityList.Count > 0)
		{
			StarPath currentStarNode = priorityList[0];//This should be the starting star on the first iteration. This is the star with the top priority to look at.





			//Check all the stars that connect to the current star and calculate the distance to that star from the start node.
			//Once this has been done update the star path information for each node so we know the shortest path to the node.
			foreach (StarPath nextStar in GetStarPathsFromRoutesDictionary(currentStarNode.star.starRoutes, priorityList))
			{
				//Calculate the distance to the starting star by adding the distance from the start node to the current star onto the distance from the current star to the next star.
				float distance = nextStar.star.starRoutes[currentStarNode.star] + currentStarNode.smallestDistanceToStart;

				//Check if this new path's distance is shorter than the current path to this star from the start node.
				if (distance < nextStar.smallestDistanceToStart)
				{
					//Coming from the start via the current star is the shortest path to this star so update the path information.
					nextStar.smallestDistanceToStart = distance;
					nextStar.shortestPathToStart = currentStarNode;//This variable/line of code contains the information that tells you what path is currently the shortest one back to the starting star.
				}
			}

			//Once we reach this point the current star at the top of the priority list has fully been checked!!!
			//Now we should remove it from the priority list and then reorder it for the next iteration of the loop.
			priorityList.Remove(currentStarNode);
			if (priorityList.Count > 0)
			{
				priorityList = OrderPriorityListByDistance(priorityList);
				if (priorityList[0].star == endGoalStar && priorityList[0].smallestDistanceToStart != float.MaxValue)
				{
					//If the star with the highest priority is now the end star then that means we have found the shortest path to the end.
					//However the distance to the start must not be infinite else that just means we haven't found a connection.
					//as the end star will only have the highest priority when it has the shortest distance value so we now need to backtrack and construct the path list to return.
					List<Star> pathToEnd = new List<Star>();
					StarPath backtrackStar = priorityList[0]; //This is currently the end goal star.
					pathToEnd.Add(backtrackStar.star);

					while (backtrackStar.star != startStar)
					{
						//We are now retracing our steps. Going through each star that is the shortest path to the current one and then adding it to the path to the end goal list.
						backtrackStar = backtrackStar.shortestPathToStart;
						pathToEnd.Add(backtrackStar.star);
					}

					//The path to the start star from the end star has been constructed. We now want to reverse it so that that the path given is from the start star to the end star instead.
					pathToEnd.Reverse();
					return pathToEnd;//We have the path to the goal star so we now want to return it!!!
				}
			}
		}

		//If we get out of the loop then there is no path to the end as the priority list has ran out without getting to the end star.
		return new List<Star>();
	}

	/// <summary>
	/// This function goes through all the star paths in the list and sorts them by the shortest distance path using a bubble sort.
	/// </summary>
	/// <param name="a_pathList"></param>
	/// <returns></returns>
	private static List<StarPath> OrderPriorityListByDistance(List<StarPath> a_pathList)
	{
		for (int i = 0; i < a_pathList.Count; i++)
		{
			for (int j = 0; j < a_pathList.Count - 1; j++)
			{
				StarPath first = a_pathList[j];
				StarPath second = a_pathList[j + 1];
				if (first.smallestDistanceToStart > second.smallestDistanceToStart)
				{
					a_pathList[j] = second;
					a_pathList[j + 1] = first;
				}
			}
		}

		return a_pathList;
	}

	/// <summary>
	/// This function gets the star path information from a star's connections.
	/// </summary>
	/// <param name="starRoutes"></param>
	/// <param name="priorityList"></param>
	/// <returns></returns>
	private static List<StarPath> GetStarPathsFromRoutesDictionary(Dictionary<Star, float> starRoutes, List<StarPath> priorityList)
	{
		List<StarPath> result = new List<StarPath>();

		foreach (KeyValuePair<Star, float> route in starRoutes)
		{
			Star star = route.Key;
			for (int i = 0; i < priorityList.Count; i++)
			{
				if (priorityList[i].star == star)
				{
					result.Add(priorityList[i]);
					break;
				}
			}
		}




		return result;
	}

	//Getters and setters.
	/// <summary>
	/// Set the galaxy list.
	/// </summary>
	/// <param name="stars"></param>
	public static void SetGalaxyStarList(List<Star> stars)
	{
		galaxyStarList = new List<Star>(stars);
	}

	//Structs.
	/// <summary>
	/// This class contains all the path information for a star.
	/// </summary>
	public class StarPath
	{
		public Star star;
		public StarPath shortestPathToStart;
		public float smallestDistanceToStart;

		public StarPath(Star a_current, StarPath a_shortest)
		{
			star = a_current;
			shortestPathToStart = a_shortest;
			smallestDistanceToStart = float.MaxValue;
		}
	}
}