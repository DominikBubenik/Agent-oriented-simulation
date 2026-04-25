using OSPABA;
using Simulation;
using DISS_sem_3.Entities;
using MainLogic;

namespace Agents.AgentEDepartment
{
	//meta! id="21"
	public class AgentEDepartment : OSPABA.Agent
	{
		public StatPriorityQueue<Patient> EntryQueue;
		public StatPriorityQueue<Patient> MedicalTreatQueueA;
		public StatPriorityQueue<Patient> MedicalTreatQueueB;
		
		public AgentEDepartment(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			
			EntryQueue = new StatPriorityQueue<Patient>(MySim.CurrentTime);
			MedicalTreatQueueA = new StatPriorityQueue<Patient>(MySim.CurrentTime);
			MedicalTreatQueueB = new StatPriorityQueue<Patient>(MySim.CurrentTime);
		}
		
		public void EnqueuePatientEntry(MyMessage myMsg)
		{
			GlobalLogger.PrintLog(myMsg.Patient.ToString() + "is entring queue of length " + EntryQueue.Count, MySim.CurrentTime);
			var patient = myMsg.Patient;
			patient.StartEntryQueueWait();
			EntryQueue.Enqueue(patient, patient.Priority, patient.ArrivalTime, MySim.CurrentTime);
		}
		
		public void DequeuePatientEntry(MyMessage myMsg)
		{
			myMsg.Patient = EntryQueue.Dequeue(MySim.CurrentTime);
			myMsg.Patient.StopEntryWaiting();
		}
		
		public void EnqueueAfterEntryExam(Patient patient)
		{
			if (patient.Priority < 3)
			{
				MedicalTreatQueueA.Enqueue(patient, patient.Priority, patient.ArrivalTime, MySim.CurrentTime);
			}
			else
			{
				MedicalTreatQueueB.Enqueue(patient, patient.Priority, patient.ArrivalTime, MySim.CurrentTime);
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEDepartment(SimId.ManagerEDepartment, MySim, this);
			AddOwnMessage(Mc.EntranceTransmition);
			AddOwnMessage(Mc.TreatPatient);
			AddOwnMessage(Mc.MedicalTreatResources);
			AddOwnMessage(Mc.ExitTransmition);
			AddOwnMessage(Mc.EntryExamResources);
			AddOwnMessage(Mc.EntryExamPatient);
			AddOwnMessage(Mc.MedicalTreatPatient);
			AddOwnMessage(Mc.BetweenAmbulanceTransition);
		}
		//meta! tag="end"
	}
}