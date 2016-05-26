using UnityEngine;
using System.Collections;

public class PlayerStatus{

	public float MaxHp;
	public float MoveSpeed;
	public float bulletSpeed;
	public float fireSpeed;

	public float damage;

	public int level = 0;
	public float exp = 0;
	public float pointMaxHp = 0;
	public float pointMoveSpeed = 0;
	public float pointbulletSpeed = 0;
	public float pointfireSpeed = 0;

	public int pointLeft;

	public PlayerStatus(PlayerController.PlayerClass playerClass){

		SetClass (playerClass);
	}

	public void SetClass(PlayerController.PlayerClass playerClass){
		if (playerClass == PlayerController.PlayerClass.NOVICE) {
			MaxHp = 100;
			MoveSpeed = 3.0f;
			bulletSpeed = 6.0f;
			fireSpeed = 0.5f;
			damage = 15;
		} else if (playerClass == PlayerController.PlayerClass.SHOTGUN) {
			MaxHp = 120;
			MoveSpeed = 3.0f;
			bulletSpeed = 11;
			fireSpeed = 0.15f;
			damage = 11;
		} else if (playerClass == PlayerController.PlayerClass.CANNON) {
			MaxHp = 120;
			MoveSpeed = 3.0f;
			bulletSpeed = 6.0f;
			fireSpeed = 0.65f;
			damage = 35;
		} else if (playerClass == PlayerController.PlayerClass.BLADER) {
			MaxHp = 80;
			MoveSpeed = 3.0f;
			bulletSpeed = 11;
			fireSpeed = 0.15f;
			damage = 5;
		} else if (playerClass == PlayerController.PlayerClass.SNIPER) {
			MaxHp = 100;
			MoveSpeed = 3.0f;
			//bulletSpeed = 11;
			fireSpeed = 0.75f;
			damage = 70;
		}

		MaxHp *= (pointMaxHp / 5.0f + 1);
		MoveSpeed *= (pointMoveSpeed/ 5.0f + 1);
		bulletSpeed *= (pointbulletSpeed / 5.0f + 1);
		fireSpeed *= (pointfireSpeed / 5.0f + 1);
	}
}
