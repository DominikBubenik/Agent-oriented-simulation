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
			patient.StartEntryQueueWait();
			patient.PatientStatus = PatientStatus.EntryQueue;
			EntryQueue.Enqueue(patient, patient.Priority, patient.ArrivalTime, MySim.CurrentTime);
			if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"{patient.Name} enqueued, waiting for resources");
		}
		
		public void DequeuePatientEntry(MyMessage myMsg)
		{
			myMsg.Patient = EntryQueue.Dequeue(MySim.CurrentTime);
			myMsg.Patient.StopEntryWaiting();
			myMsg.Patient.PatientStatus = PatientStatus.Moving;
			if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"{myMsg.Patient.Name} dequeued, ready for entry exam");
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
			if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"{myMsg.Patient.Name} dequeued, ready for medical treat");
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
			if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"{patient.Name} waiting for medical treat");
			
		}
		
		public Patient GetWaitingPatientForMedicalTreat(Patient patient)
		{
			return patient.Priority < 3 ? MedicalTreatQueueA.Pop() : MedicalTreatQueueB.Pop();
		}
		
		public void Reset()
		{
			EntryQueue.Reset(MySim.CurrentTime);
			MedicalTreatQueueA.Reset(MySim.CurrentTime);
			MedicalTreatQueueB.Reset(MySim.CurrentTime);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEDepartment(SimId.ManagerEDepartment, MySim, this);
			AddOwnMessage(Mc.TreatPatient);
			AddOwnMessage(Mc.EntranceTransition);
			AddOwnMessage(Mc.EntryExamPatient);
			AddOwnMessage(Mc.BetweenAmbulanceTransition);
			AddOwnMessage(Mc.MedicalTreatPatient);
			AddOwnMessage(Mc.SendMedicalTreatResources);
			AddOwnMessage(Mc.SendEntryExamResources);
			AddOwnMessage(Mc.ExitTransition);
		}
		//meta! tag="end"
	}
}