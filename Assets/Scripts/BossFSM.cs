using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Estate { Run, Attack, Dead }
public class BossFSM : MonoBehaviour
{
    public State currentState { get; set; }     // 현재 보스가 가지고 있는 상태 저장
    private readonly BossScript controll; // 보스 로직을 담은 BossScript클래스의 인스턴스를 참조함
    public BossFSM(BossScript controller)
    {                                   // BossScript의 인스턴스를 받아 controll에 할당함.
        controll = controller;
    }
    public void ChangeState(State state)    // 보스가 다른 상태로 전환하는 메소드
    {
        if (currentState != null) currentState.ExitState(); // 현재 실행중인 상태가 있으면 Exit
        currentState = state;       // 새로운 상태를 현재State에 할당
        currentState.EnterState();      // 새로운 상태의 EnterState 호출
    }
}