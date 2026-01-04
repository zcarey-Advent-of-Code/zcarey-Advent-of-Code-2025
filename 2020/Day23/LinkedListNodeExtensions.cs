using System;
using System.Collections.Generic;
using System.Text;

namespace Day23 {
	public static class LinkedListNodeExtensions {

		/// <summary>
		/// Finds the next node in a linked list but if the end of the list is reached it insteead wraps around returns the first node in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="node"></param>
		/// <returns></returns>
		public static LinkedListNode<T> CircularNext<T>(this LinkedListNode<T> node) {
			if (node.Next == null) return node.List.First;
			else return node.Next;
		}

		/// <summary>
		/// Finds the previous node in a linked list but if the fron of the list is reached it instead wraps around and return the last node in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="node"></param>
		/// <returns></returns>
		public static LinkedListNode<T> CircularPrevious<T>(this LinkedListNode<T> node) {
			if (node.Previous == null) return node.List.Last;
			else return node.Previous;
		}

		/// <summary>
		/// Iterates through the linked list nodes starting with this node and in a circular fashion.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Node"></param>
		/// <returns></returns>
		public static IEnumerable<LinkedListNode<T>> AsCircularEnumerable<T>(this LinkedListNode<T> Node) {
			LinkedListNode<T> currentNode = Node;
			do {
				yield return currentNode;
				currentNode = currentNode.CircularNext();
			} while (currentNode != Node);
		}

		/// <summary>
		/// Inserts the given nodes after this node, keeping their order in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="node"></param>
		/// <param name="elements"></param>
		public static void AddAfter<T>(this LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> elements) {
			LinkedList<T> list = node.List;
			LinkedListNode<T> currentNode = node;
			foreach(LinkedListNode<T> element in elements) {
				list.AddAfter(currentNode, element);
				currentNode = element;
			}
		}

		/// <summary>
		/// Removes the given number of nodes after this node and returns them. If the end of the list is reached, it will wrap around to the first element in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="node"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static IEnumerable<LinkedListNode<T>> CircularRemoveAfter<T>(this LinkedListNode<T> node, int count) {
			LinkedList<T> list = node.List;
			while (count-- > 0) {
				LinkedListNode<T> next = node.CircularNext();
				list.Remove(next);
				yield return next;
			}
		}

	}
}
