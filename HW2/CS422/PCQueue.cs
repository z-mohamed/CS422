using System;

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
			
			// empty list
			if (_front == null) 
			{
				_back = new Node (dataValue);

				_front = _back;

			}

			// 1 item in list
			else if (_front == _back && _front.next == null) 
			{
				_back = new Node (dataValue);
				
				_front.next = _back;
			} 

			//>1 item
			else 
			{
				_temp = _back;

				_back = new Node (dataValue);

				_temp.next = _back;

			}


		}



		public bool Dequeue(ref int out_value)
		{

			//if list is empty, return false
			if (_front == null && _dummy == null )
				return false;

			//mark node in front
			_dummy = _front;

			//if node after front dosen't exist, 1 item in list
			if (_dummy == null) 
			{
				out_value = _front.data;
				//_front = _;

			} 
			//multiple item in list delete one in front
			else 
			{
				out_value = _front.data;
				_front = _dummy;
				
			}

			return true;
		}
			
	}
}

