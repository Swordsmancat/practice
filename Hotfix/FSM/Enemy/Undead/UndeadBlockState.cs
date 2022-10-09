using Farm.Hotfix;
using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
public class UndeadBlockState : EnemyBlockState
{
    private readonly static int Block = Animator.StringToHash("Block");
    private readonly static int BlockWalkState = Animator.StringToHash("BlockWalkState");
    private readonly static float ExitTime = 3f;
    private TargetableObject m_Plyaer;
    private UndeadLogic owner;

    // Start is called before the first frame update
    protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        owner = procedureOwner.Owner as UndeadLogic;
        owner.SetRichAiStop();
        owner.m_Animator.SetBool(Block, true);
        int num = Utility.Random.GetRandom(0, 3);
       
        owner.m_Animator.SetInteger(BlockWalkState, Utility.Random.GetRandom(0, 2));
        owner.IsBlock = true;
    }

    // Update is called once per frame
    protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        m_Plyaer = owner.LockingEntity;
        //AIUtility.RotateToTarget(m_Plyaer, owner, -15f, 15f);

        owner.BlockTime += elapseSeconds;
        if (owner.BlockTime >= ExitTime)
        {
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
        }
    }
    protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        owner.IsAnimPlayed = false;
        owner.IsBlock = false;
        owner.m_Animator.SetBool(Block, false);
        owner.m_Animator.SetInteger(BlockWalkState, -1);
    }
    private void Blockcancel()
    {
        if (owner.IsBlock)
        {
            owner.BoxColliderLeft.enabled = false;
        }
    }
    public static UndeadBlockState Create()
    {
        UndeadBlockState state = ReferencePool.Acquire<UndeadBlockState>();
        return state;
    }

}
