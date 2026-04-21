using Agents.AgentResources;
using OSPABA;
using Simulation;

namespace Agents.AgentResources.InstantAssistants
{
	/*!
	 * otazka tu pomocou nich alokojem zdroje napriklad tie ktore su najmenej vytazene??
	 */
	//meta! id="82"
	public class AllocateEntryExamRes : OSPABA.Adviser
	{
		public AllocateEntryExamRes(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void Execute(MessageForm message)
		{
		}
		public new AgentResources MyAgent
		{
			get
			{
				return (AgentResources)base.MyAgent;
			}
		}
	}
}
