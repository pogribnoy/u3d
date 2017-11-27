// ����� ���������, ���� ������ ���� ���������� ����� ���-�� ������� � ���������
public class State {
  public int code;
	public string name;
  private SortedDictionary<int, int> transitions;
	
	public State(int code, string name, int[] transitions = null) {
		this.code = code;
		this.name = name;
		this.transitions = transitions;
	}
};

public class Event {
  public int code;
	
	public Event(int code) {
		this.code = code;
	}
};

public class StateMachine {
	public SortedDictionary<int, SortedDictionary<int, int>> states = new SortedDictionary<int, new SortedDictionary<int, int>()>();
  public int currentState;
	
  public StateMachine() {
		//states.Add(new State(0))
	}
	
	public addStateTransitions(int fromStateCode, int eventCode, int toStateCode) {
		SortedDictionary<int, int> trans; // �������� ��� ���������
		if (!states.TryGetValue(fromStateCode, out trans)){ // ������ ��������� ���, �������
			states[fromStateCode] = new SortedDictionary<int, int>());
			trans = new SortedDictionary<int, int>();
		}
		// ��������� ����
		// ��������� �������
		states[fromStateCode][eventCode] = toStateCode;
	}
	
	public void setState( int stateCode ){
		if(states.ContainsKey(stateCode)) currentState = stateCode;
		else Debug.Log("setState: state "+ stateCode.ToString() + " coudn't be set, because it not found for StateMachine");
	}
	
  public int dispatchEvent( int eventCode ){
		SortedDictionary<int, int> trans = states[currentState];
		if (trans.ContainsKey(eventCode)) setState(trans[eventCode]);
		else Debug.Log("dispatchEvent: event "+ eventCode.ToString() + " coudn't be dispatched, because it not found for state");
		return currentState;
	}
	
	// ������� �� ���������� ��� �������, ������������, ����� ��� ���������
	public int nextState(){
		SortedDictionary<int, int> trans = states[currentState];
		//SortedDictionary<int, int>.KeyCollection keyColl = trans.Keys;
		
		foreach(int st in trans.Keys) {
			setState(st); return currentState;
		}
	}
};

// ������ ������������� ��� ������
class Rocket{
	private enum StateTypes{
		IDLE, // ������ �� �������
		MOVE, // �����
		TIMEOUT, // ����� ��� �������
		DESTROY // �����
	}
	private enum EventTypes{
		START,	// ������ ������
		TIMEOUT, // ����������� �������
		DESTROY, // �����
		RESET // ������� � ��������� ���������
	}
	private StateMachine sm = new StateMachine();;
	
	void Awake(){
		sm.addStateTransition(StateTypes.IDLE, EventTypes.START, StateTypes.MOVE); // ������
		sm.addStateTransition(StateTypes.MOVE, EventTypes.TIMEOUT, StateTypes.TIMEOUT); // ����������� �������
		sm.addStateTransition(StateTypes.MOVE, EventTypes.DESTROY, StateTypes.DESTROY); // ������ � ����, ��������
		sm.addStateTransition(StateTypes.TIMEOUT, EventTypes.DESTROY, StateTypes.DESTROY); // ������ � ���� ��� ����������� �����, ��������
		sm.addStateTransition(StateTypes.DESTROY, EventTypes.RESET, StateTypes.IDLE); // ������ � ���� ��� ����������� �����, ��������
		
		sm.setState(StateTypes.IDLE);
	}
	void Update() {
		int newState = sm.dispatchEvent(EventTypes.START);
		switch(newState){
		case StateTypes.IDLE:
			// ������ ���-��, ���������  ������ ����������
			break;
		case StateTypes.MOVE:
			// ������ ���-��, ���������  ������ ����������
			break;
		default:
			break;
		};
	}
}