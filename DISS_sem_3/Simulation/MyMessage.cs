using DISS_sem_3.Entities;
using OSPABA;

namespace Simulation
{
	public class MyMessage : OSPABA.MessageForm
	{
		public Patient Patient { get; set; }
		public Nurse? Nurse { get; set; }
		public Room? Room { get; set; }
		public Doctor? Doctor { get; set; }
		public Patient? EntryWaiting { get; set; }
		public Patient? MedicalWaitingA { get; set; }
		public Patient? MedicalWaitingB { get; set; }
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
			Nurse = original.Nurse;
			Room = original.Room;
			Doctor = original.Doctor;
			EntryWaiting = original.EntryWaiting;
			MedicalWaitingA = original.MedicalWaitingA;
			MedicalWaitingB = original.MedicalWaitingB;
		}
	}
}