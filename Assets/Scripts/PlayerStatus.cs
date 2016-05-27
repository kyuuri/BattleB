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

	public float class1 = 0;
	public float class2 = 0;
	public float class3 = 0;
	public float class4 = 0;

	public int pointClass;

	public PlayerController.PlayerClass currentClass;

	public PlayerStatus(PlayerController.PlayerClass playerClass){

		SetClass (playerClass);
	}

	public bool SetClass(PlayerController.PlayerClass playerClass){
		bool isNew = false;
		if (currentClass != playerClass) {
			isNew = true;
		}

		currentClass = playerClass;

		if (playerClass == PlayerController.PlayerClass.NOVICE) {
			MaxHp = 100;
			MoveSpeed = 3.0f;
			bulletSpeed = 4.0f;
			fireSpeed = 0.5f;
			damage = 15;
		} else if (playerClass == PlayerController.PlayerClass.SHOTGUN) {
			MaxHp = 120;
			MoveSpeed = 3.0f;
			bulletSpeed = 5;
			fireSpeed = 0.15f;
			damage = 6;
		} else if (playerClass == PlayerController.PlayerClass.CANNON) {
			MaxHp = 120;
			MoveSpeed = 3.0f;
			bulletSpeed = 4.0f;
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
			fireSpeed = 0.9f;
			damage = 70;
		} else if (playerClass == PlayerController.PlayerClass.SHOTGUNCANNON) {
			MaxHp = 100;
			MoveSpeed = 3.1f;
			//bulletSpeed = 11;
			fireSpeed = 0.5f;
			damage = 20;
		}

		MaxHp *= (pointMaxHp / 7.0f + 1);
		MoveSpeed *= (pointMoveSpeed/ 10.0f + 1);
		bulletSpeed *= (pointbulletSpeed / 7.0f + 1);
		fireSpeed /= (pointfireSpeed / 14.0f + 1);

		return isNew;
	}
}
