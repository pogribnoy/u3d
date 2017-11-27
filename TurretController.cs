public class TurretController{
	void Update() {
		//rotate turret
		// т.к. нам необходимо вращать только вокруг оси Y, то остальные локальные углы вращения надо сохранить
		Vector3 tmpRotation = transform.localRotation; 
		Vector3 leadTargetPosition = FirstOrderIntercept(transform.position,Vector3.zero,laserParticleLeft.particleEmitter.localVelocity.z,target.transform.position,target.rigidbody.velocity);
		targetPointTurret = (leadTargetPosition - transform.position).normalized; //get normalized vector toward target
		targetRotationTurret = Quaternion.LookRotation (targetPointTurret, parent.transform.up); //get a rotation for the turret that looks toward the target
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationTurret, Time.deltaTime * turnSpeed); //gradually turn towards the target at the specified turnSpeed
		transform.localRotation = Quaternion.Euler( tmpRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, tmpRotation.eulerAngles.z); //reset x and z rotations and only rotates the y on its local axis
		//rotate barrels
		tmpRotation = barrels.transform.localRotation; //preserves the local rotation since we only want to rotate on local x axis
		targetPointBarrels = (leadTargetPosition - barrels.transform.position).normalized; //get a normalized vector towards the target
		targetRotationBarrels = Quaternion.LookRotation (targetPointBarrels, parent.transform.up); //get a rotation that looks at the target
		barrels.transform.rotation = Quaternion.Slerp(barrels.transform.rotation, targetRotationBarrels, Time.deltaTime * turnSpeed); //gradually turn towards the target as the specified turnSpeed
		barrels.transform.localRotation = Quaternion.Euler( barrels.transform.localRotation.eulerAngles.x, tmpRotation.eulerAngles.y, tmpRotation.eulerAngles.z); //reset y and z rotations and only rotates the x on its local axis
	}
	
	//first-order intercept using absolute target position
	public static Vector3 FirstOrderIntercept(Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity) {
		Vector3 targetRelativePosition = targetPosition - shooterPosition;
		Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
		float t = FirstOrderInterceptTime ( shotSpeed, targetRelativePosition, targetRelativeVelocity);
		return targetPosition + t*(targetRelativeVelocity);
	}
	
	//first-order intercept using relative target position
	public static float FirstOrderInterceptTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity) {
		float velocitySquared = targetRelativeVelocity.sqrMagnitude;
		if(velocitySquared < 0.001f)
			return 0f;
	 
		float a = velocitySquared - shotSpeed*shotSpeed;
	 
		//handle similar velocities
		if (Mathf.Abs(a) < 0.001f) {
			float t = -targetRelativePosition.sqrMagnitude / ( 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition));
			return Mathf.Max(t, 0f); //don't shoot back in time
		}
	 
		float b = 2f*Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
		float c = targetRelativePosition.sqrMagnitude;
		float determinant = b*b - 4f*a*c;
	 
		if (determinant > 0f) { //determinant > 0; two intercept paths (most common)
			float	t1 = (-b + Mathf.Sqrt(determinant))/(2f*a);
			float t2 = (-b - Mathf.Sqrt(determinant))/(2f*a);
			if (t1 > 0f) {
				if (t2 > 0f)
					return Mathf.Min(t1, t2); //both are positive
				else
					return t1; //only t1 is positive
			} else
				return Mathf.Max(t2, 0f); //don't shoot back in time
		} else if (determinant < 0f) //determinant < 0; no intercept path
			return 0f;
		else //determinant = 0; one intercept path, pretty much never happens
			return Mathf.Max(-b/(2f*a), 0f); //don't shoot back in time
	}
}