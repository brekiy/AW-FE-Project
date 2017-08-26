using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

/// <summary>
/// Base class for all units in the game.
/// </summary>
public class FEUnit : Unit {

  /// <summary>
  /// Cell that the unit is currently occupying.
  /// </summary>
  public Color LeadingColor;

  //all stats aside from hitpoints should be capped at 50
  public string UnitName;
  public int MeleeAttack; //damage dealt with hand weapons
  public int RangedAttack; //damage dealt with missile weapons
  public int MagicAttack; //damage dealt with magic
  public int RangedRange; //range of missile weapons
  public int MagicRange; //range of magic weapons
  public int Ammunition; //ammunition for missile weapons
  public int MeleeDefense; //chance to not be hit in melee
  public int Armor; //damage reduction from hand/missile weapons
  public int Resistance; //damage reduction from magic
  public int Morale; //unit morale, affects combat stats
  public AttackType CurrentAttack = AttackType.nullattack;


  /// <summary>
  /// Method called after object instantiation to initialize fields etc. 
  /// </summary>
  public override void Initialize(){
    base.Initialize();
    transform.position += new Vector3(0, 0, -1);
    GetComponent<Renderer>().material.color = LeadingColor;
  }

  /// <summary>
  /// Method indicates if it is possible to attack unit given as parameter, from cell given as second parameter.
  /// </summary>
  public override bool IsUnitAttackable(Unit other, Cell sourceCell){
    if (sourceCell.GetDistance(other.Cell) <= 1 
      || sourceCell.GetDistance(other.Cell) <= RangedRange
      || sourceCell.GetDistance(other.Cell) <= MagicRange)
      return true;
    else return false;
  }

  public void UnitAttackMode(FEUnit unit, Unit other, Cell sourceCell) {
    GameObject AttackPanel = GameObject.Find("Attack Panel");
    AttackPanel.SetActive(true);
    Button Melee = GameObject.Find("Select Melee Attack").GetComponent<Button>();
    Button Ranged = GameObject.Find("Select Ranged Attack").GetComponent<Button>();
    Button Magic = GameObject.Find("Select Magic Attack").GetComponent<Button>();
    //Melee.onClick
  }

  /// <summary>
  /// Method deals damage to unit given as parameter.
  /// </summary>
  public void DealDamage(FEUnit other){
    if (isMoving) return;
    if (ActionPoints == 0) return;
    if (!IsUnitAttackable(other, Cell)) return;
    UnitAttackMode(this, other, Cell);

    MarkAsAttacking(other);
    ActionPoints--;
    switch (this.CurrentAttack){
      case AttackType.melee:
        other.Defend(this, MeleeAttack, AttackType.melee); break;
      case AttackType.ranged:
        other.Defend(this, RangedAttack, AttackType.ranged); break;
      case AttackType.magic:
        other.Defend(this, RangedAttack, AttackType.ranged); break;
      default:
        other.Defend(this, RangedAttack, AttackType.ranged); break;
    }

    if (ActionPoints == 0) {
      SetState(new UnitStateMarkedAsFinished(this));
      MovementPoints = 0;
    }  
  }
  /// <summary>
  /// Attacking unit calls Defend method on defending unit. 
  /// </summary>
  protected void Defend(FEUnit other, int damage, AttackType attack){
      MarkAsDefending(other);
      int DamageTaken = 0;
      switch (attack) {
          case AttackType.melee:
              if (this.MeleeDefense < UnityEngine.Random.Range(0, 50)) DamageTaken = 0;
              else DamageTaken = Mathf.Clamp(damage - Armor, 1, damage);
              break;
          case AttackType.ranged:
              DamageTaken = Mathf.Clamp(damage - Armor, 1, damage);
              break;
          case AttackType.magic:
              DamageTaken = Mathf.Clamp(damage - Resistance, 1, damage);
              break;
          default: DamageTaken = 0; break;
      }
      HitPoints -= DamageTaken;  //Damage is calculated by subtracting attack factor of attacker and defence factor of defender.
                                  //If result is below 1, it is set to 1.
                                  //This behaviour can be overridden in derived classes.
      //if (this.UnitAttacked != null)
        //  UnitAttacked.Invoke(this, new AttackEventArgs(other, this, damage));

      if (HitPoints <= 0){
          //if (UnitDestroyed != null)
            //  UnitDestroyed.Invoke(this, new AttackEventArgs(other, this, damage));
          OnDestroyed();
      }
  }

  /// <summary>
  /// Gives visual indication that the unit is under attack.
  /// </summary>
  /// <param name="other"></param>
  public override void MarkAsDefending(Unit other) { }
  /// <summary>
  /// Gives visual indication that the unit is attacking.
  /// </summary>
  /// <param name="other"></param>
  public override void MarkAsAttacking(Unit other) { }
  /// <summary>
  /// Gives visual indication that the unit is destroyed. It gets called right before the unit game object is
  /// destroyed, so either instantiate some new object to indicate destruction or redesign Defend method. 
  /// </summary>
  public override void MarkAsDestroyed() { }

  /// <summary>
  /// Method marks unit as current players unit.
  /// </summary>
  public override void MarkAsFriendly() {
      SetColor(new Color(0.8f, 1, 0.8f));
  }
  /// <summary>
  /// Method mark units to indicate user that the unit is in range and can be attacked.
  /// </summary>
  public override void MarkAsReachableEnemy() {
      SetColor(new Color(1,0.8f,0.8f));
  }
  /// <summary>
  /// Method marks unit as currently selected, to distinguish it from other units.
  /// </summary>
  public override void MarkAsSelected() {
      SetColor(new Color(0.8f, 0.8f, 1));
  }
  /// <summary>
  /// Method marks unit to indicate user that he can't do anything more with it this turn.
  /// </summary>
  public override void MarkAsFinished(){
      SetColor(Color.gray);
  }
  /// <summary>
  /// Method returns the unit to its base appearance
  /// </summary>
  public override void UnMark() {
      SetColor(Color.white);
  }

  private void SetColor(Color color) {
    var _renderer = GetComponent<SpriteRenderer>();
    if (_renderer != null) {
      _renderer.color = color;
    }
  }

};

public enum AttackType{
  melee,
  ranged,
  magic, 
  nullattack
}