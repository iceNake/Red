using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class AttackSystem : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponSO currentWeapon;

    [Header("HitBox")]
    public BoxCollider2D hitBoxWeapon;

    [Header("Variables")]
    public bool isAttacking;
    public bool canChainAttack;
    public int comboIndex;
    public float damage;
    public Vector2 dirKnocBack;
    public float forceKnockback;

    private Coroutine _attackCoroutine;
    private Coroutine _comboWindowCoroutine;

    private void Awake()
    {
        if (hitBoxWeapon == null)
            hitBoxWeapon = GetComponent<BoxCollider2D>();

        hitBoxWeapon.gameObject.SetActive(false);
    }

    #region Switch

    public void Attack(bool isGrounded, AttackDirection dir)
    {
        AttackSO attackSO = null;

        switch (dir)
        {
            case AttackDirection.Neutral:
                if (isGrounded)
                {
                    GroundedNeutral();
                    return;
                }
                attackSO = currentWeapon.AirNeutralAttackSos;
                break;

            case AttackDirection.Up:
                attackSO = isGrounded
                    ? currentWeapon.GroundedUpAttackSos
                    : currentWeapon.AirUpAttackSOs;
                break;

            case AttackDirection.Down:
                attackSO = isGrounded
                    ? currentWeapon.GroundedDownAttackSos
                    : currentWeapon.AirDownAttackSos;
                break;

            case AttackDirection.Left:
                attackSO = isGrounded
                    ? currentWeapon.GroundedLeftAttackSos
                    : currentWeapon.AirLeftAttackSos;
                break;

            case AttackDirection.Right:
                attackSO = isGrounded
                    ? currentWeapon.GroundedRightAttackSos
                    : currentWeapon.AirRightAttackSos;
                break;
        }

        if (attackSO != null)
        {
            SingleAttacks(attackSO);
        }
    }

    #endregion

    #region NormalAttacks


    public void SingleAttacks(AttackSO attackSO)
    {
        if (canChainAttack & isAttacking)
        {
            ChainSingle(attackSO);
            return;
        }
        if (_attackCoroutine != null) return;

        StartSingleAttack(attackSO);
    }
    public void StartSingleAttack(AttackSO attackSO)
    {
        Debug.Log("Atacando con" +  attackSO);
            CancelCurrentAttack();
        _attackCoroutine = StartCoroutine(AttackCoroutine(attackSO));
    }
    void ChainSingle(AttackSO attackSO)
    {
        canChainAttack = false;
        StartSingleAttack(attackSO);
    }


    #endregion

    #region ComboNeutral

    public void GroundedNeutral()
    {
        if (canChainAttack)
        {
            ChainNeutral();
            return;
        }

        if (_attackCoroutine != null) return;

        StartNeutral();
    }

    void StartNeutral()
    {
        CancelCurrentAttack(false);

        AttackSO attackSO = currentWeapon.GroundedNeutralAttackSos[comboIndex];
        _attackCoroutine = StartCoroutine(AttackCoroutine(attackSO));
    }

    void ChainNeutral()
    {
        comboIndex++;

        if (comboIndex >= currentWeapon.GroundedNeutralAttackSos.Length)
            comboIndex = 0;

        canChainAttack = false;
        StartNeutral();
    }

    #endregion

    #region Corrutinas

    IEnumerator AttackCoroutine(AttackSO attackSO)
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackSO.startup);
        HitBox hitbox = hitBoxWeapon.GetComponent<HitBox>();
        hitbox.Init(this, gameObject);
        hitBoxWeapon.size = attackSO.rangeAttack;
        hitBoxWeapon.gameObject.SetActive(true);
        damage = attackSO.damage;
        dirKnocBack = attackSO.direction;
        forceKnockback = attackSO.forceKnockback;

        Debug.Log("Da�o es de " + damage);

        yield return new WaitForSeconds(attackSO.durationAttack);

        hitBoxWeapon.gameObject.SetActive(false);

        OpenComboWindow(attackSO);

        yield return new WaitForSeconds(attackSO.endlag);

        isAttacking = false;
        _attackCoroutine = null;

        Debug.Log("Finish Attack");
    }

    void OpenComboWindow(AttackSO attackSO)
    {
        if (_comboWindowCoroutine != null)
            StopCoroutine(_comboWindowCoroutine);

        _comboWindowCoroutine = StartCoroutine(ComboWindowCoroutine(attackSO));
    }

    IEnumerator ComboWindowCoroutine(AttackSO attackSO)
    {
        canChainAttack = true;
        Debug.Log("Combo Open");

        yield return new WaitForSeconds(attackSO.comboWindow);

        canChainAttack = false;
        comboIndex = 0;

        Debug.Log("Combo Close");
    }

    #endregion

    #region Limpiador de corrutinas etc

    void CancelCurrentAttack(bool resetCombo = true)
    {
        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);

        if (_comboWindowCoroutine != null)
            StopCoroutine(_comboWindowCoroutine);

        hitBoxWeapon.gameObject.SetActive(false);

        canChainAttack = false;

        if (resetCombo)
            comboIndex = 0;

        isAttacking = false;
        _attackCoroutine = null;
    }

    #endregion

    #region Test

    [Button("attackGRoundedUp")]
    public void TestGroundedUp()
    {
        Attack(true, AttackDirection.Up);
    }

    [Button("attackGrounded Neutral")]
    public void TestNeutral()
    {
        Attack(true, AttackDirection.Neutral);
    }

    #endregion


}