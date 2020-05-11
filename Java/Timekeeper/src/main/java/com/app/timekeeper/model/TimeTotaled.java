package com.app.timekeeper.model;

/**
 * Used for returning data for totalTime queries
 */
public class TimeTotaled {
	private String _id;
	private int totalTime;
	
	public TimeTotaled(String _id, int totalTime) {
		this._id = _id;
		this.totalTime = totalTime;
	}
	
	public String getId() {
		return _id;
	}
	
	public int getTotalTime() {
		return totalTime;
	}
}
