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
		
		public void EnqueuePatientEntry(Patient patient)
		{
			// GlobalLogger.PrintLog(patient.ToString() + "is entring queue of length " + EntryQueue.Count, MySim.CurrentTime);
			patient.StartEntryQueueWait();
			patient.PatientStatus = PatientStatus.EntryQueue;
			EntryQueue.Enqueue(patient, patient.Priority, patient.ArrivalTime, MySim.CurrentTime);
		}
		
		public void DequeuePatientEntry(MyMessage myMsg)
		{
			myMsg.Patient = EntryQueue.Dequeue(MySim.CurrentTime);
			myMsg.Patient.StopEntryWaiting();
			myMsg.Patient.PatientStatus = PatientStatus.Moving;
		}
		
		public void DequeuePatientMedicalTreat(MyMessage myMsg)
		{
			if (myMsg.Patient.Priority < 3 && myMsg.Room.IsTypeA())
			{
				myMsg.Patient = MedicalTreatQueueA.Dequeue(MySim.CurrentTime);
			}
			else
			{
				myMsg.Patient = MedicalTreatQueueB.Dequeue(MySim.CurrentTime);
			}
			myMsg.Patient.PatientStatus = PatientStatus.Moving;
			myMsg.Patient.StopMedicalQueueWaiting();
		}
		
		public void EnqueueAfterEntryExam(Patient patient)
		{
			patient.StartMedicalQueueWait();
			if (patient.Priority < 3)
			{
				MedicalTreatQueueA.Enqueue(patient, patient.Priority, patient.ArrivalTime, MySim.CurrentTime);
			}
			else
			{
				MedicalTreatQueueB.Enqueue(patient, patient.Priority, patient.ArrivalTime, MySim.CurrentTime);
			}

			patient.PatientStatus = PatientStatus.MedicalWait;
		}
		
		public Patient GetWaitingPatientForMedicalTreat(Patient patient)
		{
			return patient.Priority < 3 ? MedicalTreatQueueA.Pop() : MedicalTreatQueueB.Pop();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEDepartment(SimId.ManagerEDepartment, MySim, this);
			AddOwnMessage(Mc.EntranceTransition);
			AddOwnMessage(Mc.TreatPatient);
			AddOwnMessage(Mc.EntryExamPatient);
			AddOwnMessage(Mc.BetweenAmbulanceTransition);
			AddOwnMessage(Mc.SendMedicalTreatResources);
			AddOwnMessage(Mc.MedicalTreatPatient);
			AddOwnMessage(Mc.SendEntryExamResources);
			AddOwnMessage(Mc.ExitTransition);
		}
		//meta! tag="end"
	}
}