package com.app.timekeeper.service;

import java.util.List;

import com.app.timekeeper.model.Time;
import com.app.timekeeper.model.TimeEntity;

public interface TimeEntityService {
	/**
	 * Adds entity to database
	 * @param entity
	 * @return Returns true if successful, false otherwise
	 */
	boolean addEntity(TimeEntity entity);
	
	/**
	 * Add time to existing entity
	 * @param name Name to add
	 * @param time time in hours to add
	 * @return Total time added to entity if successful >= 0; -1 if entity does not exist, -2 if other error
	 */
	int addTime(String name, float hours);
	
	/**
	 * Add time to existing entity
	 * @param id ObjectId of database entity
	 * @param time time in hours to add
	 * @return Total time added to entity if successful >= 0; -1 if entity does not exist, -2 if other error
	 */
	int addTime(Long id, float hours);
	
	/**
	 * Displays time for week from previous Sunday to current date
	 * @return List of times for week
	 */
	List<com.app.timekeeper.model.TimeTotaled> timeForWeek();
	
	/**
	 * Shows total time for given entity
	 * @param name Name of entity
	 * @return List<Time> containing times and dates; null if entity does not exist in database
	 */
	List<Time> timeForEntity(String name);
	
	/**
	 * Shows total time for given entity
	 * @param id ObjectId in database
	 * @return List<Time> containing times and dates; null if entity does not exist in database
	 */
	List<Time> timeForEntity(Long id);

}