package com.app.timekeeper.model;

import java.util.Date;

import org.springframework.data.annotation.CreatedDate;

import lombok.Data;

/**
 * Represents Time in database, stored as part of TimeEntity
 * ["time": 0, "addDate": ISODate()]
 */
@Data
public class Time {
	private float time;
	@CreatedDate
	private Date addDate;
	
	public float getHours() {
		return time;
	}

	public void setHours(float time) {
		this.time = time;
	}

	public Date getDate() {
		return addDate;
	}

	public void setDate(Date date) {
		this.addDate = date;
	}

	public Time(float time) {
		this.time = time;
		this.addDate = new Date();
	}
	
	public Time() {
		
	}
}
