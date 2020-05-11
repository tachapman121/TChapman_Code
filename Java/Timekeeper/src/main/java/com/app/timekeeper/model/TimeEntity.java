package com.app.timekeeper.model;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import javax.persistence.GeneratedValue;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import com.fasterxml.jackson.annotation.JsonIgnore;

/**
 * Represents entity in database and associated times
 */
@Document(collection="entity")
public class TimeEntity {
	@Id
	@JsonIgnore
	@GeneratedValue
	private String _id;
	private String name;
	
	@CreatedDate
	private Date firstDate;
	private String type;
	private List<Time> times;
	
	public String get_id() {
		return _id;
	}

	public void set_id(String _id) {
		this._id = _id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Date getFirstDate() {
		return firstDate;
	}

	public void setFirstDate(Date firstDate) {
		this.firstDate = firstDate;
	}

	public String getType() {
		return type;
	}

	public void setType(String type) {
		this.type = type;
	}

	public List<Time> getTimes() {
		return times;
	}

	public void setTimes(List<Time> times) {
		this.times = times;
	}
	
	public TimeEntity() {
		this.firstDate = new Date();
		times = new ArrayList<Time>();
	}
	
	public TimeEntity(String name, String type) {
		this.name = name;
		this.type = type;
		this.firstDate = new Date();
		times = new ArrayList<Time>();
	}
	
	public TimeEntity(String name, String type, List<Time> times) {
		this.name = name;
		this.times = times;
		this.type = type;
		this.firstDate = new Date();
	}
	
	/**
	 * Returns total time spent on entity
	 * @return
	 */
	public int getTotalTime() {
		int hours = 0;
		for(Time time : times)
			hours += time.getHours();
		
		return hours;
	}
}
