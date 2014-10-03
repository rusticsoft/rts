﻿using UnityEngine;
using System.Collections;
public enum LEG_STATE { DEFAULT, LIFTING, LOWERING }
public class SimpleLeg : MonoBehaviour {

	public SliderJoint2D thigh;
	public SliderJoint2D foot;
	public float maxLiftForce;
	public float desiredLiftSpeed;
	JointMotor2D liftMotor;
	JointMotor2D lowerMotor;
	public float maxAdvanceForce;
	public float desiredAdvanceSpeed;
	JointMotor2D advanceMotor;
	JointMotor2D retractMotor;
	public LEG_STATE legState = LEG_STATE.DEFAULT;
	public bool isFullyLifted() {
		return (thigh.limitState == JointLimitState2D.LowerLimit);
	}
	public bool isFullyLowered() {
		return (thigh.limitState == JointLimitState2D.UpperLimit);
	}
	public bool isAdvanced() {
		return (foot.limitState == JointLimitState2D.UpperLimit);
	}
	public bool isAdvancedOpposed() {
		return (foot.limitState == JointLimitState2D.LowerLimit);
	}
	public bool isPastCenterOfMass() {
		return (foot.jointTranslation > 0f);
	}
	public bool isPastCenterOfMassOpposed() {
		return (foot.jointTranslation < 0f);
	}


	// Use this for initialization
	void Start () {
		liftMotor.maxMotorTorque = maxLiftForce;
		liftMotor.motorSpeed = -desiredLiftSpeed;
		lowerMotor.maxMotorTorque = maxLiftForce;
		lowerMotor.motorSpeed = desiredLiftSpeed;
		advanceMotor.maxMotorTorque = maxAdvanceForce;
		advanceMotor.motorSpeed = desiredAdvanceSpeed;
		retractMotor.maxMotorTorque = maxAdvanceForce;
		retractMotor.motorSpeed = -desiredAdvanceSpeed;
	}

	public void lift() {
		legState = LEG_STATE.LIFTING;
		thigh.motor = liftMotor;
		thigh.useMotor = true;
	}
	
	public void lower() {
		legState = LEG_STATE.LOWERING;
		thigh.motor = lowerMotor;
		thigh.useMotor = true;
	}
	
	public void advance () {
		foot.motor = advanceMotor;
		foot.useMotor = true;
	}

	//TODO: this will likely go all screwy on inclines
	public static SimpleLeg getHigher(SimpleLeg first, SimpleLeg other) {
		if(first.thigh.transform.localPosition.y > other.thigh.transform.localPosition.y) {
			return first;
		}
		return other;
	}
	
	public void advanceOpposed () {
		foot.motor = retractMotor;
		foot.useMotor = true;
	}
	
	public void relax () {
		foot.useMotor = false;
	}

	// Update is called once per frame
	void Update () {
		liftMotor.maxMotorTorque = maxLiftForce;
		liftMotor.motorSpeed = -desiredLiftSpeed;
		lowerMotor.maxMotorTorque = maxLiftForce;
		lowerMotor.motorSpeed = desiredLiftSpeed;
		advanceMotor.maxMotorTorque = maxAdvanceForce;
		advanceMotor.motorSpeed = desiredAdvanceSpeed;
		retractMotor.maxMotorTorque = maxAdvanceForce;
		retractMotor.motorSpeed = -desiredAdvanceSpeed;
	}
}
