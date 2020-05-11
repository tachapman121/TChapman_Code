package com.app.timekeeper.model;

/**
 * Used for returning data for totalTime queries
 */
public class TimeTotaled {
	private String id;
	private float totalTime;
	
	public TimeTotaled(String id, float totalTime) {
		this.id = id;
		this.totalTime = totalTime;
	}
	
	public String getId() {
		return id;
	}
	
	public float getTotalTime() {
		return totalTime;
	}
}
