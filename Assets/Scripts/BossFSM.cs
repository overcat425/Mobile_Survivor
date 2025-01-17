using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Estate { Run, Attack, Dead }
public class BossFSM : MonoBehaviour
{
    public State currentState { get; set; }     // ���� ������ ������ �ִ� ���� ����
    private readonly BossScript controll; // ���� ������ ���� BossScriptŬ������ �ν��Ͻ��� ������
    public BossFSM(BossScript controller)
    {                                   // BossScript�� �ν��Ͻ��� �޾� controll�� �Ҵ���.
        controll = controller;
    }
    public void ChangeState(State state)    // ������ �ٸ� ���·� ��ȯ�ϴ� �޼ҵ�
    {
        if (currentState != null) currentState.ExitState(); // ���� �������� ���°� ������ Exit
        currentState = state;       // ���ο� ���¸� ����State�� �Ҵ�
        currentState.EnterState();      // ���ο� ������ EnterState ȣ��
    }
}