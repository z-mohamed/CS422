using System;
using System.IO;
namespace CS422
{
	public class PCQueue
	{
		public Node _front = null,_back = null,_dummy = null, _temp = null;

		public PCQueue()
		{
			
		}

		public class Node
		{
			public int data;
			public Node next;

			public Node(int val)
			{
				
				data = val;
				next = null;

			}

		}

		public void Enqueue(int dataValue)
		{
			bool _try_enqueue = true;

			// empty list
			if (_front == null) 
			{
				_back = new Node (dataValue);
				Console.WriteLine ("First");
				_front = _back;
				Console.WriteLine ("2nd");
			}
			else
			{
				while(_try_enqueue)
				{
					if (_dummy == _front)
					{
						//do nothing, dequeue in progress
					}
					else
					{
						_temp = _back;
						_back = new Node (dataValue);
						_temp.next = _back;
						_try_enqueue = false;
					}
				}

			}
	 	}

		public bool Dequeue(ref int out_value)
		{

			//if list is empty, return false
			if (_front == null) {
				Console.WriteLine ("Third");
				return false;
			}

			//mark node in front for deletion
			_dummy = _front;

			//save data before deletetin 
			out_value = _front.data;
		
			//delete
			_front = _front.next;

			return true;
		}
			
	}
}

