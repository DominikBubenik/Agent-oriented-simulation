using DISS_sem_3.Entities;
using OSPABA;

namespace Simulation
{
	public class MyMessage : OSPABA.MessageForm
	{
		public Patient Patient { get; set; }
		public MyMessage(OSPABA.Simulation mySim) :
			base(mySim)
		{
		}

		public MyMessage(MyMessage original) :
			base(original)
		{
			// copy() is called in superclass
		}

		override public MessageForm CreateCopy()
		{
			return new MyMessage(this);
		}

		override protected void Copy(MessageForm message)
		{
			base.Copy(message);
			MyMessage original = (MyMessage)message;
			// Copy attributes
			Patient = original.Patient;
		}
	}
}